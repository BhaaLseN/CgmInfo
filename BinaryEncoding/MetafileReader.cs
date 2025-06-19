using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Utilities;
using BaseMetafileReader = CgmInfo.MetafileReader;

namespace CgmInfo.BinaryEncoding
{
    public class MetafileReader : BaseMetafileReader
    {
        // ISO/IEC 8632-3 8.1, Table 2
        private readonly Dictionary<(int ElementClass, int ElementId), Func<MetafileReader, CommandHeader, Command>> _commandTable = new Dictionary<(int, int), Func<MetafileReader, CommandHeader, Command>>
        {
            // delimiter elements [ISO/IEC 8632-3 8.2, Table 3]
            // { (0, 0), ReadNoop }, // no-op; these are skipped already while reading the command header
            { (0, 1), ReadBeginMetafile },
            { (0, 2), ReadEndMetafile },
            { (0, 3), DelimiterElementReader.BeginPicture },
            { (0, 4), DelimiterElementReader.BeginPictureBody },
            { (0, 5), DelimiterElementReader.EndPicture },
            { (0, 6), DelimiterElementReader.BeginSegment },
            { (0, 7), DelimiterElementReader.EndSegment },
            { (0, 8), DelimiterElementReader.BeginFigure },
            { (0, 9), DelimiterElementReader.EndFigure },
            // entries 10, 11 and 12 do not exist in ISO/IEC 8632-3
            { (0, 13), DelimiterElementReader.BeginProtectionRegion },
            { (0, 14), DelimiterElementReader.EndProtectionRegion },
            { (0, 15), DelimiterElementReader.BeginCompoundLine },
            { (0, 16), DelimiterElementReader.EndCompoundLine },
            { (0, 17), DelimiterElementReader.BeginCompoundTextPath },
            { (0, 18), DelimiterElementReader.EndCompoundTextPath },
            { (0, 19), DelimiterElementReader.BeginTileArray },
            { (0, 20), DelimiterElementReader.EndTileArray },
            { (0, 21), DelimiterElementReader.BeginApplicationStructure },
            { (0, 22), DelimiterElementReader.BeginApplicationStructureBody },
            { (0, 23), DelimiterElementReader.EndApplicationStructure },

            // metafile descriptor elements [ISO/IEC 8632-3 8.3, Table 4]
            { (1, 1), MetafileDescriptorReader.MetafileVersion },
            { (1, 2), MetafileDescriptorReader.MetafileDescription },
            { (1, 3), ReadVdcType },
            { (1, 4), ReadIntegerPrecision },
            { (1, 5), ReadRealPrecision },
            { (1, 6), ReadIndexPrecision },
            { (1, 7), ReadColorPrecision },
            { (1, 8), ReadColorIndexPrecision },
            { (1, 9), MetafileDescriptorReader.MaximumColorIndex },
            { (1, 10), MetafileDescriptorReader.ColorValueExtent },
            { (1, 11), MetafileDescriptorReader.MetafileElementsList },
            { (1, 12), ReadMetafileDefaultsReplacement },
            { (1, 13), MetafileDescriptorReader.FontList },
            { (1, 14), MetafileDescriptorReader.CharacterSetList },
            { (1, 15), MetafileDescriptorReader.CharacterCodingAnnouncer },
            { (1, 16), ReadNamePrecision },
            { (1, 17), MetafileDescriptorReader.MaximumVdcExtent },
            { (1, 18), MetafileDescriptorReader.SegmentPriorityExtent },
            { (1, 19), ReadColorModel },
            //{ (1, 20), MetafileDescriptorReader.ColorCalibration },
            { (1, 21), MetafileDescriptorReader.FontProperties },
            //{ (1, 22), MetafileDescriptorReader.GlyphMapping },
            //{ (1, 23), MetafileDescriptorReader.SymbolLibraryList },
            //{ (1, 24), MetafileDescriptorReader.PictureDirectory },

            // picture descriptor elements [ISO/IEC 8632-3 8.4, Table 5]
            { (2, 1), PictureDescriptorReader.ScalingMode },
            { (2, 2), ReadColorSelectionMode },
            { (2, 3), ReadLineWidthSpecificationMode },
            { (2, 4), ReadMarkerSizeSpecificationMode },
            { (2, 5), ReadEdgeWidthSpecificationMode },
            { (2, 6), PictureDescriptorReader.VdcExtent },
            { (2, 7), PictureDescriptorReader.BackgroundColor },
            { (2, 8), PictureDescriptorReader.DeviceViewport },
            { (2, 9), ReadDeviceViewportSpecificationMode },
            //{ (2, 10), PictureDescriptorReader.DeviceViewportMapping },
            //{ (2, 11), PictureDescriptorReader.LineRepresentation },
            //{ (2, 12), PictureDescriptorReader.MarkerRepresentation },
            //{ (2, 13), PictureDescriptorReader.TextRepresentation },
            //{ (2, 14), PictureDescriptorReader.FillRepresentation },
            //{ (2, 15), PictureDescriptorReader.EdgeRepresentation },
            { (2, 16), ReadInteriorStyleSpecificationMode },
            { (2, 17), PictureDescriptorReader.LineAndEdgeTypeDefinition },
            { (2, 18), PictureDescriptorReader.HatchStyleDefinition },
            { (2, 19), PictureDescriptorReader.GeometricPatternDefinition },
            //{ (2, 20), PictureDescriptorReader.ApplicationStructureDirectory },

            // control elements [ISO/IEC 8632-3 8.5, Table 6]
            { (3, 1), ReadVdcIntegerPrecision },
            { (3, 2), ReadVdcRealPrecision },
            { (3, 3), ControlElementReader.AuxiliaryColor },
            { (3, 4), ControlElementReader.Transparency },
            { (3, 5), ControlElementReader.ClipRectangle },
            { (3, 6), ControlElementReader.ClipIndicator },
            { (3, 7), ControlElementReader.LineClippingMode },
            { (3, 8), ControlElementReader.MarkerClippingMode },
            { (3, 9), ControlElementReader.EdgeClippingMode },
            { (3, 10), ControlElementReader.NewRegion },
            { (3, 11), ControlElementReader.SavePrimitiveContext },
            { (3, 12), ControlElementReader.RestorePrimitiveContext },
            // entries 13 until 16 do not exist in ISO/IEC 8632-3
            { (3, 17), ControlElementReader.ProtectionRegionIndicator },
            { (3, 18), ControlElementReader.GeneralizedTextPathMode },
            { (3, 19), ControlElementReader.MiterLimit },
            //{ (3, 20), ControlElementReader.TransparentCellColor },

            // graphical primitive elements [ISO/IEC 8632-3 8.6, Table 7]
            { (4, 1), GraphicalPrimitiveReader.Polyline },
            { (4, 2), GraphicalPrimitiveReader.DisjointPolyline },
            { (4, 3), GraphicalPrimitiveReader.Polymarker },
            { (4, 4), GraphicalPrimitiveReader.Text },
            { (4, 5), GraphicalPrimitiveReader.RestrictedText },
            { (4, 6), GraphicalPrimitiveReader.AppendText },
            { (4, 7), GraphicalPrimitiveReader.Polygon },
            { (4, 8), GraphicalPrimitiveReader.PolygonSet },
            { (4, 9), GraphicalPrimitiveReader.CellArray },
            //{ (4, 10), GraphicalPrimitiveReader.GeneralizedDrawingPrimitive },
            { (4, 11), GraphicalPrimitiveReader.Rectangle },
            { (4, 12), GraphicalPrimitiveReader.Circle },
            { (4, 13), GraphicalPrimitiveReader.CircularArc3Point },
            { (4, 14), GraphicalPrimitiveReader.CircularArc3PointClose },
            { (4, 15), GraphicalPrimitiveReader.CircularArcCenter },
            { (4, 16), GraphicalPrimitiveReader.CircularArcCenterClose },
            { (4, 17), GraphicalPrimitiveReader.Ellipse },
            { (4, 18), GraphicalPrimitiveReader.EllipticalArc },
            { (4, 19), GraphicalPrimitiveReader.EllipticalArcClose },
            { (4, 20), GraphicalPrimitiveReader.CircularArcCenterReversed },
            { (4, 21), GraphicalPrimitiveReader.ConnectingEdge },
            { (4, 22), GraphicalPrimitiveReader.HyperbolicArc },
            { (4, 23), GraphicalPrimitiveReader.ParabolicArc },
            { (4, 24), GraphicalPrimitiveReader.NonUniformBSpline },
            { (4, 25), GraphicalPrimitiveReader.NonUniformRationalBSpline },
            { (4, 26), GraphicalPrimitiveReader.Polybezier },
            //{ (4, 27), GraphicalPrimitiveReader.Polysymbol },
            { (4, 28), GraphicalPrimitiveReader.BitonalTile },
            { (4, 29), GraphicalPrimitiveReader.Tile },

            // attribute elements [ISO/IEC 8632-3 8.7, Table 8]
            { (5, 1), AttributeReader.LineBundleIndex },
            { (5, 2), AttributeReader.LineType },
            { (5, 3), AttributeReader.LineWidth },
            { (5, 4), AttributeReader.LineColor },
            { (5, 5), AttributeReader.MarkerBundleIndex },
            { (5, 6), AttributeReader.MarkerType },
            { (5, 7), AttributeReader.MarkerSize },
            { (5, 8), AttributeReader.MarkerColor },
            { (5, 9), AttributeReader.TextBundleIndex },
            { (5, 10), AttributeReader.TextFontIndex },
            { (5, 11), AttributeReader.TextPrecision },
            { (5, 12), AttributeReader.CharacterExpansionFactor },
            { (5, 13), AttributeReader.CharacterSpacing },
            { (5, 14), AttributeReader.TextColor },
            { (5, 15), AttributeReader.CharacterHeight },
            { (5, 16), AttributeReader.CharacterOrientation },
            { (5, 17), AttributeReader.TextPath },
            { (5, 18), AttributeReader.TextAlignment },
            { (5, 19), AttributeReader.CharacterSetIndex },
            { (5, 20), AttributeReader.AlternateCharacterSetIndex },
            { (5, 21), AttributeReader.FillBundleIndex },
            { (5, 22), AttributeReader.InteriorStyle },
            { (5, 23), AttributeReader.FillColor },
            { (5, 24), AttributeReader.HatchIndex },
            { (5, 25), AttributeReader.PatternIndex },
            { (5, 26), AttributeReader.EdgeBundleIndex },
            { (5, 27), AttributeReader.EdgeType },
            { (5, 28), AttributeReader.EdgeWidth },
            { (5, 29), AttributeReader.EdgeColor },
            { (5, 30), AttributeReader.EdgeVisibility },
            { (5, 31), AttributeReader.FillReferencePoint },
            { (5, 32), AttributeReader.PatternTable },
            { (5, 33), AttributeReader.PatternSize },
            { (5, 34), ReadColorTable },
            { (5, 35), AttributeReader.AspectSourceFlags },
            { (5, 36), AttributeReader.PickIdentifier },
            { (5, 37), AttributeReader.LineCap },
            { (5, 38), AttributeReader.LineJoin },
            { (5, 39), AttributeReader.LineTypeContinuation },
            { (5, 40), AttributeReader.LineTypeInitialOffset },
            //{ (5, 41), ControlElementReader.TextScoreType },
            { (5, 42), AttributeReader.RestrictedTextType },
            { (5, 43), AttributeReader.InterpolatedInterior },
            { (5, 44), AttributeReader.EdgeCap },
            { (5, 45), AttributeReader.EdgeJoin },
            { (5, 46), AttributeReader.EdgeTypeContinuation },
            { (5, 47), AttributeReader.EdgeTypeInitialOffset },
            //{ (5, 48), ControlElementReader.SymbolLibraryIndex },
            //{ (5, 49), ControlElementReader.SymbolColor },
            //{ (5, 50), ControlElementReader.SymbolSize },
            //{ (5, 51), ControlElementReader.SymbolOrientation },

            // escape elements [ISO/IEC 8632-3 8.8, Table 9]
            { (6, 1), EscapeReader.Escape },

            // external elements [ISO/IEC 8632-3 8.9, Table 10]
            { (7, 1), ExternalReader.Message },
            { (7, 2), ExternalReader.ApplicationData },

            // segment control/segment attribute elements [ISO/IEC 8632-3 8.10, Table 11]
            { (8, 1), SegmentReader.CopySegment },
            { (8, 2), SegmentReader.InheritanceFilter },
            { (8, 3), SegmentReader.ClipInheritance },
            { (8, 4), SegmentReader.SegmentTransformation },
            { (8, 5), SegmentReader.SegmentHighlighting },
            { (8, 6), SegmentReader.SegmentDisplayPriority },
            { (8, 7), SegmentReader.SegmentPickPriority },

            // application structure descriptor elements [ISO/IEC 8632-3 8.11, Table 12]
            { (9, 1), ApplicationStructureDescriptorReader.ApplicationStructureAttribute },
        };

        private BinaryReader _reader;
        private bool _insideMetafile;
        // assume a default ISO 8859-1 [ISO/IEC 8632-1 6.3.4.5]
        private Encoding _currentEncoding = GetDefaultEncoding();

        public MetafileReader(string fileName)
            : base(fileName, isBinaryEncoding: true)
        {
            _reader = null!;
        }
        private MetafileReader(MetafileReader parent, byte[] subBuffer)
            : base(parent)
        {
            _reader = new BinaryReader(new MemoryStream(subBuffer));
            _currentEncoding = parent._currentEncoding;
        }

        public static bool IsBinaryMetafile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!stream.CanSeek)
                throw new InvalidOperationException("Cannot seek the stream.");

            stream.Seek(0, SeekOrigin.Begin);
            ushort commandHeader = stream.ReadWord();
            int elementClass = (commandHeader >> 12) & 0xF;
            int elementId = (commandHeader >> 5) & 0x7F;

            // check whether the first two bytes are 0/1 (BEGIN METAFILE)
            return elementClass == 0 && elementId == 1;
        }
        protected override Command? ReadCommand(Stream stream)
        {
            // stop at EOF; or when we cannot at least read another command header
            if (stream.Position + 2 > stream.Length)
                return null;

            Command result;
            var commandHeader = ReadCommandHeader(stream);
            // special case: we might encounter a no-op after END METAFILE, which leads into EOF.
            // ReadCommandHeader will return null in that case, and we should simply pass this on here.
            if (commandHeader == null)
                return null;

            if (!_commandTable.TryGetValue((commandHeader.ElementClass, commandHeader.ElementId), out var commandHandler) || commandHandler == null)
                commandHandler = ReadUnsupportedElement;

            try
            {
                result = commandHandler(this, commandHeader);
                result.Buffer = commandHeader.Buffer;
            }
            catch (Exception ex)
            {
                return new InvalidCommand(commandHeader.ElementClass, commandHeader.ElementId, ex)
                {
                    Buffer = commandHeader.Buffer,
                };
            }
            // the only case where _insideMetafile is allowed to be false is at the end of the file (0/2 END METAFILE)
            if (result != null && !_insideMetafile)
            {
                if (result.ElementClass != 0)
                    throw new FormatException("Expected Element Class 0 (Delimiter) at the beginning of a Metafile");

                if (result.ElementId == 2)
                {
                    // the Metafile should end at END METAFILE and EOF; +/- a padding byte
                    if (stream.Position < stream.Length - 2)
                    {
                        throw new FormatException(string.Format(
                            "Found Element Id 2 (END METAFILE), but got {0} bytes left to read. Multiple Metafiles within a single file are not supported.",
                            stream.Length - stream.Position - 1));
                    }
                }
                else if (result.ElementId != 1)
                {
                    throw new FormatException("Expected Element Id 1 (BEGIN METAFILE) at the beginning of a Metafile");
                }
            }

            return result;
        }

        private CommandHeader? ReadCommandHeader(Stream stream)
        {
            var trackingBuffer = TrackInternalBuffer ? new TrackingBuffer(stream.Position) : null;

            // commands are always word aligned [ISO/IEC 8632-3 5.4]
            if (stream.Position % 2 == 1)
            {
                byte align = (byte)stream.ReadByte();
                trackingBuffer?.SetAlign(align);
            }

            ushort commandHeader = stream.ReadWord();
            int elementClass = (commandHeader >> 12) & 0xF;
            int elementId = (commandHeader >> 5) & 0x7F;
            int parameterListLength = commandHeader & 0x1F;
            trackingBuffer?.SetHeader(commandHeader);

            // store the whole element in a buffer; resolving long commands ahead of time
            var readBuffer = new MemoryStream(parameterListLength);
            // long format commands have a parameterListLength of all-ones (31, 0x1F, 11111b) and use word-size prefixed partitions.
            // each partition indicates whether it is the last by having its top-most bit set to 0 [ISO/IEC 8632-3 5.4]
            bool isLongFormat = parameterListLength == 0x1F;
            if (isLongFormat)
            {
                bool isLastPartition;
                parameterListLength = 0;
                do
                {
                    // first comes the length; 2 octets
                    ushort longFormCommandHeader = stream.ReadWord();
                    trackingBuffer?.AddLongCommand(longFormCommandHeader);
                    // top-most bit indicates whether more partitions follow or not
                    int partitionFlag = (longFormCommandHeader >> 15) & 0x1;
                    isLastPartition = partitionFlag == 0;
                    // the remaining 15 bits are the actual length of this partition
                    int partitionLength = longFormCommandHeader & 0x7FFF;
                    parameterListLength += partitionLength;

                    // directly after the length, data follows
                    byte[] buffer = new byte[partitionLength];
                    stream.Read(buffer, 0, buffer.Length);
                    readBuffer.Write(buffer, 0, buffer.Length);
                    trackingBuffer?.AddBuffer(buffer);

                } while (!isLastPartition);
            }
            else
            {
                // short command form; buffer the contents directly
                byte[] buffer = new byte[parameterListLength];
                stream.Read(buffer, 0, buffer.Length);
                readBuffer.Write(buffer, 0, buffer.Length);
                trackingBuffer?.AddBuffer(buffer);
            }

            readBuffer.Position = 0;

            _reader?.Dispose();
            _reader = new BinaryReader(readBuffer);

            bool isNoop = elementClass == 0 && elementId == 0;
            if (isNoop)
            {
                // no need to seek here anymore; the whole no-op has been read into the temporary buffer already anyways
                // however, if we reached EOF, we simply caught a padding no-op...lets say we reached EOF right away, and be done.
                if (stream.Position >= stream.Length)
                    return null;
                return ReadCommandHeader(stream);
            }

            return new CommandHeader(elementClass, elementId, parameterListLength)
            {
                Buffer = trackingBuffer,
            };
        }

        private static Command ReadUnsupportedElement(MetafileReader reader, CommandHeader commandHeader)
        {
            // no need to seek here anymore; the whole unsupported element has been read into the temporary buffer already anyways
            return new UnsupportedCommand(commandHeader.ElementClass, commandHeader.ElementId, reader.GetInternalBuffer());
        }

        private static Command ReadBeginMetafile(MetafileReader reader, CommandHeader commandHeader)
        {
            var result = DelimiterElementReader.BeginMetafile(reader, commandHeader);
            reader._insideMetafile = true;
            return result;
        }
        private static Command ReadEndMetafile(MetafileReader reader, CommandHeader commandHeader)
        {
            var result = DelimiterElementReader.EndMetafile(reader, commandHeader);
            reader._insideMetafile = false;
            return result;
        }

        private static Command ReadVdcType(MetafileReader reader, CommandHeader commandHeader)
        {
            var vdcType = MetafileDescriptorReader.VdcType(reader, commandHeader);
            reader.Descriptor.VdcType = vdcType.Specification;
            return vdcType;
        }
        private static Command ReadIntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var integerPrecision = MetafileDescriptorReader.IntegerPrecision(reader, commandHeader);
            reader.Descriptor.IntegerPrecision = integerPrecision.Precision;
            return integerPrecision;
        }
        private static Command ReadRealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var realPrecision = MetafileDescriptorReader.RealPrecision(reader, commandHeader);
            reader.Descriptor.RealPrecision = realPrecision.Specification;
            return realPrecision;
        }
        private static Command ReadIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var indexPrecision = MetafileDescriptorReader.IndexPrecision(reader, commandHeader);
            reader.Descriptor.IndexPrecision = indexPrecision.Precision;
            return indexPrecision;
        }
        private static Command ReadColorPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var colorPrecision = MetafileDescriptorReader.ColorPrecision(reader, commandHeader);
            reader.Descriptor.ColorPrecision = colorPrecision.Precision;
            return colorPrecision;
        }
        private static Command ReadColorIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var colorIndexPrecision = MetafileDescriptorReader.ColorIndexPrecision(reader, commandHeader);
            reader.Descriptor.ColorIndexPrecision = colorIndexPrecision.Precision;
            return colorIndexPrecision;
        }
        private static Command ReadNamePrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var namePrecision = MetafileDescriptorReader.NamePrecision(reader, commandHeader);
            reader.Descriptor.NamePrecision = namePrecision.Precision;
            return namePrecision;
        }
        private static Command ReadColorModel(MetafileReader reader, CommandHeader commandHeader)
        {
            var colorModel = MetafileDescriptorReader.ColorModelCommand(reader, commandHeader);
            reader.Descriptor.ColorModel = colorModel.ColorModel;
            return colorModel;
        }

        private static Command ReadColorSelectionMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var colorSelectionMode = PictureDescriptorReader.ColorSelectionMode(reader, commandHeader);
            reader.Descriptor.ColorSelectionMode = colorSelectionMode.ColorMode;
            return colorSelectionMode;
        }
        private static Command ReadLineWidthSpecificationMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var lineWidthSpecificationMode = PictureDescriptorReader.LineWidthSpecificationMode(reader, commandHeader);
            reader.Descriptor.LineWidthSpecificationMode = lineWidthSpecificationMode.WidthSpecificationMode;
            return lineWidthSpecificationMode;
        }
        private static Command ReadMarkerSizeSpecificationMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var markerSizeSpecificationMode = PictureDescriptorReader.MarkerSizeSpecificationMode(reader, commandHeader);
            reader.Descriptor.MarkerSizeSpecificationMode = markerSizeSpecificationMode.WidthSpecificationMode;
            return markerSizeSpecificationMode;
        }
        private static Command ReadEdgeWidthSpecificationMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var edgeWidthSpecificationMode = PictureDescriptorReader.EdgeWidthSpecificationMode(reader, commandHeader);
            reader.Descriptor.EdgeWidthSpecificationMode = edgeWidthSpecificationMode.WidthSpecificationMode;
            return edgeWidthSpecificationMode;
        }
        private static Command ReadDeviceViewportSpecificationMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var deviceViewportSpecificationMode = PictureDescriptorReader.DeviceViewportSpecificationMode(reader, commandHeader);
            reader.Descriptor.DeviceViewportSpecificationMode = deviceViewportSpecificationMode.SpecificationMode;
            return deviceViewportSpecificationMode;
        }
        private static Command ReadInteriorStyleSpecificationMode(MetafileReader reader, CommandHeader commandHeader)
        {
            var interiorStyleSpecificationMode = PictureDescriptorReader.InteriorStyleSpecificationMode(reader, commandHeader);
            reader.Descriptor.InteriorStyleSpecificationMode = interiorStyleSpecificationMode.WidthSpecificationMode;
            return interiorStyleSpecificationMode;
        }

        private static Command ReadVdcIntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var vdcIntegerPrecision = ControlElementReader.VdcIntegerPrecision(reader, commandHeader);
            reader.Descriptor.VdcIntegerPrecision = vdcIntegerPrecision.Precision;
            return vdcIntegerPrecision;
        }
        private static Command ReadVdcRealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            var vdcRealPrecision = ControlElementReader.VdcRealPrecision(reader, commandHeader);
            reader.Descriptor.VdcRealPrecision = vdcRealPrecision.Specification;
            return vdcRealPrecision;
        }

        private static Command ReadColorTable(MetafileReader reader, CommandHeader commandHeader)
        {
            var colorTable = AttributeReader.ColorTable(reader, commandHeader);
            reader.Descriptor.UpdateColorTable(colorTable);
            return colorTable;
        }

        private static MetafileDefaultsReplacement ReadMetafileDefaultsReplacement(MetafileReader reader, CommandHeader commandHeader)
        {
            // this is a memory stream set by ReadCommandHeader, which contains 1..n commands itself.
            // however, _reader is disposed after every run, so we need to keep this buffer around another way.
            using var replacementsStream = new MemoryStream();
            reader._reader.BaseStream.CopyTo(replacementsStream);
            replacementsStream.Position = 0;

            var commands = new List<Command>();
            while (true)
            {
                var command = reader.ReadCommand(replacementsStream);
                if (command == null)
                    break;
                commands.Add(command);
            }
            return new MetafileDefaultsReplacement(commands.ToArray());
        }

        public long Position
        {
            get { return _reader != null ? _reader.BaseStream.Position : -1; }
        }

        internal bool HasMoreData()
        {
            return HasMoreData(1);
        }

        internal bool HasMoreData(int minimumLeft)
        {
            return _reader != null && _reader.BaseStream.Position + minimumLeft <= _reader.BaseStream.Length;
        }

        private byte[] GetInternalBuffer()
        {
            return ((MemoryStream)_reader.BaseStream).ToArray();
        }

        internal StructuredDataRecord ReadStructuredDataRecord()
        {
            return ReadStructuredDataRecord(new StructuredDataRecordReader());
        }

        internal StructuredDataRecord ReadStructuredDataRecord(StructuredDataRecordReader sdrReader)
        {
            // overall length is encoded similar to the string length [ISO/IEC 8632-3 7, Table 1, Note 12/Note 17]
            // (ie. one byte, followed by one word if its 255).
            int length = ReadByte();
            if (length == 255)
                length = ReadWord();

            byte[] sdrBuffer = ReadBytes(length);
            return sdrReader.Read(new MetafileReader(this, sdrBuffer));
        }

        internal int ReadInteger()
        {
            // integer is a signed integer at integer precision [ISO/IEC 8632-3 7, Table 1, I]
            return ReadInteger(Descriptor.IntegerPrecision / 8, unsigned: false);
        }
        internal int ReadInteger(int numBytes, bool unsigned)
        {
            if (numBytes < 1 || numBytes > 4)
                throw new ArgumentOutOfRangeException(nameof(numBytes), numBytes, "Number of bytes must be between 1 and 4");
            uint ret = 0;
            int signBit = 1 << ((numBytes * 8) - 1);
            int maxUnsignedValue = 1 << (numBytes * 8);
            while (numBytes-- > 0)
                ret = (ret << 8) | ReadByte();
            int signedRet = (int)ret;
            if (!unsigned && (ret & signBit) > 0)
                signedRet = signedRet - maxUnsignedValue;
            return signedRet;
        }

        internal byte[] ReadBytes(int length) => _reader.ReadBytes(length);
        internal byte[] ReadBitstream()
        {
            // bitstream is a series of unsigned integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, BS / Note 15]
            // 16 bits per entry is chosen for portability reasons and need not be filled completely; the remainder is set to 0.
            byte[] data = ReadBytes((int)(_reader.BaseStream.Length - _reader.BaseStream.Position));
            // the data is little endian; swap if necessary.
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    (data[i + 1], data[i]) = (data[i], data[i + 1]);
                }
            }
            return data;
        }

        internal int ReadEnum()
        {
            // enum is a signed integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, E / Note 3]
            return ReadInteger(2, unsigned: false);
        }
        internal TEnum ReadEnum<TEnum>() where TEnum : Enum
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), ReadEnum());
        }
        internal MetafilePoint ReadPoint()
        {
            return new MetafilePoint(ReadVdc(), ReadVdc());
        }
        internal double ReadSizeSpecification(WidthSpecificationModeType widthSpecificationMode)
        {
            // When the value is 'absolute', then an associated parameter of type SS
            // resolves to the basic data type VDC. Otherwise, associated
            // SS parameters resolve to the basic data type R. [ISO/IEC 8632-1 7.1, Table 11]
            if (widthSpecificationMode == WidthSpecificationModeType.Absolute)
                return ReadVdc();
            else
                return ReadReal();
        }
        internal double ReadVdc()
        {
            // a VDC is either an int or a double; depending on what VDC TYPE said [ISO/IEC 8632-3 7, Table 1, Note 7]
            if (Descriptor.VdcType == VdcTypeSpecification.Integer)
            {
                // value is a signed integer at VDC Integer Precision
                return ReadInteger(Descriptor.VdcIntegerPrecision / 8, unsigned: false);
            }
            else if (Descriptor.VdcType == VdcTypeSpecification.Real)
            {
                return ReadReal(Descriptor.VdcRealPrecision);
            }

            throw new NotSupportedException("The current VDC TYPE is not supported");
        }

        internal MetafileMatrix ReadMatrix()
        {
            return new MetafileMatrix(
                // a11 and a12
                ReadReal(), ReadReal(),
                // a21 and a22
                ReadReal(), ReadReal(),
                // a13 and a23
                ReadVdc(), ReadVdc());
        }
        internal double ReadViewportCoordinate()
        {
            // a Viewport Coordinate (VC) is either an int or a double; depending on what DEVICE VIEWPORT SPECIFICATION MODE said [ISO/IEC 8632-3 7, Table 1, Note 13/14]
            if (Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.MillimetersWithScaleFactor ||
                Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.PhysicalDeviceCoordinates)
            {
                return ReadInteger();
            }
            else if (Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.FractionOfDrawingSurface)
            {
                return ReadReal();
            }

            throw new NotSupportedException("The current DEVICE VIEWPORT SPECIFICATION MODE is not supported");
        }
        internal MetafilePoint ReadViewportPoint()
        {
            double x = ReadViewportCoordinate();
            double y = ReadViewportCoordinate();
            return new MetafilePoint(x, y);
        }
        internal MetafileColor ReadColor(int colorPrecision)
        {
            if (Descriptor.ColorSelectionMode == ColorModeType.Direct)
                return ReadDirectColor(colorPrecision);
            else
                return ReadIndexedColor(colorPrecision);
        }
        internal MetafileColor ReadColor()
        {
            if (Descriptor.ColorSelectionMode == ColorModeType.Direct)
                return ReadDirectColor();
            else
                return ReadIndexedColor();
        }
        internal MetafileColor ReadIndexedColor()
        {
            return ReadIndexedColor(Descriptor.ColorIndexPrecision / 8);
        }
        internal MetafileColor ReadIndexedColor(int colorIndexPrecision)
        {
            int colorIndex = ReadColorIndex(colorIndexPrecision);
            return new MetafileColorIndexed(colorIndex, Descriptor.GetIndexedColor(colorIndex));
        }

        internal int ReadColorIndex()
        {
            return ReadColorIndex(Descriptor.ColorIndexPrecision / 8);
        }
        internal int ReadColorIndex(int colorIndexPrecision)
        {
            return ReadInteger(colorIndexPrecision, true);
        }

        internal MetafileColor ReadDirectColor()
        {
            return ReadDirectColor(Descriptor.ColorPrecision / 8);
        }
        internal MetafileColor ReadDirectColor(int colorDirectPrecision)
        {
            if (Descriptor.ColorModel == ColorModel.RGB)
            {
                int r = ReadColorValue(colorDirectPrecision);
                int g = ReadColorValue(colorDirectPrecision);
                int b = ReadColorValue(colorDirectPrecision);
                return new MetafileColorRGB(r, g, b);
            }
            else if (Descriptor.ColorModel == ColorModel.CMYK)
            {
                int c = ReadColorValue(colorDirectPrecision);
                int m = ReadColorValue(colorDirectPrecision);
                int y = ReadColorValue(colorDirectPrecision);
                int k = ReadColorValue(colorDirectPrecision);
                return new MetafileColorCMYK(c, m, y, k);
            }
            else
            {
                double first = ReadReal();
                double second = ReadReal();
                double third = ReadReal();
                return new MetafileColorCIE(Descriptor.ColorModel, first, second, third);
            }
        }

        internal int ReadColorValue()
        {
            return ReadColorValue(Descriptor.ColorPrecision / 8);
        }
        internal int ReadColorValue(int colorPrecision)
        {
            // FIXME: color component in CIELAB/CIELUV/RGB-related is reals, not ints
            // color components are unsigned integers at direct color precision
            return ReadInteger(colorPrecision, unsigned: true);
        }

        internal double ReadFixedPoint(int numBytes)
        {
            // ISO/IEC 8632-3 6.4
            // real value is computed as "whole + (fraction / 2**exp)"
            // exp is the width of the fraction value
            // the "whole part" has the same form as a Signed Integer
            int whole = ReadInteger(numBytes / 2, unsigned: false);
            // the "fractional part" has the same form as an Unsigned Integer
            int fraction = ReadInteger(numBytes / 2, unsigned: true);
            // if someone wanted a 4 byte fixed point real, they get 32 bits (16 bits whole, 16 bits fraction)
            // therefore exp would be 16 here (same for 8 byte with 64 bits and 32/32 -> 32 exp)
            int exp = numBytes / 2 * 8;
            return whole + fraction / Math.Pow(2, exp);
        }
        internal double ReadFloatingPoint(int numBytes)
        {
            // ISO/IEC 8632-3 6.5
            // C# float/double conform to ANSI/IEEE 754 and have the same format as the specification wants;
            // but the endianness might not work out. swap if necessary
            if (numBytes == 4)
            {
                byte[] floatBytes = ReadBytes(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(floatBytes);
                return BitConverter.ToSingle(floatBytes, 0);
            }
            if (numBytes == 8)
            {
                byte[] doubleBytes = ReadBytes(8);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(doubleBytes);
                return BitConverter.ToDouble(doubleBytes, 0);
            }

            throw new InvalidOperationException(string.Format("Sorry, cannot read a floating point value with {0} bytes", numBytes));
        }
        internal double ReadReal()
        {
            return ReadReal(Descriptor.RealPrecision);
        }

        private double ReadReal(RealPrecisionSpecification precision) => precision switch
        {
            RealPrecisionSpecification.FixedPoint32Bit => ReadFixedPoint(4),
            RealPrecisionSpecification.FixedPoint64Bit => ReadFixedPoint(8),
            RealPrecisionSpecification.FloatingPoint32Bit => ReadFloatingPoint(4),
            RealPrecisionSpecification.FloatingPoint64Bit => ReadFloatingPoint(8),
            _ => throw new NotSupportedException("The current Real Precision is not supported"),
        };

        internal int ReadIndex()
        {
            // index is a signed integer at index precision [ISO/IEC 8632-3 7, Table 1, IX]
            return ReadInteger(Descriptor.IndexPrecision / 8, unsigned: false);
        }
        internal int ReadName()
        {
            // name is a signed integer at name precision [ISO/IEC 8632-3 7, Table 1, N]
            return ReadInteger(Descriptor.NamePrecision / 8, unsigned: false);
        }
        internal ushort ReadWord()
        {
            return (ushort)((ReadByte() << 8) | ReadByte());
        }

        internal byte ReadByte()
        {
            return _reader.ReadByte();
        }

        internal string ReadString()
        {
            // string starts with a length byte [ISO/IEC 8632-3 7, Table 1, Note 6]
            int length = ReadByte();
            byte[] characters;

            // long string: length of 255 indicates that blocks similar to long-form commands follow
            if (length == 255)
            {
                bool isPartialString;
                using var tempString = new MemoryStream();
                do
                {
                    int chunkLength = ReadWord();
                    // first bit indicates whether this is just a partial string and another one follows
                    isPartialString = (chunkLength >> 16) == 1;
                    chunkLength &= 0x7FFF;

                    byte[] chunk = ReadBytes(chunkLength);
                    tempString.Write(chunk, 0, chunk.Length);

                    length += chunkLength;
                } while (isPartialString);

                characters = tempString.ToArray();
            }
            else
            {
                characters = ReadBytes(length);
            }

            string result;
            // try to detect certain common encodings (based on ISO/IEC 2022 / ECMA-35)
            // this only checks for DOCS (DECIDE OTHER CODING SYSTEM), identified by "ESC % / F" (0x1B 0x25 0x2F F)
            // and limited to F = "G" (0x47), "H" (0x48), "I" (0x49), "J" (0x4A), "K" (0x4B) and "L" (0x4C) at this point.
            // 0x47 until 0x49 are various levels of UTF-8, while 0x4A until 0x4C are various levels of UTF-16.
            // this also specifically checks for the ASCII encoding identified by "ESC ( B" (0x1B 0x28 0x42).
            // [ISO-IR International Register of Coded Character Sets to be used with Escape Sequences, 2.8.2]
            // [ISO/IEC 8632-1 6.3.4.5, Example 1]
            if (length >= 4 && characters[0] == 0x1B && characters[1] == 0x25 && characters[2] == 0x2F &&
                characters[3] >= 0x47 && characters[3] <= 0x4C)
            {
                if (characters[3] >= 0x47 && characters[3] <= 0x49)
                {
                    // ESC 2/5 2/15 4/7: UTF-8 Level 1
                    // ESC 2/5 2/15 4/8: UTF-8 Level 2
                    // ESC 2/5 2/15 4/9: UTF-8 Level 3
                    _currentEncoding = Encoding.UTF8;
                }
                else
                {
                    // ESC 2/5 2/15 4/10: UTF-16 Level 1
                    // ESC 2/5 2/15 4/11: UTF-16 Level 2
                    // ESC 2/5 2/15 4/12: UTF-16 Level 3
                    _currentEncoding = Encoding.BigEndianUnicode;
                }
                result = _currentEncoding.GetString(characters, 4, length - 4);
            }
            else if (length >= 3 && characters[0] == 0x1B && characters[1] == 0x28 && characters[2] == 0x42)
            {
                // ESC 2/8 4/2: ISO 646, U.S. National Character Set (ASCII)
                _currentEncoding = Encoding.ASCII;
                result = _currentEncoding.GetString(characters, 3, length - 3);
            }
            else
            {
                result = _currentEncoding.GetString(characters);
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _reader?.Dispose();

            base.Dispose(disposing);
        }

        internal static Encoding GetDefaultEncoding()
        {
            try
            {
                // try to use ISO 8859-1 as default [ISO/IEC 8632-1 6.3.4.5]
                // "BEGIN METAFILE causes ISO 8859-1 Left Hand Side to be designated as the G0 set
                // and ISO 8859-1, Right Hand Side of Latin Alphabet Nr. 1 to be designated as the G1 set."
                return Encoding.GetEncoding("ISO-8859-1");
            }
            catch
            {
                try
                {
                    // in case it is not available, try to use windows-1252 instead (which is a superset of ISO 8859-1)
                    return Encoding.GetEncoding("Windows-1252");
                }
                catch
                {
                    // in case this also fails, do an absolute fallback to ASCII.
                    // this is most likely incorrect, since we should be using an 8-bit ASCII encoding,
                    // but Encoding.ASCII is only 7-bit; at least it will not trash the output when used.
                    return Encoding.ASCII;
                }
            }
        }
    }
}
