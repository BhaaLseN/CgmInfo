using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Writers;
using BaseMetafileWriter = CgmInfo.MetafileWriter;

namespace CgmInfo.BinaryEncoding
{
    public class MetafileWriter : BaseMetafileWriter
    {
        // ISO/IEC 8632-3 8.1, Table 2
        private static readonly Dictionary<int, Dictionary<int, Action<MetafileWriter, Command>>> _commandTable = new Dictionary<int, Dictionary<int, Action<MetafileWriter, Command>>>
        {
            // delimiter elements [ISO/IEC 8632-3 8.2, Table 3]
            { 0, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 0, DelimiterElementWriter.Noop },
                    { 1, DelimiterElementWriter.BeginMetafile },
                    { 2, DelimiterElementWriter.EndMetafile },
                    { 3, DelimiterElementWriter.BeginPicture },
                    { 4, DelimiterElementWriter.BeginPictureBody },
                    { 5, DelimiterElementWriter.EndPicture },
                    { 6, DelimiterElementWriter.BeginSegment },
                    { 7, DelimiterElementWriter.EndSegment },
                    { 8, DelimiterElementWriter.BeginFigure },
                    { 9, DelimiterElementWriter.EndFigure },
                    // entries 10, 11 and 12 do not exist in ISO/IEC 8632-3
                    { 13, DelimiterElementWriter.BeginProtectionRegion },
                    { 14, DelimiterElementWriter.EndProtectionRegion },
                    { 15, DelimiterElementWriter.BeginCompoundLine },
                    { 16, DelimiterElementWriter.EndCompoundLine },
                    { 17, DelimiterElementWriter.BeginCompoundTextPath },
                    { 18, DelimiterElementWriter.EndCompoundTextPath },
                    { 19, DelimiterElementWriter.BeginTileArray },
                    { 20, DelimiterElementWriter.EndTileArray },
                    { 21, DelimiterElementWriter.BeginApplicationStructure },
                    { 22, DelimiterElementWriter.BeginApplicationStructureBody },
                    { 23, DelimiterElementWriter.EndApplicationStructure },
                }
            },
            // metafile descriptor elements [ISO/IEC 8632-3 8.3, Table 4]
            { 1, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, MetafileDescriptorWriter.MetafileVersion },
                    { 2, MetafileDescriptorWriter.MetafileDescription },
                    { 3, MetafileDescriptorWriter.VdcType },
                    { 4, MetafileDescriptorWriter.IntegerPrecision },
                    { 5, MetafileDescriptorWriter.RealPrecision },
                    { 6, MetafileDescriptorWriter.IndexPrecision },
                    { 7, MetafileDescriptorWriter.ColorPrecision },
                    { 8, MetafileDescriptorWriter.ColorIndexPrecision },
                    { 9, MetafileDescriptorWriter.MaximumColorIndex },
                    { 10, MetafileDescriptorWriter.ColorValueExtent },
                    { 11, MetafileDescriptorWriter.MetafileElementsList },
                    //{ 12, MetafileDescriptorWriter.MetafileDefaultsReplacement },
                    { 13, MetafileDescriptorWriter.FontList },
                    { 14, MetafileDescriptorWriter.CharacterSetList },
                    { 15, MetafileDescriptorWriter.CharacterCodingAnnouncer },
                    { 16, MetafileDescriptorWriter.NamePrecision },
                    { 17, MetafileDescriptorWriter.MaximumVdcExtent },
                    { 18, MetafileDescriptorWriter.SegmentPriorityExtent },
                    { 19, MetafileDescriptorWriter.ColorModel },
                    //{ 20, MetafileDescriptorWriter.ColorCalibration },
                    { 21, MetafileDescriptorWriter.FontProperties },
                    //{ 22, MetafileDescriptorWriter.GlyphMapping },
                    //{ 23, MetafileDescriptorWriter.SymbolLibraryList },
                    //{ 24, MetafileDescriptorWriter.PictureDirectory },
                }
            },
            // picture descriptor elements [ISO/IEC 8632-3 8.4, Table 5]
            { 2, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, PictureDescriptorWriter.ScalingMode },
                    { 2, PictureDescriptorWriter.ColorSelectionMode },
                    { 3, PictureDescriptorWriter.LineWidthSpecificationMode },
                    { 4, PictureDescriptorWriter.MarkerSizeSpecificationMode },
                    { 5, PictureDescriptorWriter.EdgeWidthSpecificationMode },
                    { 6, PictureDescriptorWriter.VdcExtent },
                    { 7, PictureDescriptorWriter.BackgroundColor },
                    { 8, PictureDescriptorWriter.DeviceViewport },
                    { 9, PictureDescriptorWriter.DeviceViewportSpecificationMode },
                    //{ 10, PictureDescriptorWriter.DeviceViewportMapping },
                    //{ 11, PictureDescriptorWriter.LineRepresentation },
                    //{ 12, PictureDescriptorWriter.MarkerRepresentation },
                    //{ 13, PictureDescriptorWriter.TextRepresentation },
                    //{ 14, PictureDescriptorWriter.FillRepresentation },
                    //{ 15, PictureDescriptorWriter.EdgeRepresentation },
                    { 16, PictureDescriptorWriter.InteriorStyleSpecificationMode },
                    { 17, PictureDescriptorWriter.LineAndEdgeTypeDefinition },
                    { 18, PictureDescriptorWriter.HatchStyleDefinition },
                    { 19, PictureDescriptorWriter.GeometricPatternDefinition },
                    //{ 20, PictureDescriptorWriter.ApplicationStructureDirectory },
                }
            },
            // control elements [ISO/IEC 8632-3 8.5, Table 6]
            { 3, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, ControlElementWriter.VdcIntegerPrecision },
                    { 2, ControlElementWriter.VdcRealPrecision },
                    { 3, ControlElementWriter.AuxiliaryColor },
                    { 4, ControlElementWriter.Transparency },
                    { 5, ControlElementWriter.ClipRectangle },
                    { 6, ControlElementWriter.ClipIndicator },
                    { 7, ControlElementWriter.LineClippingMode },
                    { 8, ControlElementWriter.MarkerClippingMode },
                    { 9, ControlElementWriter.EdgeClippingMode },
                    { 10, ControlElementWriter.NewRegion },
                    { 11, ControlElementWriter.SavePrimitiveContext },
                    { 12, ControlElementWriter.RestorePrimitiveContext },
                    // entries 13 until 16 do not exist in ISO/IEC 8632-3
                    { 17, ControlElementWriter.ProtectionRegionIndicator },
                    { 18, ControlElementWriter.GeneralizedTextPathMode },
                    { 19, ControlElementWriter.MiterLimit },
                    //{ 20, ControlElementWriter.TransparentCellColor },
                }
            },
            // graphical primitive elements [ISO/IEC 8632-3 8.6, Table 7]
            { 4, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, GraphicalPrimitiveWriter.Polyline },
                    { 2, GraphicalPrimitiveWriter.DisjointPolyline },
                    { 3, GraphicalPrimitiveWriter.Polymarker },
                    { 4, GraphicalPrimitiveWriter.Text },
                    { 5, GraphicalPrimitiveWriter.RestrictedText },
                    { 6, GraphicalPrimitiveWriter.AppendText },
                    { 7, GraphicalPrimitiveWriter.Polygon },
                    { 8, GraphicalPrimitiveWriter.PolygonSet },
                    { 9, GraphicalPrimitiveWriter.CellArray },
                    //{ 10, GraphicalPrimitiveWriter.GeneralizedDrawingPrimitive },
                    { 11, GraphicalPrimitiveWriter.Rectangle },
                    { 12, GraphicalPrimitiveWriter.Circle },
                    { 13, GraphicalPrimitiveWriter.CircularArc3Point },
                    { 14, GraphicalPrimitiveWriter.CircularArc3PointClose },
                    { 15, GraphicalPrimitiveWriter.CircularArcCenter },
                    { 16, GraphicalPrimitiveWriter.CircularArcCenterClose },
                    { 17, GraphicalPrimitiveWriter.Ellipse },
                    { 18, GraphicalPrimitiveWriter.EllipticalArc },
                    { 19, GraphicalPrimitiveWriter.EllipticalArcClose },
                    { 20, GraphicalPrimitiveWriter.CircularArcCenterReversed },
                    { 21, GraphicalPrimitiveWriter.ConnectingEdge },
                    { 22, GraphicalPrimitiveWriter.HyperbolicArc },
                    { 23, GraphicalPrimitiveWriter.ParabolicArc },
                    { 24, GraphicalPrimitiveWriter.NonUniformBSpline },
                    { 25, GraphicalPrimitiveWriter.NonUniformRationalBSpline },
                    { 26, GraphicalPrimitiveWriter.Polybezier },
                    //{ 27, GraphicalPrimitiveWriter.Polysymbol },
                    { 28, GraphicalPrimitiveWriter.BitonalTile },
                    { 29, GraphicalPrimitiveWriter.Tile },
                }
            },
            // attribute elements [ISO/IEC 8632-3 8.7, Table 8]
            { 5, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, AttributeWriter.LineBundleIndex },
                    { 2, AttributeWriter.LineType },
                    { 3, AttributeWriter.LineWidth },
                    { 4, AttributeWriter.LineColor },
                    { 5, AttributeWriter.MarkerBundleIndex },
                    { 6, AttributeWriter.MarkerType },
                    { 7, AttributeWriter.MarkerSize },
                    { 8, AttributeWriter.MarkerColor },
                    { 9, AttributeWriter.TextBundleIndex },
                    { 10, AttributeWriter.TextFontIndex },
                    { 11, AttributeWriter.TextPrecision },
                    { 12, AttributeWriter.CharacterExpansionFactor },
                    { 13, AttributeWriter.CharacterSpacing },
                    { 14, AttributeWriter.TextColor },
                    { 15, AttributeWriter.CharacterHeight },
                    { 16, AttributeWriter.CharacterOrientation },
                    { 17, AttributeWriter.TextPath },
                    { 18, AttributeWriter.TextAlignment },
                    { 19, AttributeWriter.CharacterSetIndex },
                    { 20, AttributeWriter.AlternateCharacterSetIndex },
                    { 21, AttributeWriter.FillBundleIndex },
                    { 22, AttributeWriter.InteriorStyle },
                    { 23, AttributeWriter.FillColor },
                    { 24, AttributeWriter.HatchIndex },
                    { 25, AttributeWriter.PatternIndex },
                    { 26, AttributeWriter.EdgeBundleIndex },
                    { 27, AttributeWriter.EdgeType },
                    { 28, AttributeWriter.EdgeWidth },
                    { 29, AttributeWriter.EdgeColor },
                    { 30, AttributeWriter.EdgeVisibility },
                    { 31, AttributeWriter.FillReferencePoint },
                    { 32, AttributeWriter.PatternTable },
                    { 33, AttributeWriter.PatternSize },
                    { 34, AttributeWriter.ColorTable },
                    { 35, AttributeWriter.AspectSourceFlags },
                    { 36, AttributeWriter.PickIdentifier },
                    { 37, AttributeWriter.LineCap },
                    { 38, AttributeWriter.LineJoin },
                    { 39, AttributeWriter.LineTypeContinuation },
                    { 40, AttributeWriter.LineTypeInitialOffset },
                    //{ 41, AttributeWriter.TextScoreType },
                    { 42, AttributeWriter.RestrictedTextType },
                    { 43, AttributeWriter.InterpolatedInterior },
                    { 44, AttributeWriter.EdgeCap },
                    { 45, AttributeWriter.EdgeJoin },
                    { 46, AttributeWriter.EdgeTypeContinuation },
                    { 47, AttributeWriter.EdgeTypeInitialOffset },
                    //{ 48, AttributeWriter.SymbolLibraryIndex },
                    //{ 49, AttributeWriter.SymbolColor },
                    //{ 50, AttributeWriter.SymbolSize },
                    //{ 51, AttributeWriter.SymbolOrientation },
                }
            },
            // escape elements [ISO/IEC 8632-3 8.8, Table 9]
            { 6, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, EscapeWriter.Escape },
                }
            },
            // external elements [ISO/IEC 8632-3 8.9, Table 10]
            { 7, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, ExternalWriter.Message },
                    { 2, ExternalWriter.ApplicationData },
                }
            },
        };

        private bool _insideMetafile;
        // assume a default ISO 8859-1 [ISO/IEC 8632-1 6.3.4.5]
        private Encoding _currentEncoding = MetafileReader.GetDefaultEncoding();

        public MetafileWriter(string fileName)
            : base(fileName)
        {
        }
        private MetafileWriter(MetafileWriter parent, MemoryStream buffer)
            : base(parent, buffer)
        {
        }
        public override void Write(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_insideMetafile)
            {
                if (command is BeginMetafile)
                    throw new FormatException("Did not expect Element Id 1 (BEGIN METAFILE) inside a Metafile");
                else if (command is EndMetafile)
                    _insideMetafile = false;
            }
            else
            {
                if (command is BeginMetafile)
                    _insideMetafile = true;
                else
                    throw new FormatException("Expected Element Id 1 (BEGIN METAFILE) at the beginning of a Metafile");
            }

            if (!_commandTable.TryGetValue(command.ElementClass, out var classCommands) || classCommands == null ||
                !classCommands.TryGetValue(command.ElementId, out var commandHandler) || commandHandler == null)
            {
                // TODO: write Noop instead of doing nothing?
                //commandHandler = DelimiterElementWriter.Noop;
                return;
            }

            using var ms = new MemoryStream();
            using var bufferWriter = new MetafileWriter(this, ms);
            commandHandler(bufferWriter, command);

            // commands are always word aligned [ISO/IEC 8632-3 5.4]
            EnsureWordAligned();

            byte[] buffer = ms.ToArray();
            const byte MaxShortParameterListLength = 0x1F;
            int parameterListLength = Math.Min(buffer.Length, MaxShortParameterListLength);
            WriteCommandHeader(command.ElementClass, command.ElementId, parameterListLength);
            if (parameterListLength == MaxShortParameterListLength)
                WriteLongParameterListLength(buffer.Length);

            WriteBuffer(buffer);
        }

        internal override void EnsureWordAligned()
        {
            if (_stream.Position % 2 == 1)
                WriteByte(0);
        }

        private void WriteBuffer(byte[] buffer) => _stream.Write(buffer, 0, buffer.Length);

        private void WriteCommandHeader(int elementClass, int elementId, int parameterListLength)
        {
            const int MaxElementClassValue = 0xF;
            const int MaxElementIdValue = 0x7F;
            ushort commandHeader = (ushort)(((elementClass & MaxElementClassValue) << 12) | ((elementId & MaxElementIdValue) << 5) | (parameterListLength & 0x1F));
            WriteWord(commandHeader);
        }
        private void WriteLongParameterListLength(int parameterListLength)
        {
            const int MaxLongParameterListLength = 0x7FFF;
            while (parameterListLength > 0)
            {
                // the lower 15 bits are the actual length of this partition
                int partitionLength = Math.Min(parameterListLength, MaxLongParameterListLength);
                // top-most bit indicates whether more partitions follow or not
                bool isLastPartition = partitionLength < MaxLongParameterListLength;
                int lastPartitionFlag = isLastPartition ? 0 : 1;
                parameterListLength -= partitionLength;

                ushort longFormCommandHeader = (ushort)((lastPartitionFlag << 15) | (partitionLength & MaxLongParameterListLength));
                WriteWord(longFormCommandHeader);
            }
        }
        internal override void WriteInteger(int value)
        {
            // integer is a signed integer at integer precision [ISO/IEC 8632-3 7, Table 1, I]
            WriteInteger(value, Descriptor.IntegerPrecision);
        }
        internal override void WriteInteger(int value, int integerPrecision)
        {
            int numBytes = integerPrecision / 8;
            if (numBytes < 1 || numBytes > 4)
                throw new ArgumentOutOfRangeException(nameof(integerPrecision), integerPrecision, "Precision must be a multiple of 8 between 8 and 32");

            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            for (int i = bytes.Length - numBytes; i < bytes.Length; i++)
                WriteByte(bytes[i]);
        }
        internal override void WriteWord(ushort value)
        {
            WriteInteger(value, 16);
        }
        internal override void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }
        private void WriteIntAsByte(int value) => WriteByte((byte)(value & 0xFF));

        internal override void WriteFixedPoint(double value, int precision)
        {
            // ISO/IEC 8632-3 6.4
            // real value is computed as "whole + (fraction / 2**exp)"
            // exp is the width of the fraction value
            // the "whole part" has the same form as a Signed Integer
            int whole = (int)value;
            WriteInteger(whole, precision / 2);
            // if someone wanted a 4 byte fixed point real, they get 32 bits (16 bits whole, 16 bits fraction)
            // therefore exp would be 16 here (same for 8 byte with 64 bits and 32/32 -> 32 exp)
            int exp = precision / 2;
            double scale = Math.Pow(2, exp);
            // the "fractional part" has the same form as an Unsigned Integer
            int fraction = (int)Math.Round((value - whole) * scale);
            WriteInteger(fraction, precision / 2);
        }
        internal override void WriteFloatingPoint(double value, int precision)
        {
            // ISO/IEC 8632-3 6.5
            // C# float/double conform to ANSI/IEEE 754 and have the same format as the specification wants;
            // but the endianness might not work out. swap if necessary
            if (precision == 32)
            {
                byte[] floatBytes = BitConverter.GetBytes((float)value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(floatBytes);
                WriteBuffer(floatBytes);
                return;
            }
            if (precision == 64)
            {
                byte[] doubleBytes = BitConverter.GetBytes(value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(doubleBytes);
                WriteBuffer(doubleBytes);
                return;
            }

            throw new InvalidOperationException(string.Format("Sorry, cannot write a floating point value with {0} bytes", precision));
        }
        internal override void WriteVdc(double value)
        {
            // a VDC is either an int or a double; depending on what VDC TYPE said [ISO/IEC 8632-3 7, Table 1, Note 7]
            if (Descriptor.VdcType == VdcTypeSpecification.Integer)
            {
                // value is a signed integer at VDC Integer Precision
                WriteInteger((int)value, Descriptor.VdcIntegerPrecision);
                return;
            }
            else if (Descriptor.VdcType == VdcTypeSpecification.Real)
            {
                WriteReal(value, Descriptor.VdcRealPrecision);
                return;
            }

            throw new NotSupportedException("The current VDC TYPE is not supported");
        }

        internal override void WriteString(string value)
        {
            int stringLength = value?.Length ?? 0;

            // switch to UTF-8 if the current encoding isn't good enough. it should work for everything else.
            bool needDifferentEncoding = !_currentEncoding.Supports(value);
            if (needDifferentEncoding)
                stringLength += 4;

            // string starts with a length byte [ISO/IEC 8632-3 7, Table 1, Note 6]
            WriteIntAsByte(stringLength);
            // long string: length of 255 indicates that either one or two words follow
            if (stringLength >= 255)
            {
                // TODO: can we safely (ab)use the same logic here?
                WriteLongParameterListLength(stringLength);
            }

            if (needDifferentEncoding)
            {
                // switch to a different encodings (based on ISO/IEC 2022 / ECMA-35) - we chose UTF-8 because it likely fits everything.
                // we do this using DOCS (DECIDE OTHER CODING SYSTEM), identified by "ESC % / F" (0x1B 0x25 0x2F F)
                // with F = "I" (0x49), which indicates UTF-8 Level 3 support. This should be enough for everything.
                // [ISO-IR International Register of Coded Character Sets to be used with Escape Sequences, 2.8.2]
                // [ISO/IEC 8632-1 6.3.4.5, Example 1]
                WriteByte(0x1B);
                WriteByte(0x25);
                WriteByte(0x2F);
                WriteByte(0x49);
                _currentEncoding = Encoding.UTF8;
            }

            byte[] characters = _currentEncoding.GetBytes(value);
            WriteBuffer(characters);
        }

        internal override void WriteIndex(int value, int indexPrecision)
        {
            // index is a signed integer at index precision [ISO/IEC 8632-3 7, Table 1, IX]
            WriteInteger(value, indexPrecision);
        }
        internal override void WriteColorIndex(int value, int colorIndexPrecision)
        {
            WriteInteger(value, colorIndexPrecision);
        }
        internal override void WriteColorValue(int value, int colorPrecision)
        {
            // color components are unsigned integers at direct color precision
            // FIXME: color component in CIELAB/CIELUV/RGB-related is reals, not ints; but we only use this method for int at this point.
            WriteInteger(value, colorPrecision);
        }
        internal override void WriteName(int value)
        {
            // name is a signed integer at name precision [ISO/IEC 8632-3 7, Table 1, N]
            WriteInteger(value, Descriptor.NamePrecision);
        }
        internal override void WriteEnum<TEnum>(TEnum value)
        {
            // enum is a signed integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, E / Note 3]
            WriteInteger(Convert.ToInt32(value), 16);
        }
        internal override void WriteBitStream(byte[] data)
        {
            // bitstream is a series of unsigned integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, BS / Note 15]
            // 16 bits per entry is chosen for portability reasons and need not be filled completely; the remainder is set to 0.
            // the data is little endian; swap if necessary.
            if (!BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    byte temp = data[i];
                    data[i] = data[i + 1];
                    data[i + 1] = temp;
                }
            }
            WriteBuffer(data);
        }
        internal override void WriteStructuredDataRecord(StructuredDataRecord value)
        {
            using var ms = new MemoryStream();
            using var writer = new MetafileWriter(this, ms);

            foreach (var element in value.Elements)
                writer.WriteStructuredDataElement(element);

            byte[] sdeBuffer = ms.ToArray();
            int shortLength = Math.Min(sdeBuffer.Length, 255);
            WriteIntAsByte(shortLength);
            if (shortLength == 255)
                WriteWord((ushort)sdeBuffer.Length);
            WriteBuffer(sdeBuffer);
        }
    }
}
