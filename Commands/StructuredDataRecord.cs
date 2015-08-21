using System.Collections.Generic;
using CgmInfo.BinaryEncoding;
using CgmInfo.Commands.Enums;

namespace CgmInfo.Commands
{
    public class StructuredDataRecord
    {
        public StructuredDataRecord(IEnumerable<StructuredDataElement> elements)
        {
            Elements = elements;
        }

        public IEnumerable<StructuredDataElement> Elements { get; private set; }

        public static StructuredDataRecord Read(MetafileReader reader)
        {
            // overall length seems to be encoded similar to the string length [ISO/IEC 8632-3 7, Table 1, Note 12]
            // (ie. one byte, followed by one word if its 255).
            int length = reader.ReadByte();
            if (length == 255)
            {
                // FIXME: does an SDR also have a long form similar to a string?
                length = reader.ReadWord();
            }

            var elements = new List<StructuredDataElement>();
            // require at least 2 bytes for the enum, and 2 bytes for the count
            // some files seem to include padding or similar, which throws this off by having an extra byte available at the end
            while (reader.HasMoreData(4))
            {
                DataTypeIndex type = (DataTypeIndex)reader.ReadEnum();
                int count = reader.ReadWord();
                object[] values = new object[count];
                for (int i = 0; i < count; i++)
                {
                    values[i] = ReadValue(reader, type);
                }
                elements.Add(new StructuredDataElement(type, values));
            }
            return new StructuredDataRecord(elements);
        }
        private static object ReadValue(MetafileReader reader, DataTypeIndex type)
        {
            switch (type)
            {
                case DataTypeIndex.StructuredDataRecord:
                    return Read(reader);
                case DataTypeIndex.ColorIndex:
                    return reader.ReadIndex();
                case DataTypeIndex.ColorDirect:
                    return reader.ReadColor();
                case DataTypeIndex.Name:
                    // TODO: ReadName?
                    return reader.ReadString();
                case DataTypeIndex.Enumerated:
                    return reader.ReadEnum();
                case DataTypeIndex.Integer:
                    return reader.ReadInteger();
                case DataTypeIndex.Reserved:
                    // TODO: what exactly does reserved mean in terms of advancing position?
                    return null;
                case DataTypeIndex.SignedInteger8bit:
                    return reader.ReadInteger(1);
                case DataTypeIndex.SignedInteger16bit:
                    return reader.ReadInteger(2);
                case DataTypeIndex.SignedInteger32bit:
                    return reader.ReadInteger(4);
                case DataTypeIndex.Index:
                    return reader.ReadIndex();
                case DataTypeIndex.Real:
                    return reader.ReadReal();
                case DataTypeIndex.String:
                case DataTypeIndex.StringFixed:
                    // TODO: difference between S and SF? charset/escape code handling? 
                    return reader.ReadString();
                case DataTypeIndex.ViewportCoordinate:
                    // FIXME: this depends on DEVICE VIEWPORT SPECIFICATION MODE,
                    //        lets simply hope this wont come up as frequently until its implemented...
                    return reader.ReadVdc();
                case DataTypeIndex.VDC:
                    return reader.ReadVdc();
                case DataTypeIndex.ColorComponent:
                    return reader.ReadColorValue();
                case DataTypeIndex.UnsignedInteger8bit:
                    return reader.ReadInteger(1);
                case DataTypeIndex.UnsignedInteger32Bit:
                    return reader.ReadInteger(4);
                case DataTypeIndex.UnsignedInteger16bit:
                    return reader.ReadInteger(2);
                case DataTypeIndex.BitStream:
                case DataTypeIndex.ColorList:
                default:
                    // FIXME: how are those implemented?
                    return null;
            }
        }
    }
}
