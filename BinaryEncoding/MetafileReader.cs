using System;
using System.Collections.Generic;
using System.Drawing;
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
        private BinaryReader _reader;
        private bool _insideMetafile;
        // assume a default ASCII unless I misunderstood the spec [ISO/IEC 8632-1 6.3.4.5, Example 2]
        private Encoding _currentEncoding = Encoding.ASCII;

        public MetafileReader(string fileName)
            : base(fileName, true)
        {
        }

        public static bool IsBinaryMetafile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "stream is null.");
            if (!stream.CanSeek)
                throw new InvalidOperationException("Cannot seek the stream.");

            stream.Seek(0, SeekOrigin.Begin);
            ushort commandHeader = stream.ReadWord();
            int elementClass = (commandHeader >> 12) & 0xF;
            int elementId = (commandHeader >> 5) & 0x7F;

            // check whether the first two bytes are 0/1 (BEGIN METAFILE)
            return elementClass == 0 && elementId == 1;
        }
        protected override Command ReadCommand(Stream stream)
        {
            // stop at EOF; or when we cannot at least read another command header
            if (stream.Position + 2 > stream.Length)
                return null;

            Command result;
            CommandHeader commandHeader = ReadCommandHeader(stream);
            // special case: we might encounter a no-op after END METAFILE, which leads into EOF.
            // ReadCommandHeader will return null in that case, and we should simply pass this on here.
            if (commandHeader == null)
                return null;

            // ISO/IEC 8632-3 8.1, Table 2
            switch (commandHeader.ElementClass)
            {
                case 0: // delimiter
                    result = ReadDelimiterElement(commandHeader);
                    break;
                case 1: // metafile descriptor
                    result = ReadMetafileDescriptorElement(commandHeader);
                    break;
                case 2: // picture descriptor
                    result = ReadPictureDescriptorElement(commandHeader);
                    break;
                case 3: // control
                    result = ReadControlElement(commandHeader);
                    break;
                case 4: // graphical primitive
                    result = ReadGraphicalPrimitive(commandHeader);
                    break;
                case 5: // attribute
                    result = ReadAttribute(commandHeader);
                    break;
                case 9: // application structure descriptor
                    result = ReadApplicationStructureDescriptor(commandHeader);
                    break;
                case 6: // escape
                case 7: // external
                case 8: // segment control/segment attribute
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
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
                        throw new FormatException(string.Format(
                            "Found Element Id 2 (END METAFILE), but got {0} bytes left to read. Multiple Metafiles within a single file are not supported.",
                            stream.Length - stream.Position - 1));
                }
                else if (result.ElementId != 1)
                    throw new FormatException("Expected Element Id 1 (BEGIN METAFILE) at the beginning of a Metafile");
            }

            return result;
        }

        private CommandHeader ReadCommandHeader(Stream stream)
        {
            // commands are always word aligned [ISO/IEC 8632-3 5.4]
            if (stream.Position % 2 == 1)
                stream.Seek(1, SeekOrigin.Current);

            ushort commandHeader = stream.ReadWord();
            int elementClass = (commandHeader >> 12) & 0xF;
            int elementId = (commandHeader >> 5) & 0x7F;
            int parameterListLength = commandHeader & 0x1F;

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

                } while (!isLastPartition);
            }
            else
            {
                // short command form; buffer the contents directly
                byte[] buffer = new byte[parameterListLength];
                stream.Read(buffer, 0, buffer.Length);
                readBuffer.Write(buffer, 0, buffer.Length);
            }

            readBuffer.Position = 0;

            if (_reader != null)
                _reader.Dispose();
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

            return new CommandHeader(elementClass, elementId, parameterListLength);
        }

        private Command ReadUnsupportedElement(CommandHeader commandHeader)
        {
            // no need to seek here anymore; the whole unsupported element has been read into the temporary buffer already anyways
            return new UnsupportedCommand(commandHeader.ElementClass, commandHeader.ElementId);
        }

        private Command ReadDelimiterElement(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.2, Table 3
            switch (commandHeader.ElementId)
            {
                //case 0: // no-op; these are skipped already while reading the command header
                case 1: // BEGIN METAFILE
                    result = DelimiterElementReader.BeginMetafile(this, commandHeader);
                    _insideMetafile = result != null;
                    break;
                case 2: // END METAFILE
                    result = DelimiterElementReader.EndMetafile(this, commandHeader);
                    _insideMetafile = false;
                    break;
                case 3: // BEGIN PICTURE
                    result = DelimiterElementReader.BeginPicture(this, commandHeader);
                    break;
                case 4: // BEGIN PICTURE BODY
                    result = DelimiterElementReader.BeginPictureBody(this, commandHeader);
                    break;
                case 5: // END PICTURE
                    result = DelimiterElementReader.EndPicture(this, commandHeader);
                    break;
                case 6: // BEGIN SEGMENT
                    result = DelimiterElementReader.BeginSegment(this, commandHeader);
                    break;
                case 7: // END SEGMENT
                    result = DelimiterElementReader.EndSegment(this, commandHeader);
                    break;
                case 8: // BEGIN FIGURE
                    result = DelimiterElementReader.BeginFigure(this, commandHeader);
                    break;
                case 9: // END FIGURE
                    result = DelimiterElementReader.EndFigure(this, commandHeader);
                    break;
                case 13: // BEGIN PROTECTION REGION
                    result = DelimiterElementReader.BeginProtectionRegion(this, commandHeader);
                    break;
                case 14: // END PROTECTION REGION
                    result = DelimiterElementReader.EndProtectionRegion(this, commandHeader);
                    break;
                case 15: // BEGIN COMPOUND LINE
                    result = DelimiterElementReader.BeginCompoundLine(this, commandHeader);
                    break;
                case 16: // END COMPOUND LINE
                    result = DelimiterElementReader.EndCompoundLine(this, commandHeader);
                    break;
                case 17: // BEGIN COMPOUND TEXT PATH
                    result = DelimiterElementReader.BeginCompoundTextPath(this, commandHeader);
                    break;
                case 18: // END COMPOUND TEXT PATH
                    result = DelimiterElementReader.EndCompoundTextPath(this, commandHeader);
                    break;
                case 19: // BEGIN TILE ARRAY
                    result = DelimiterElementReader.BeginTileArray(this, commandHeader);
                    break;
                case 20: // END TILE ARRAY
                    result = DelimiterElementReader.EndTileArray(this, commandHeader);
                    break;
                case 21: // BEGIN APPLICATION STRUCTURE
                    result = DelimiterElementReader.BeginApplicationStructure(this, commandHeader);
                    break;
                case 22: // BEGIN APPLICATION STRUCTURE BODY
                    result = DelimiterElementReader.BeginApplicationStructureBody(this, commandHeader);
                    break;
                case 23: // END APPLICATION STRUCTURE
                    result = DelimiterElementReader.EndApplicationStructure(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadMetafileDescriptorElement(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.3, Table 4
            switch (commandHeader.ElementId)
            {
                case 1: // METAFILE VERSION
                    result = MetafileDescriptorReader.MetafileVersion(this, commandHeader);
                    break;
                case 2: // METAFILE DESCRIPTION
                    result = MetafileDescriptorReader.MetafileDescription(this, commandHeader);
                    break;
                case 3: // VDC TYPE
                    var vdcType = MetafileDescriptorReader.VdcType(this, commandHeader);
                    Descriptor.VdcType = vdcType.Specification;
                    result = vdcType;
                    break;
                case 4: // INTEGER PRECISION
                    var integerPrecision = MetafileDescriptorReader.IntegerPrecision(this, commandHeader);
                    Descriptor.IntegerPrecision = integerPrecision.Precision;
                    result = integerPrecision;
                    break;
                case 5: // REAL PRECISION
                    var realPrecision = MetafileDescriptorReader.RealPrecision(this, commandHeader);
                    Descriptor.RealPrecision = realPrecision.Specification;
                    result = realPrecision;
                    break;
                case 6: // INDEX PRECISION
                    var indexPrecision = MetafileDescriptorReader.IndexPrecision(this, commandHeader);
                    Descriptor.IndexPrecision = indexPrecision.Precision;
                    result = indexPrecision;
                    break;
                case 7: // COLOUR PRECISION
                    var colorPrecision = MetafileDescriptorReader.ColorPrecision(this, commandHeader);
                    Descriptor.ColorPrecision = colorPrecision.Precision;
                    result = colorPrecision;
                    break;
                case 8: // COLOUR INDEX PRECISION
                    var colorIndexPrecision = MetafileDescriptorReader.ColorIndexPrecision(this, commandHeader);
                    Descriptor.ColorIndexPrecision = colorIndexPrecision.Precision;
                    result = colorIndexPrecision;
                    break;
                case 9: // MAXIMUM COLOUR INDEX
                    result = MetafileDescriptorReader.MaximumColorIndex(this, commandHeader);
                    break;
                case 10: // COLOUR VALUE EXTENT
                    result = MetafileDescriptorReader.ColorValueExtent(this, commandHeader);
                    break;
                case 11: // METAFILE ELEMENTS LIST
                    result = MetafileDescriptorReader.MetafileElementsList(this, commandHeader);
                    break;
                case 12: // METAFILE DEFAULTS REPLACEMENT
                    result = ReadMetafileDefaultsReplacement(commandHeader);
                    break;
                case 13: // FONT LIST
                    result = MetafileDescriptorReader.FontList(this, commandHeader);
                    break;
                case 14: // CHARACTER SET LIST
                    result = MetafileDescriptorReader.CharacterSetList(this, commandHeader);
                    break;
                case 15: // CHARACTER CODING ANNOUNCER
                    result = MetafileDescriptorReader.CharacterCodingAnnouncer(this, commandHeader);
                    break;
                case 16: // NAME PRECISION
                    var namePrecision = MetafileDescriptorReader.NamePrecision(this, commandHeader);
                    Descriptor.NamePrecision = namePrecision.Precision;
                    result = namePrecision;
                    break;
                case 17: // MAXIMUM VDC EXTENT
                    result = MetafileDescriptorReader.MaximumVdcExtent(this, commandHeader);
                    break;
                case 19: // COLOUR MODEL
                    var colorModel = MetafileDescriptorReader.ColorModelCommand(this, commandHeader);
                    Descriptor.ColorModel = colorModel.ColorModel;
                    result = colorModel;
                    break;
                case 18: // SEGMENT PRIORITY EXTENT
                case 20: // COLOUR CALIBRATION
                case 21: // FONT PROPERTIES
                case 22: // GLYPH MAPPING
                case 23: // SYMBOL LIBRARY LIST
                case 24: // PICTURE DIRECTORY
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadPictureDescriptorElement(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.4, Table 5
            switch (commandHeader.ElementId)
            {
                case 1: // SCALING MODE
                    result = PictureDescriptorReader.ScalingMode(this, commandHeader);
                    break;
                case 2: // COLOUR SELECTION MODE
                    var colorSelectionMode = PictureDescriptorReader.ColorSelectionMode(this, commandHeader);
                    Descriptor.ColorSelectionMode = colorSelectionMode.ColorMode;
                    result = colorSelectionMode;
                    break;
                case 3: // LINE WIDTH SPECIFICATION MODE
                    var lineWidthSpecificationMode = PictureDescriptorReader.LineWidthSpecificationMode(this, commandHeader);
                    Descriptor.LineWidthSpecificationMode = lineWidthSpecificationMode.WidthSpecificationMode;
                    result = lineWidthSpecificationMode;
                    break;
                case 4: // MARKER SIZE SPECIFICATION MODE
                    var markerSizeSpecificationMode = PictureDescriptorReader.MarkerSizeSpecificationMode(this, commandHeader);
                    Descriptor.MarkerSizeSpecificationMode = markerSizeSpecificationMode.WidthSpecificationMode;
                    result = markerSizeSpecificationMode;
                    break;
                case 5: // EDGE WIDTH SPECIFICATION MODE
                    var edgeWidthSpecificationMode = PictureDescriptorReader.EdgeWidthSpecificationMode(this, commandHeader);
                    Descriptor.EdgeWidthSpecificationMode = edgeWidthSpecificationMode.WidthSpecificationMode;
                    result = edgeWidthSpecificationMode;
                    break;
                case 6: // VDC EXTENT
                    result = PictureDescriptorReader.VdcExtent(this, commandHeader);
                    break;
                case 7: // BACKGROUND COLOUR
                    result = PictureDescriptorReader.BackgroundColor(this, commandHeader);
                    break;
                case 8: // DEVICE VIEWPORT
                    result = PictureDescriptorReader.DeviceViewport(this, commandHeader);
                    break;
                case 9: // DEVICE VIEWPORT SPECIFICATION MODE
                    var deviceViewportSpecificationMode = PictureDescriptorReader.DeviceViewportSpecificationMode(this, commandHeader);
                    Descriptor.DeviceViewportSpecificationMode = deviceViewportSpecificationMode.SpecificationMode;
                    result = deviceViewportSpecificationMode;
                    break;
                case 16: // INTERIOR STYLE SPECIFICATION MODE
                    var interiorStyleSpecificationMode = PictureDescriptorReader.InteriorStyleSpecificationMode(this, commandHeader);
                    Descriptor.InteriorStyleSpecificationMode = interiorStyleSpecificationMode.WidthSpecificationMode;
                    result = interiorStyleSpecificationMode;
                    break;
                case 17: // LINE AND EDGE TYPE DEFINITION
                    result = PictureDescriptorReader.LineAndEdgeTypeDefinition(this, commandHeader);
                    break;
                case 18: // HATCH STYLE DEFINITION
                    result = PictureDescriptorReader.HatchStyleDefinition(this, commandHeader);
                    break;
                case 19: // GEOMETRIC PATTERN DEFINITION
                    result = PictureDescriptorReader.GeometricPatternDefinition(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadControlElement(CommandHeader commandHeader)
        {
            Command result;
            switch (commandHeader.ElementId)
            {
                case 1: // VDC INTEGER PRECISION
                    var vdcIntegerPrecision = ControlElementReader.VdcIntegerPrecision(this, commandHeader);
                    Descriptor.VdcIntegerPrecision = vdcIntegerPrecision.Precision;
                    result = vdcIntegerPrecision;
                    break;
                case 2: // VDC REAL PRECISION
                    var vdcRealPrecision = ControlElementReader.VdcRealPrecision(this, commandHeader);
                    Descriptor.VdcRealPrecision = vdcRealPrecision.Specification;
                    result = vdcRealPrecision;
                    break;
                case 3: // AUXILIARY COLOR
                    result = ControlElementReader.AuxiliaryColor(this, commandHeader);
                    break;
                case 4: // TRANSPARENCY
                    result = ControlElementReader.Transparency(this, commandHeader);
                    break;
                case 5: // CLIP RECTANGLE
                    result = ControlElementReader.ClipRectangle(this, commandHeader);
                    break;
                case 6: // CLIP INDICATOR
                    result = ControlElementReader.ClipIndicator(this, commandHeader);
                    break;
                case 7: // LINE CLIPPING MODE
                    result = ControlElementReader.LineClippingMode(this, commandHeader);
                    break;
                case 8: // MARKER CLIPPING MODE
                    result = ControlElementReader.MarkerClippingMode(this, commandHeader);
                    break;
                case 9: // EDGE CLIPPING MODE
                    result = ControlElementReader.EdgeClippingMode(this, commandHeader);
                    break;
                case 10: // NEW REGION
                    result = ControlElementReader.NewRegion(this, commandHeader);
                    break;
                case 11: // SAVE PRIMITIVE CONTEXT
                    result = ControlElementReader.SavePrimitiveContext(this, commandHeader);
                    break;
                case 12: // RESTORE PRIMITIVE CONTEXT
                    result = ControlElementReader.RestorePrimitiveContext(this, commandHeader);
                    break;
                case 17: // PROTECTION REGION INDICATOR
                    result = ControlElementReader.ProtectionRegionIndicator(this, commandHeader);
                    break;
                case 18: // GENERALIZED TEXT PATH MODE
                    result = ControlElementReader.GeneralizedTextPathMode(this, commandHeader);
                    break;
                case 19: // MITRE LIMIT
                    result = ControlElementReader.MiterLimit(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadGraphicalPrimitive(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.6, Table 7
            switch (commandHeader.ElementId)
            {
                case 1: // POLYLINE
                    result = GraphicalPrimitiveReader.Polyline(this, commandHeader);
                    break;
                case 4: // TEXT
                    result = GraphicalPrimitiveReader.Text(this, commandHeader);
                    break;
                case 5: // RESTRICTED TEXT
                    result = GraphicalPrimitiveReader.RestrictedText(this, commandHeader);
                    break;
                case 6: // APPEND TEXT
                    result = GraphicalPrimitiveReader.AppendText(this, commandHeader);
                    break;
                case 7: // POLYGON
                    result = GraphicalPrimitiveReader.Polygon(this, commandHeader);
                    break;
                case 11: // RECTANGLE
                    result = GraphicalPrimitiveReader.Rectangle(this, commandHeader);
                    break;
                case 12: // CIRCLE
                    result = GraphicalPrimitiveReader.Circle(this, commandHeader);
                    break;
                case 15: // CIRCULAR ARC CENTER
                    result = GraphicalPrimitiveReader.CircularArcCenter(this, commandHeader);
                    break;
                case 17: // ELLIPSE
                    result = GraphicalPrimitiveReader.Ellipse(this, commandHeader);
                    break;
                case 18: // ELLIPTICAL ARC
                    result = GraphicalPrimitiveReader.EllipticalArc(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadAttribute(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.11, Table 8
            switch (commandHeader.ElementId)
            {
                case 1: // LINE BUNDLE INDEX
                    result = AttributeReader.LineBundleIndex(this, commandHeader);
                    break;
                case 2: // LINE TYPE
                    result = AttributeReader.LineType(this, commandHeader);
                    break;
                case 3: // LINE WIDTH
                    result = AttributeReader.LineWidth(this, commandHeader);
                    break;
                case 4: // LINE COLOUR
                    result = AttributeReader.LineColor(this, commandHeader);
                    break;
                case 5: // MARKER BUNDLE INDEX
                    result = AttributeReader.MarkerBundleIndex(this, commandHeader);
                    break;
                case 6: // MARKER TYPE
                    result = AttributeReader.MarkerType(this, commandHeader);
                    break;
                case 7: // MARKER SIZE
                    result = AttributeReader.MarkerSize(this, commandHeader);
                    break;
                case 8: // MARKER COLOUR
                    result = AttributeReader.MarkerColor(this, commandHeader);
                    break;
                case 9: // TEXT BUNDLE INDEX
                    result = AttributeReader.TextBundleIndex(this, commandHeader);
                    break;
                case 10: // TEXT FONT INDEX
                    result = AttributeReader.TextFontIndex(this, commandHeader);
                    break;
                case 11: // TEXT PRECISION
                    result = AttributeReader.TextPrecision(this, commandHeader);
                    break;
                case 12: // CHARACTER EXPANSION FACTOR
                    result = AttributeReader.CharacterExpansionFactor(this, commandHeader);
                    break;
                case 13: // CHARACTER SPACING
                    result = AttributeReader.CharacterSpacing(this, commandHeader);
                    break;
                case 14: // TEXT COLOUR
                    result = AttributeReader.TextColor(this, commandHeader);
                    break;
                case 15: // CHARACTER HEIGHT
                    result = AttributeReader.CharacterHeight(this, commandHeader);
                    break;
                case 16: // CHARACTER ORIENTATION
                    result = AttributeReader.CharacterOrientation(this, commandHeader);
                    break;
                case 17: // TEXT PATH
                    result = AttributeReader.TextPath(this, commandHeader);
                    break;
                case 18: // TEXT ALIGNMENT
                    result = AttributeReader.TextAlignment(this, commandHeader);
                    break;
                case 19: // CHARACTER SET INDEX
                    result = AttributeReader.CharacterSetIndex(this, commandHeader);
                    break;
                case 20: // ALTERNATE CHARACTER SET INDEX
                    result = AttributeReader.AlternateCharacterSetIndex(this, commandHeader);
                    break;
                case 21: // FILL BUNDLE INDEX
                    result = AttributeReader.FillBundleIndex(this, commandHeader);
                    break;
                case 22: // INTERIOR STYLE
                    result = AttributeReader.InteriorStyle(this, commandHeader);
                    break;
                case 23: // FILL COLOUR
                    result = AttributeReader.FillColor(this, commandHeader);
                    break;
                case 24: // HATCH INDEX
                    result = AttributeReader.HatchIndex(this, commandHeader);
                    break;
                case 25: // PATTERN INDEX
                    result = AttributeReader.PatternIndex(this, commandHeader);
                    break;
                case 26: // EDGE BUNDLE INDEX
                    result = AttributeReader.EdgeBundleIndex(this, commandHeader);
                    break;
                case 27: // EDGE TYPE
                    result = AttributeReader.EdgeType(this, commandHeader);
                    break;
                case 28: // EDGE WIDTH
                    result = AttributeReader.EdgeWidth(this, commandHeader);
                    break;
                case 29: // EDGE COLOUR
                    result = AttributeReader.EdgeColor(this, commandHeader);
                    break;
                case 30: // EDGE VISIBILITY
                    result = AttributeReader.EdgeVisibility(this, commandHeader);
                    break;
                case 31: // FILL REFERENCE POINT
                    result = AttributeReader.FillReferencePoint(this, commandHeader);
                    break;
                case 32: // PATTERN TABLE
                    result = AttributeReader.PatternTable(this, commandHeader);
                    break;
                case 33: // PATTERN SIZE
                    result = AttributeReader.PatternSize(this, commandHeader);
                    break;
                case 34: // COLOUR TABLE
                    var colorTable = AttributeReader.ColorTable(this, commandHeader);
                    Descriptor.UpdateColorTable(colorTable);
                    result = colorTable;
                    break;
                case 35: // ASPECT SOURCE FLAGS
                    result = AttributeReader.AspectSourceFlags(this, commandHeader);
                    break;
                case 36: // PICK IDENTIFIER
                    result = AttributeReader.PickIdentifier(this, commandHeader);
                    break;
                case 37: // LINE CAP
                    result = AttributeReader.LineCap(this, commandHeader);
                    break;
                case 38: // LINE JOIN
                    result = AttributeReader.LineJoin(this, commandHeader);
                    break;
                case 39: // LINE TYPE CONTINUATION
                    result = AttributeReader.LineTypeContinuation(this, commandHeader);
                    break;
                case 40: // LINE TYPE INITIAL OFFSET
                    result = AttributeReader.LineTypeInitialOffset(this, commandHeader);
                    break;
                case 42: // RESTRICTED TEXT TYPE
                    result = AttributeReader.RestrictedTextType(this, commandHeader);
                    break;
                case 43: // INTERPOLATED INTERIOR
                    result = AttributeReader.InterpolatedInterior(this, commandHeader);
                    break;
                case 44: // EDGE CAP
                    result = AttributeReader.EdgeCap(this, commandHeader);
                    break;
                case 45: // EDGE JOIN
                    result = AttributeReader.EdgeJoin(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadApplicationStructureDescriptor(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.11, Table 12
            switch (commandHeader.ElementId)
            {
                case 1: // APPLICATION STRUCTURE ATTRIBUTE
                    result = ApplicationStructureDescriptorReader.ApplicationStructureAttribute(this, commandHeader);
                    break;
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private MetafileDefaultsReplacement ReadMetafileDefaultsReplacement(CommandHeader commandHeader)
        {
            // this is a memory stream set by ReadCommandHeader, which contains 1..n commands itself.
            // however, _reader is disposed after every run, so we need to keep this buffer around another way.
            using (var replacementsStream = new MemoryStream())
            {
                _reader.BaseStream.CopyTo(replacementsStream);
                replacementsStream.Position = 0;

                var commands = new List<Command>();
                while (true)
                {
                    var command = ReadCommand(replacementsStream);
                    if (command == null)
                        break;
                    commands.Add(command);
                }
                return new MetafileDefaultsReplacement(commands.ToArray());
            }
        }

        internal bool HasMoreData()
        {
            return HasMoreData(1);
        }

        internal bool HasMoreData(int minimumLeft)
        {
            return _reader != null && _reader.BaseStream.Position + minimumLeft <= _reader.BaseStream.Length;
        }

        internal int ReadInteger()
        {
            // integer is a signed integer at integer precision [ISO/IEC 8632-3 7, Table 1, I]
            return ReadInteger(Descriptor.IntegerPrecision / 8, false);
        }
        internal int ReadInteger(int numBytes, bool unsigned)
        {
            if (numBytes < 1 || numBytes > 4)
                throw new ArgumentOutOfRangeException("numBytes", numBytes, "Number of bytes must be between 1 and 4");
            uint ret = 0;
            int signBit = 1 << ((numBytes * 8) - 1);
            int maxUnsignedValue = 1 << (numBytes * 8);
            while (numBytes --> 0)
                ret = (ret << 8) | ReadByte();
            int signedRet = (int)ret;
            if (!unsigned && (ret & signBit) > 0)
                signedRet = signedRet - maxUnsignedValue;
            return signedRet;
        }

        internal int ReadEnum()
        {
            // enum is a signed integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, E / Note 3]
            return ReadInteger(2, false);
        }
        internal TEnum ReadEnum<TEnum>() where TEnum : struct
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), ReadEnum());
        }
        internal PointF ReadPoint()
        {
            return new PointF((float)ReadVdc(), (float)ReadVdc());
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
                return ReadInteger(Descriptor.VdcIntegerPrecision / 8, false);
            }
            else if (Descriptor.VdcType == VdcTypeSpecification.Real)
            {
                return ReadReal(Descriptor.VdcRealPrecision);
            }

            throw new NotSupportedException("The current VDC TYPE is not supported");
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
        internal PointF ReadViewportPoint()
        {
            double x = ReadViewportCoordinate();
            double y = ReadViewportCoordinate();
            return new PointF((float)x, (float)y);
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
            return ReadInteger(colorPrecision, true);
        }

        internal double ReadFixedPoint(int numBytes)
        {
            // ISO/IEC 8632-3 6.4
            // real value is computed as "whole + (fraction / 2**exp)"
            // exp is the width of the fraction value
            // the "whole part" has the same form as a Signed Integer
            int whole = ReadInteger(numBytes / 2, false);
            // the "fractional part" has the same form as an Unsigned Integer
            int fraction = ReadInteger(numBytes / 2, true);
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
                byte[] floatBytes = _reader.ReadBytes(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(floatBytes);
                return BitConverter.ToSingle(floatBytes, 0);
            }
            if (numBytes == 8)
            {
                byte[] doubleBytes = _reader.ReadBytes(8);
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

        private double ReadReal(RealPrecisionSpecification precision)
        {
            switch (precision)
            {
                case RealPrecisionSpecification.FixedPoint32Bit:
                    return ReadFixedPoint(4);
                case RealPrecisionSpecification.FixedPoint64Bit:
                    return ReadFixedPoint(8);
                case RealPrecisionSpecification.FloatingPoint32Bit:
                    return ReadFloatingPoint(4);
                case RealPrecisionSpecification.FloatingPoint64Bit:
                    return ReadFloatingPoint(8);
            }
            throw new NotSupportedException("The current Real Precision is not supported");
        }

        internal int ReadIndex()
        {
            // index is a signed integer at index precision [ISO/IEC 8632-3 7, Table 1, IX]
            return ReadInteger(Descriptor.IndexPrecision / 8, false);
        }
        internal int ReadName()
        {
            // name is a signed integer at name precision [ISO/IEC 8632-3 7, Table 1, N]
            return ReadInteger(Descriptor.NamePrecision / 8, false);
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
            // long string: length of 255 indicates that either one or two words follow
            bool isPartialString = false;
            if (length == 255)
            {
                length = ReadWord();
                // first bit indicates whether this is just a partial string and another one follows
                isPartialString = (length >> 16) == 1;
                length &= 0x7FFF;
            }

            byte[] characters = new byte[length];
            for (int i = 0; i < length; i++)
                characters[i] = ReadByte();

            string result;
            // try to detect certain common encodings (based on ISO/IEC 2022 / ECMA-35)
            // this only checks for DOCS (DECIDE OTHER CODING SYSTEM), identified by "ESC % / F" (0x1B 0x25 0x2F F)
            // and limited to F = "G" (0x47), "H" (0x48), "I" (0x49), "J" (0x4A), "K" (0x4B) and "L" (0x4C) at this point.
            // 0x47 until 0x49 are various levels of UTF-8, while 0x4A until 0x4C are various levels of UTF-16
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
            else
            {
                result = _currentEncoding.GetString(characters);
            }

            // TODO: verify this actually works like that; not sure if the string immediately follows...
            if (isPartialString)
                result += ReadString();

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null)
                    _reader.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
