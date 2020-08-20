using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;
using CgmInfo.Writers;

namespace CgmInfo
{
    public abstract class MetafileWriter : IDisposable
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
            // segment control/segment attribute elements [ISO/IEC 8632-3 8.10, Table 11]
            { 8, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, SegmentWriter.CopySegment },
                    { 2, SegmentWriter.InheritanceFilter },
                    { 3, SegmentWriter.ClipInheritance },
                    { 4, SegmentWriter.SegmentTransformation },
                    { 5, SegmentWriter.SegmentHighlighting },
                    { 6, SegmentWriter.SegmentDisplayPriority },
                    { 7, SegmentWriter.SegmentPickPriority },
                }
            },
            // application structure descriptor elements [ISO/IEC 8632-3 8.11, Table 12]
            { 9, new Dictionary<int, Action<MetafileWriter, Command>>
                {
                    { 1, ApplicationStructureDescriptorWriter.ApplicationStructureAttribute },
                }
            },
        };

        protected bool _insideMetafile;

        protected readonly Stream _stream;

        public MetafileDescriptor Descriptor { get; } = new MetafileDescriptor();

        protected MetafileWriter(string fileName)
        {
            _stream = File.Open(fileName, FileMode.Create);
        }
        protected MetafileWriter(MetafileWriter other, Stream stream)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            _stream = stream;
            Descriptor = other.Descriptor;
        }

        public void Write(Command command)
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

            WriteInternal(command, commandHandler);
        }

        protected abstract void WriteInternal(Command command, Action<MetafileWriter, Command> commandHandler);

        private bool _isDisposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _stream?.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal abstract void WriteString(string value);
        internal virtual void WriteInteger(int value) => WriteInteger(value, Descriptor.IntegerPrecision);
        internal abstract void WriteInteger(int value, int integerPrecision);
        internal abstract void WriteWord(ushort value);
        internal abstract void WriteByte(byte value);
        internal virtual void WriteReal(double value)
        {
            WriteReal(value, Descriptor.RealPrecision);
        }
        internal virtual void WriteReal(double value, RealPrecisionSpecification precision)
        {
            switch (precision)
            {
                case RealPrecisionSpecification.FixedPoint32Bit:
                    WriteFixedPoint(value, precision: 32);
                    return;
                case RealPrecisionSpecification.FixedPoint64Bit:
                    WriteFixedPoint(value, precision: 64);
                    return;
                case RealPrecisionSpecification.FloatingPoint32Bit:
                    WriteFloatingPoint(value, precision: 32);
                    return;
                case RealPrecisionSpecification.FloatingPoint64Bit:
                    WriteFloatingPoint(value, precision: 64);
                    return;
            }
            throw new NotSupportedException("The current Real Precision is not supported");
        }
        internal abstract void WriteFixedPoint(double value, int precision);
        internal abstract void WriteFloatingPoint(double value, int precision);
        internal abstract void WriteVdc(double value);
        internal virtual void WriteSizeSpecification(double value, WidthSpecificationModeType widthSpecificationMode)
        {
            // When the value is 'absolute', then an associated parameter of type SS
            // resolves to the basic data type VDC. Otherwise, associated
            // SS parameters resolve to the basic data type R. [ISO/IEC 8632-1 7.1, Table 11]
            if (widthSpecificationMode == WidthSpecificationModeType.Absolute)
                WriteVdc(value);
            else
                WriteReal(value);
        }
        internal abstract void WriteName(int value);
        internal virtual void WriteIndex(int value) => WriteIndex(value, Descriptor.IndexPrecision);
        internal abstract void WriteIndex(int value, int indexPrecision);
        internal virtual void WriteColorIndex(int value) => WriteColorIndex(value, Descriptor.ColorIndexPrecision);
        internal abstract void WriteColorIndex(int value, int indexPrecision);
        internal virtual void WriteColor(MetafileColor value)
        {
            if (Descriptor.ColorSelectionMode == ColorModeType.Direct)
                WriteDirectColor(value);
            else
                WriteIndexedColor(value);
        }
        internal virtual void WriteColor(MetafileColor value, int colorPrecision)
        {
            if (Descriptor.ColorSelectionMode == ColorModeType.Direct)
                WriteDirectColor(value, colorPrecision);
            else
                WriteIndexedColor(value, colorPrecision);
        }
        internal virtual void WriteDirectColor(MetafileColor value) => WriteDirectColor(value, Descriptor.ColorPrecision);
        // NOTE: this assumes we use the correct subclass of MetafileColor that matches the given color model
        internal virtual void WriteDirectColor(MetafileColor value, int colorDirectPrecision)
        {
            if (value is MetafileColorIndexed colorIndexed)
            {
                WriteColorIndex(colorIndexed.Index);
                return;
            }

            if (Descriptor.ColorModel == ColorModel.RGB)
            {
                var colorRGB = (MetafileColorRGB)value;
                WriteColorValue(colorRGB.Red, colorDirectPrecision);
                WriteColorValue(colorRGB.Green, colorDirectPrecision);
                WriteColorValue(colorRGB.Blue, colorDirectPrecision);
            }
            else if (Descriptor.ColorModel == ColorModel.CMYK)
            {
                var colorCMYK = (MetafileColorCMYK)value;
                WriteColorValue(colorCMYK.Cyan, colorDirectPrecision);
                WriteColorValue(colorCMYK.Magenta, colorDirectPrecision);
                WriteColorValue(colorCMYK.Yellow, colorDirectPrecision);
                WriteColorValue(colorCMYK.Black, colorDirectPrecision);
            }
            else // assume CIE*
            {
                var colorCIE = (MetafileColorCIE)value;
                WriteReal(colorCIE.Component1);
                WriteReal(colorCIE.Component2);
                WriteReal(colorCIE.Component3);
            }
        }
        internal virtual void WriteIndexedColor(MetafileColor value) => WriteIndexedColor(value, Descriptor.ColorIndexPrecision);
        internal virtual void WriteIndexedColor(MetafileColor value, int colorIndexPrecision)
        {
            int index = Descriptor.GetColorIndex(value);
            WriteIndex(index, colorIndexPrecision);
        }

        internal abstract void EnsureWordAligned();
        internal virtual void WriteColorValue(int value) => WriteColorValue(value, Descriptor.ColorPrecision);
        internal abstract void WriteColorValue(int value, int colorPrecision);
        internal virtual void WritePoint(MetafilePoint value)
        {
            WriteVdc(value.X);
            WriteVdc(value.Y);
        }
        internal abstract void WriteEnum<TEnum>(TEnum value) where TEnum : Enum;
        internal abstract void WriteBitStream(byte[] value);
        internal virtual void WriteViewportPoint(MetafilePoint point)
        {
            WriteViewportCoordinate(point.X);
            WriteViewportCoordinate(point.Y);
        }
        internal virtual void WriteViewportCoordinate(double value)
        {
            // a Viewport Coordinate (VC) is either an int or a double; depending on what DEVICE VIEWPORT SPECIFICATION MODE said [ISO/IEC 8632-3 7, Table 1, Note 13/14]
            if (Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.MillimetersWithScaleFactor ||
                Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.PhysicalDeviceCoordinates)
            {
                WriteInteger((int)value);
                return;
            }
            else if (Descriptor.DeviceViewportSpecificationMode == DeviceViewportSpecificationModeType.FractionOfDrawingSurface)
            {
                WriteReal(value);
                return;
            }

            throw new NotSupportedException("The current DEVICE VIEWPORT SPECIFICATION MODE is not supported");
        }

        internal virtual void WriteStructuredDataRecord(StructuredDataRecord value)
        {
            if (value == null)
                return;

            foreach (var element in value.Elements)
                WriteStructuredDataElement(element);
        }
        internal virtual void WriteStructuredDataElement(StructuredDataElement value)
        {
            if (value == null)
                return;

            WriteEnum(value.Type);
            WriteInteger(value.Values.Length);
            switch (value.Type)
            {
                case DataTypeIndex.StructuredDataRecord:
                    writeAll<StructuredDataRecord>(WriteStructuredDataRecord);
                    break;
                case DataTypeIndex.ColorIndex:
                    writeAll<int>(WriteColorIndex);
                    break;
                case DataTypeIndex.ColorDirect:
                    writeAll<MetafileColor>(WriteDirectColor);
                    break;
                case DataTypeIndex.Name:
                    writeAll<int>(WriteName);
                    break;
                case DataTypeIndex.Enumerated:
                    writeAll<Enum>(WriteEnum);
                    break;
                case DataTypeIndex.Integer:
                    writeAll<int>(WriteInteger);
                    break;
                case DataTypeIndex.Reserved:
                    // FIXME: do nothing?
                    break;
                case DataTypeIndex.SignedInteger8bit:
                case DataTypeIndex.UnsignedInteger8bit:
                    writeAll<int>(i => WriteInteger(i, 8));
                    break;
                case DataTypeIndex.SignedInteger16bit:
                case DataTypeIndex.UnsignedInteger16bit:
                    writeAll<int>(i => WriteInteger(i, 16));
                    break;
                case DataTypeIndex.SignedInteger32bit:
                case DataTypeIndex.UnsignedInteger32bit:
                    writeAll<int>(i => WriteInteger(i, 32));
                    break;
                case DataTypeIndex.Index:
                    writeAll<int>(WriteIndex);
                    break;
                case DataTypeIndex.Real:
                    writeAll<double>(WriteReal);
                    break;
                case DataTypeIndex.String:
                case DataTypeIndex.StringFixed:
                    // TODO: difference between S and SF? charset/escape code handling?
                    writeAll<string>(WriteString);
                    break;
                case DataTypeIndex.ViewportCoordinate:
                    writeAll<double>(WriteViewportCoordinate);
                    break;
                case DataTypeIndex.VDC:
                    writeAll<double>(WriteVdc);
                    break;
                case DataTypeIndex.ColorComponent:
                    writeAll<int>(WriteColorValue);
                    break;
                case DataTypeIndex.BitStream:
                    writeAll<byte[]>(WriteBitStream);
                    break;
                case DataTypeIndex.ColorList:
                default:
                    throw new NotSupportedException($"Cannot write SDR elements of type '{value.Type}'.");
            }

            void writeAll<T>(Action<T> writeAction)
            {
                foreach (var value in value.Values.OfType<T>())
                    writeAction(value);
            }
        }
    }
}
