using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using CgmInfoGui.ViewModels.Nodes.Sources;

namespace CgmInfoGui.ViewModels.Nodes
{
    public sealed class TiffGenerator
    {
        private readonly ITileSource _tile;
        private readonly MemoryStream _stream = new MemoryStream();

        public TiffGenerator(ITileSource tile)
        {
            _tile = tile ?? throw new ArgumentNullException(nameof(tile));
        }

        private void WriteImageFileHeader(BinaryWriter writer)
        {
            // Byte-order identifer
            writer.WriteChars(BitConverter.IsLittleEndian ? "II" : "MM");

            // TIFF version number (always 42)
            writer.WriteShort(42);

            // first Image File Directory offset. we'll put it right next to this; so simply point 4 bytes forward.
            writer.WriteInteger((int)writer.BaseStream.Position + 4);
        }

        private void WriteImageFileDirectory(BinaryWriter writer, IEnumerable<IFDEntry> entries)
        {
            int WriteArray(object[] values, short type)
            {
                long startOffset = writer.BaseStream.Position;

                switch (type)
                {
                    case TiffField.DataType.Short:
                        foreach (short s in values)
                            writer.WriteShort(s);
                        break;

                    case TiffField.DataType.Long:
                        foreach (int i in values)
                            writer.WriteInteger(i);
                        break;

                    default:
                        throw new NotSupportedException($"Cannot write arrays of type {type}.");
                }

                return (int)(writer.BaseStream.Position - startOffset);
            }

            var entriesToWrite = entries;
            // there must be one entry of type StripOffsets; it tells where the data is
            if (!entries.Any(e => e.Field == TiffField.StripOffsets))
                entriesToWrite = entriesToWrite.Concat(new[] { new IFDEntry(TiffField.StripOffsets, -1) });

            // Number of IFD entries
            writer.WriteShort((short)entriesToWrite.Count());

            // entries that don't fit inline and need an offset stored
            var writeLater = new Dictionary<long, IFDEntry>();

            long dataOffset = -1;
            foreach (var entry in entriesToWrite.OrderBy(e => e.Field.Tag))
            {
                // Field Tag
                writer.WriteShort(entry.Field.Tag);
                // Field type
                writer.WriteShort(entry.Field.Type);
                // Number of values
                writer.WriteInteger(entry.NumberOfValues);

                if (entry.Field == TiffField.StripOffsets)
                    dataOffset = writer.BaseStream.Position;

                if (entry.StoreInline)
                {
                    if (entry.NumberOfValues == 1)
                    {
                        int valueToWrite = entry.Value switch
                        {
                            short s => s,
                            int i => i,
                            long l => (int)l,
                            _ => throw new NotSupportedException($"Unsupported TIFF content type {entry.Value?.GetType().Name}."),
                        };
                        writer.WriteInteger(valueToWrite);
                    }
                    else
                    {
                        object[] values = (object[])entry.Value;
                        int bytesWritten = WriteArray(values, entry.Field.Type);
                        // make sure we fill the space for the IFD offset, this field must be of the correct size.
                        while (bytesWritten++ < IFDEntry.OffsetFieldSize)
                            writer.WriteByte(0);
                    }
                }
                else
                {
                    writeLater[writer.BaseStream.Position] = entry;
                    writer.WriteInteger(-1);
                }
            }

            // next IFD offset. we only write one, so thats the end.
            writer.WriteInteger(0);

            // write all entries that didn't fit earlier
            foreach (var (offset, longEntry) in writeLater)
            {
                long position = writer.BaseStream.Position;

                if (longEntry.Field.Type == TiffField.DataType.Rational)
                {
                    // assume rational is not fractional
                    int numerator = (int)longEntry.Value;
                    int denominator = 1;

                    writer.WriteInteger(numerator);
                    writer.WriteInteger(denominator);
                }
                else if (longEntry.Value is object[] values)
                {
                    WriteArray(values, longEntry.Field.Type);
                }
                else
                {
                    throw new NotSupportedException($"Unsupported TIFF Field Type {longEntry.Field.Type} for offset writing.");
                }

                long returnHere = writer.BaseStream.Position;
                writer.BaseStream.Position = offset;
                writer.WriteInteger((int)position);
                writer.BaseStream.Position = returnHere;
            }

            // write the target position 
            long dataPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = dataOffset;
            writer.WriteInteger((int)dataPosition);
            writer.BaseStream.Position = dataPosition;
        }
        public Stream CreateStream()
        {
            // FIXME: the generated header/structure in here is "good enough" to do something,
            //        but causes issues for some compression types (mostly CCITT).
            //        having /some/ support feels more useful than none at all, even though
            //        more than half of the realistically occurring types are broken.
            //        time might find a solution for the problem, so it stays here for later.
            using (var writer = new BinaryWriter(_stream, Encoding.ASCII, leaveOpen: true))
            {
                WriteImageFileHeader(writer);

                var entries = new List<IFDEntry>
                {
                    new IFDEntry(TiffField.NewSubfileType, 0),
                    new IFDEntry(TiffField.ImageWidth, _tile.Width),
                    new IFDEntry(TiffField.ImageLength, _tile.Height),
                    new IFDEntry(TiffField.Compression, ConvertCompression(_tile.CompressionType)),
                    new IFDEntry(TiffField.PhotometricInterpretation,0),
                    new IFDEntry(TiffField.RowsPerStrip, _tile.Width),
                    new IFDEntry(TiffField.StripByteCounts, _tile.CompressedCells.LongLength),
                    new IFDEntry(TiffField.XResolution, _tile.DpiX),
                    new IFDEntry(TiffField.YResolution, _tile.DpiY),
                    new IFDEntry(TiffField.ResolutionUnit, 2),
                };
                short colorPrecision = (short)_tile.ColorPrecision;
                if (_tile.IsBlackAndWhite)
                {
                    entries.Add(new IFDEntry(TiffField.SamplesPerPixel, 1));
                    entries.Add(new IFDEntry(TiffField.BitsPerSample, colorPrecision));
                }
                else
                {
                    entries.Add(new IFDEntry(TiffField.SamplesPerPixel, 3));
                    entries.Add(new IFDEntry(TiffField.BitsPerSample, new object[] { colorPrecision, colorPrecision, colorPrecision }));
                }
                WriteImageFileDirectory(writer, entries);
            }

            // Image data (straight from CGM)
            _stream.Write(_tile.CompressedCells, 0, _tile.CompressedCells.Length);

            _stream.Position = 0;
            return _stream;
        }

        private short ConvertCompression(int compressionType) => compressionType switch
        {
            // CCITT Group 4 T6
            2 => 4,
            // CCITT Group 3 T4 (1D)
            3 => 2,
            // CCITT Group 3 T4 (2D)
            4 => 3,
            // Bitmap (uncompressed)
            5 => 1,
            // Baseline JPEG
            7 => 6,
            // LZW
            8 => 5,
            _ => throw new NotSupportedException($"Compression type {compressionType} is not supported by TIFF"),
        };

        private sealed class IFDEntry
        {
            public IFDEntry(TiffField field, object value)
            {
                Field = field;
                Value = value;
                if (Value is object[] values)
                    NumberOfValues = values.Length;
                else
                    NumberOfValues = 1;
            }
            public TiffField Field { get; }
            public object Value { get; }
            public int NumberOfValues { get; }

            // Value can be stored inline if it doesn't exceed the available space in the IFD offset field
            public bool StoreInline => Field.Size * NumberOfValues <= OffsetFieldSize;

            // IFD offset is a 32-bit value (4 bytes)
            internal const int OffsetFieldSize = 4;
        }
        private sealed class TiffField
        {
            public short Tag { get; }
            public short Type { get; }
            public int Size => DataType.Sizes[Type];

            private TiffField(short tag, short type)
            {
                Tag = tag;
                Type = type;
            }

            public static readonly TiffField NewSubfileType = new TiffField(254, DataType.Short);
            public static readonly TiffField ImageWidth = new TiffField(256, DataType.Short);
            public static readonly TiffField ImageLength = new TiffField(257, DataType.Short);
            public static readonly TiffField BitsPerSample = new TiffField(258, DataType.Short);
            public static readonly TiffField Compression = new TiffField(259, DataType.Short);
            public static readonly TiffField PhotometricInterpretation = new TiffField(262, DataType.Short);
            public static readonly TiffField StripOffsets = new TiffField(273, DataType.Long);
            public static readonly TiffField SamplesPerPixel = new TiffField(277, DataType.Short);
            public static readonly TiffField RowsPerStrip = new TiffField(278, DataType.Short);
            public static readonly TiffField StripByteCounts = new TiffField(279, DataType.Short);
            public static readonly TiffField XResolution = new TiffField(282, DataType.Rational);
            public static readonly TiffField YResolution = new TiffField(283, DataType.Rational);
            public static readonly TiffField ResolutionUnit = new TiffField(296, DataType.Short);

            internal static class DataType
            {
                internal static readonly IReadOnlyDictionary<short, int> Sizes = new ReadOnlyDictionary<short, int>(new Dictionary<short, int>
                {
                    { Short, 2 },
                    { Long, 4 },
                    { Rational, 8 },
                });
                internal const short Short = 3; // 16-bit unsigned integer
                internal const short Long = 4; // 32-bit unsigned integer
                internal const short Rational = 5; // 2x 32-bit unsigned integer (numerator and denominator)
            }
        }
    }
    internal static class BinaryWriterExtensions
    {
        public static void WriteChars(this BinaryWriter writer, string s) => writer.Write(s.ToCharArray());
        public static void WriteByte(this BinaryWriter writer, byte b) => writer.Write(b);
        public static void WriteShort(this BinaryWriter writer, short s) => writer.Write(s);
        public static void WriteInteger(this BinaryWriter writer, int i) => writer.Write(i);
    }
    internal static class DictionaryExtensions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> source, out TKey key, out TValue value)
        {
            key = source.Key;
            value = source.Value;
        }
    }
}
