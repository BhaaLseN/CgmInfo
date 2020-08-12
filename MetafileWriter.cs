using System;
using System.IO;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

namespace CgmInfo
{
    public abstract class MetafileWriter : IDisposable
    {
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

        public abstract void Write(Command command);

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
