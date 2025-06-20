using System;
using System.IO;
using System.Text;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using BaseMetafileWriter = CgmInfo.MetafileWriter;

namespace CgmInfo.BinaryEncoding
{
    public class MetafileWriter : BaseMetafileWriter
    {
        // assume a default ISO 8859-1 [ISO/IEC 8632-1 6.3.4.5]
        private Encoding _currentEncoding = MetafileReader.GetDefaultEncoding();

        public MetafileWriter(string fileName)
            : base(fileName)
        {
        }
        private MetafileWriter(MetafileWriter parent, MemoryStream buffer)
            : base(parent, buffer)
        {
            _currentEncoding = parent._currentEncoding;
        }

        protected override void WriteInternal(Command command, Action<BaseMetafileWriter, Command> commandHandler)
        {
            using var ms = new MemoryStream();
            using var bufferWriter = new MetafileWriter(this, ms);
            commandHandler(bufferWriter, command);

            // commands are always word aligned [ISO/IEC 8632-3 5.4]
            EnsureWordAligned();

            byte[] buffer = ms.ToArray();
            const byte MaxShortParameterListLength = 0x1F;
            int parameterListLength = Math.Min(buffer.Length, MaxShortParameterListLength);
            WriteCommandHeader(command.ElementClass, command.ElementId, parameterListLength);

            // long-form commands write 0x1F to the regular field, then write everything in blocks preceded by a 15 bit length each
            // (where the 16th bit indicates whether another block follows) [ISO/IEC 8632-3 5.4, Figure 2]
            if (parameterListLength == MaxShortParameterListLength)
                WriteLongParameterList(buffer);
            else
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
        private void WriteLongParameterList(byte[] longParameterList)
        {
            const int MaxLongParameterListLength = 0x7FFF;
            int parameterListLength = longParameterList.Length;
            int sourceIndex = 0;
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

                byte[] writeBuffer = new byte[partitionLength];
                Array.Copy(longParameterList, sourceIndex, writeBuffer, 0, partitionLength);
                WriteBuffer(writeBuffer);
                sourceIndex += partitionLength;
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

            byte[] characters;
            if (needDifferentEncoding)
            {
                // switch to a different encodings (based on ISO/IEC 2022 / ECMA-35) - we chose UTF-8 because it likely fits everything.
                // we do this using DOCS (DECIDE OTHER CODING SYSTEM), identified by "ESC % / F" (0x1B 0x25 0x2F F)
                // with F = "I" (0x49), which indicates UTF-8 Level 3 support. This should be enough for everything.
                // [ISO-IR International Register of Coded Character Sets to be used with Escape Sequences, 2.8.2]
                // [ISO/IEC 8632-1 6.3.4.5, Example 1]
                _currentEncoding = Encoding.UTF8;
                characters = _currentEncoding.GetBytes(value);

                Array.Resize(ref characters, characters.Length + 4);
                Array.Copy(characters, 0, characters, 4, characters.Length - 4);

                characters[0] = 0x1B;
                characters[1] = 0x25;
                characters[2] = 0x2F;
                characters[3] = 0x49;
            }
            else
            {
                characters = _currentEncoding.GetBytes(value);
            }

            // long string: length of 255 indicates that either one or two words follow
            if (stringLength >= 255)
                WriteLongParameterList(characters);
            else
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
                    (data[i + 1], data[i]) = (data[i], data[i + 1]);
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
