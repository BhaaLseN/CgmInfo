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
        private static readonly Dictionary<(int ElementClass, int ElementId), Action<MetafileWriter, Command>> _commandTable = new Dictionary<(int, int), Action<MetafileWriter, Command>>
        {
            // delimiter elements [ISO/IEC 8632-3 8.2, Table 3]
            { (0, 0), DelimiterElementWriter.Noop },
            { (0, 1), DelimiterElementWriter.BeginMetafile },
            { (0, 2), DelimiterElementWriter.EndMetafile },
            { (0, 3), DelimiterElementWriter.BeginPicture },
            { (0, 4), DelimiterElementWriter.BeginPictureBody },
            { (0, 5), DelimiterElementWriter.EndPicture },
            { (0, 6), DelimiterElementWriter.BeginSegment },
            { (0, 7), DelimiterElementWriter.EndSegment },
            { (0, 8), DelimiterElementWriter.BeginFigure },
            { (0, 9), DelimiterElementWriter.EndFigure },
            // entries 10, 11 and 12 do not exist in ISO/IEC 8632-3
            { (0, 13), DelimiterElementWriter.BeginProtectionRegion },
            { (0, 14), DelimiterElementWriter.EndProtectionRegion },
            { (0, 15), DelimiterElementWriter.BeginCompoundLine },
            { (0, 16), DelimiterElementWriter.EndCompoundLine },
            { (0, 17), DelimiterElementWriter.BeginCompoundTextPath },
            { (0, 18), DelimiterElementWriter.EndCompoundTextPath },
            { (0, 19), DelimiterElementWriter.BeginTileArray },
            { (0, 20), DelimiterElementWriter.EndTileArray },
            { (0, 21), DelimiterElementWriter.BeginApplicationStructure },
            { (0, 22), DelimiterElementWriter.BeginApplicationStructureBody },
            { (0, 23), DelimiterElementWriter.EndApplicationStructure },

            // metafile descriptor elements [ISO/IEC 8632-3 8.3, Table 4]
            { (1, 1), MetafileDescriptorWriter.MetafileVersion },
            { (1, 2), MetafileDescriptorWriter.MetafileDescription },
            { (1, 3), MetafileDescriptorWriter.VdcType },
            { (1, 4), MetafileDescriptorWriter.IntegerPrecision },
            { (1, 5), MetafileDescriptorWriter.RealPrecision },
            { (1, 6), MetafileDescriptorWriter.IndexPrecision },
            { (1, 7), MetafileDescriptorWriter.ColorPrecision },
            { (1, 8), MetafileDescriptorWriter.ColorIndexPrecision },
            { (1, 9), MetafileDescriptorWriter.MaximumColorIndex },
            { (1, 10), MetafileDescriptorWriter.ColorValueExtent },
            { (1, 11), MetafileDescriptorWriter.MetafileElementsList },
            //{ (1, 12), MetafileDescriptorWriter.MetafileDefaultsReplacement },
            { (1, 13), MetafileDescriptorWriter.FontList },
            { (1, 14), MetafileDescriptorWriter.CharacterSetList },
            { (1, 15), MetafileDescriptorWriter.CharacterCodingAnnouncer },
            { (1, 16), MetafileDescriptorWriter.NamePrecision },
            { (1, 17), MetafileDescriptorWriter.MaximumVdcExtent },
            { (1, 18), MetafileDescriptorWriter.SegmentPriorityExtent },
            { (1, 19), MetafileDescriptorWriter.ColorModel },
            //{ (1, 20), MetafileDescriptorWriter.ColorCalibration },
            { (1, 21), MetafileDescriptorWriter.FontProperties },
            //{ (1, 22), MetafileDescriptorWriter.GlyphMapping },
            //{ (1, 23), MetafileDescriptorWriter.SymbolLibraryList },
            //{ (1, 24), MetafileDescriptorWriter.PictureDirectory },

            // picture descriptor elements [ISO/IEC 8632-3 8.4, Table 5]
            { (2, 1), PictureDescriptorWriter.ScalingMode },
            { (2, 2), PictureDescriptorWriter.ColorSelectionMode },
            { (2, 3), PictureDescriptorWriter.LineWidthSpecificationMode },
            { (2, 4), PictureDescriptorWriter.MarkerSizeSpecificationMode },
            { (2, 5), PictureDescriptorWriter.EdgeWidthSpecificationMode },
            { (2, 6), PictureDescriptorWriter.VdcExtent },
            { (2, 7), PictureDescriptorWriter.BackgroundColor },
            { (2, 8), PictureDescriptorWriter.DeviceViewport },
            { (2, 9), PictureDescriptorWriter.DeviceViewportSpecificationMode },
            //{ (2, 10), PictureDescriptorWriter.DeviceViewportMapping },
            //{ (2, 11), PictureDescriptorWriter.LineRepresentation },
            //{ (2, 12), PictureDescriptorWriter.MarkerRepresentation },
            //{ (2, 13), PictureDescriptorWriter.TextRepresentation },
            //{ (2, 14), PictureDescriptorWriter.FillRepresentation },
            //{ (2, 15), PictureDescriptorWriter.EdgeRepresentation },
            { (2, 16), PictureDescriptorWriter.InteriorStyleSpecificationMode },
            { (2, 17), PictureDescriptorWriter.LineAndEdgeTypeDefinition },
            { (2, 18), PictureDescriptorWriter.HatchStyleDefinition },
            { (2, 19), PictureDescriptorWriter.GeometricPatternDefinition },
            //{ (2, 20), PictureDescriptorWriter.ApplicationStructureDirectory },

            // control elements [ISO/IEC 8632-3 8.5, Table 6]
            { (3, 1), ControlElementWriter.VdcIntegerPrecision },
            { (3, 2), ControlElementWriter.VdcRealPrecision },
            { (3, 3), ControlElementWriter.AuxiliaryColor },
            { (3, 4), ControlElementWriter.Transparency },
            { (3, 5), ControlElementWriter.ClipRectangle },
            { (3, 6), ControlElementWriter.ClipIndicator },
            { (3, 7), ControlElementWriter.LineClippingMode },
            { (3, 8), ControlElementWriter.MarkerClippingMode },
            { (3, 9), ControlElementWriter.EdgeClippingMode },
            { (3, 10), ControlElementWriter.NewRegion },
            { (3, 11), ControlElementWriter.SavePrimitiveContext },
            { (3, 12), ControlElementWriter.RestorePrimitiveContext },
            // entries 13 until 16 do not exist in ISO/IEC 8632-3
            { (3, 17), ControlElementWriter.ProtectionRegionIndicator },
            { (3, 18), ControlElementWriter.GeneralizedTextPathMode },
            { (3, 19), ControlElementWriter.MiterLimit },
            //{ (3, 20), ControlElementWriter.TransparentCellColor },

            // graphical primitive elements [ISO/IEC 8632-3 8.6, Table 7]
            { (4, 1), GraphicalPrimitiveWriter.Polyline },
            { (4, 2), GraphicalPrimitiveWriter.DisjointPolyline },
            { (4, 3), GraphicalPrimitiveWriter.Polymarker },
            { (4, 4), GraphicalPrimitiveWriter.Text },
            { (4, 5), GraphicalPrimitiveWriter.RestrictedText },
            { (4, 6), GraphicalPrimitiveWriter.AppendText },
            { (4, 7), GraphicalPrimitiveWriter.Polygon },
            { (4, 8), GraphicalPrimitiveWriter.PolygonSet },
            { (4, 9), GraphicalPrimitiveWriter.CellArray },
            //{ (4, 10), GraphicalPrimitiveWriter.GeneralizedDrawingPrimitive },
            { (4, 11), GraphicalPrimitiveWriter.Rectangle },
            { (4, 12), GraphicalPrimitiveWriter.Circle },
            { (4, 13), GraphicalPrimitiveWriter.CircularArc3Point },
            { (4, 14), GraphicalPrimitiveWriter.CircularArc3PointClose },
            { (4, 15), GraphicalPrimitiveWriter.CircularArcCenter },
            { (4, 16), GraphicalPrimitiveWriter.CircularArcCenterClose },
            { (4, 17), GraphicalPrimitiveWriter.Ellipse },
            { (4, 18), GraphicalPrimitiveWriter.EllipticalArc },
            { (4, 19), GraphicalPrimitiveWriter.EllipticalArcClose },
            { (4, 20), GraphicalPrimitiveWriter.CircularArcCenterReversed },
            { (4, 21), GraphicalPrimitiveWriter.ConnectingEdge },
            { (4, 22), GraphicalPrimitiveWriter.HyperbolicArc },
            { (4, 23), GraphicalPrimitiveWriter.ParabolicArc },
            { (4, 24), GraphicalPrimitiveWriter.NonUniformBSpline },
            { (4, 25), GraphicalPrimitiveWriter.NonUniformRationalBSpline },
            { (4, 26), GraphicalPrimitiveWriter.Polybezier },
            //{ (4, 27), GraphicalPrimitiveWriter.Polysymbol },
            { (4, 28), GraphicalPrimitiveWriter.BitonalTile },
            { (4, 29), GraphicalPrimitiveWriter.Tile },

            // attribute elements [ISO/IEC 8632-3 8.7, Table 8]
            { (5, 1), AttributeWriter.LineBundleIndex },
            { (5, 2), AttributeWriter.LineType },
            { (5, 3), AttributeWriter.LineWidth },
            { (5, 4), AttributeWriter.LineColor },
            { (5, 5), AttributeWriter.MarkerBundleIndex },
            { (5, 6), AttributeWriter.MarkerType },
            { (5, 7), AttributeWriter.MarkerSize },
            { (5, 8), AttributeWriter.MarkerColor },
            { (5, 9), AttributeWriter.TextBundleIndex },
            { (5, 10), AttributeWriter.TextFontIndex },
            { (5, 11), AttributeWriter.TextPrecision },
            { (5, 12), AttributeWriter.CharacterExpansionFactor },
            { (5, 13), AttributeWriter.CharacterSpacing },
            { (5, 14), AttributeWriter.TextColor },
            { (5, 15), AttributeWriter.CharacterHeight },
            { (5, 16), AttributeWriter.CharacterOrientation },
            { (5, 17), AttributeWriter.TextPath },
            { (5, 18), AttributeWriter.TextAlignment },
            { (5, 19), AttributeWriter.CharacterSetIndex },
            { (5, 20), AttributeWriter.AlternateCharacterSetIndex },
            { (5, 21), AttributeWriter.FillBundleIndex },
            { (5, 22), AttributeWriter.InteriorStyle },
            { (5, 23), AttributeWriter.FillColor },
            { (5, 24), AttributeWriter.HatchIndex },
            { (5, 25), AttributeWriter.PatternIndex },
            { (5, 26), AttributeWriter.EdgeBundleIndex },
            { (5, 27), AttributeWriter.EdgeType },
            { (5, 28), AttributeWriter.EdgeWidth },
            { (5, 29), AttributeWriter.EdgeColor },
            { (5, 30), AttributeWriter.EdgeVisibility },
            { (5, 31), AttributeWriter.FillReferencePoint },
            { (5, 32), AttributeWriter.PatternTable },
            { (5, 33), AttributeWriter.PatternSize },
            { (5, 34), AttributeWriter.ColorTable },
            { (5, 35), AttributeWriter.AspectSourceFlags },
            { (5, 36), AttributeWriter.PickIdentifier },
            { (5, 37), AttributeWriter.LineCap },
            { (5, 38), AttributeWriter.LineJoin },
            { (5, 39), AttributeWriter.LineTypeContinuation },
            { (5, 40), AttributeWriter.LineTypeInitialOffset },
            //{ (5, 41), AttributeWriter.TextScoreType },
            { (5, 42), AttributeWriter.RestrictedTextType },
            { (5, 43), AttributeWriter.InterpolatedInterior },
            { (5, 44), AttributeWriter.EdgeCap },
            { (5, 45), AttributeWriter.EdgeJoin },
            { (5, 46), AttributeWriter.EdgeTypeContinuation },
            { (5, 47), AttributeWriter.EdgeTypeInitialOffset },
            //{ (5, 48), AttributeWriter.SymbolLibraryIndex },
            //{ (5, 49), AttributeWriter.SymbolColor },
            //{ (5, 50), AttributeWriter.SymbolSize },
            //{ (5, 51), AttributeWriter.SymbolOrientation },

            // escape elements [ISO/IEC 8632-3 8.8, Table 9]
            { (6, 1), EscapeWriter.Escape },

            // external elements [ISO/IEC 8632-3 8.9, Table 10]
            { (7, 1), ExternalWriter.Message },
            { (7, 2), ExternalWriter.ApplicationData },

            // segment control/segment attribute elements [ISO/IEC 8632-3 8.10, Table 11]
            { (8, 1), SegmentWriter.CopySegment },
            { (8, 2), SegmentWriter.InheritanceFilter },
            { (8, 3), SegmentWriter.ClipInheritance },
            { (8, 4), SegmentWriter.SegmentTransformation },
            { (8, 5), SegmentWriter.SegmentHighlighting },
            { (8, 6), SegmentWriter.SegmentDisplayPriority },
            { (8, 7), SegmentWriter.SegmentPickPriority },

            // application structure descriptor elements [ISO/IEC 8632-3 8.11, Table 12]
            { (9, 1), ApplicationStructureDescriptorWriter.ApplicationStructureAttribute },
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

            if (!_commandTable.TryGetValue((command.ElementClass, command.ElementId), out var commandHandler) || commandHandler == null)
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
