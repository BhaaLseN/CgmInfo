using System;
using System.Collections.Generic;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

namespace CgmInfo.BinaryEncoding
{
    public class StructuredDataRecordReader
    {
        public StructuredDataRecord Read(MetafileReader reader)
        {
            var elements = new List<StructuredDataElement>();
            // require at least the number of bytes for the enum and the count; which depends on integer/index precision:
            // > The integer of the "data count" and the index of the "data type index" are represented respectively at the current
            // > Integer Precision and the current Index Precision of the metafile. [ISO/IEC 8632-1 H.2.2]
            // some files seem to include padding or similar, which throws this off by having an extra byte available at the end
            while (reader.HasMoreData((reader.Descriptor.IndexPrecision + reader.Descriptor.IntegerPrecision) / 8))
            {
                // enum is an index at the current index precision for SDR [ISO/IEC 8632-1 H.2.2]
                var type = (DataTypeIndex)Enum.ToObject(typeof(DataTypeIndex), reader.ReadIndex());
                // count is an interger at the current integer precision for SDR [ISO/IEC 8632-1 H.2.2]
                int count = reader.ReadInteger();
                object[] values = new object[count];
                for (int i = 0; i < count; i++)
                {
                    values[i] = ReadValue(reader, type);
                }
                elements.Add(new StructuredDataElement(type, values));
            }
            return new StructuredDataRecord(elements);
        }
        private object ReadValue(MetafileReader reader, DataTypeIndex type)
        {
            switch (type)
            {
                case DataTypeIndex.StructuredDataRecord:
                    return ReadStructuredDataRecord(reader);
                case DataTypeIndex.ColorIndex:
                    return ReadIndexedColor(reader);
                case DataTypeIndex.ColorDirect:
                    return ReadDirectColor(reader);
                case DataTypeIndex.Name:
                    return ReadName(reader);
                case DataTypeIndex.Enumerated:
                    return ReadEnum(reader);
                case DataTypeIndex.Integer:
                    return ReadInteger(reader);
                case DataTypeIndex.Reserved:
                    // TODO: what exactly does reserved mean in terms of advancing position?
                    return null;
                case DataTypeIndex.SignedInteger8bit:
                    return ReadSigned8BitInteger(reader);
                case DataTypeIndex.SignedInteger16bit:
                    return ReadSigned16BitInteger(reader);
                case DataTypeIndex.SignedInteger32bit:
                    return ReadSigned32BitInteger(reader);
                case DataTypeIndex.Index:
                    return ReadIndex(reader);
                case DataTypeIndex.Real:
                    return ReadReal(reader);
                case DataTypeIndex.String:
                    return ReadString(reader);
                case DataTypeIndex.StringFixed:
                    return ReadStringFixed(reader);
                case DataTypeIndex.ViewportCoordinate:
                    return ReadViewportCoordinate(reader);
                case DataTypeIndex.VDC:
                    return ReadVDC(reader);
                case DataTypeIndex.ColorComponent:
                    return ReadColorComponent(reader);
                case DataTypeIndex.UnsignedInteger8bit:
                    return ReadUnsigned8BitInteger(reader);
                case DataTypeIndex.UnsignedInteger32bit:
                    return ReadUnsigned32BitInteger(reader);
                case DataTypeIndex.UnsignedInteger16bit:
                    return ReadUnsigned16BitInteger(reader);
                case DataTypeIndex.BitStream:
                    return ReadBitStream(reader);
                case DataTypeIndex.ColorList:
                default:
                    // FIXME: how are those implemented?
                    return null;
            }
        }

        protected virtual StructuredDataRecord ReadStructuredDataRecord(MetafileReader reader) => reader.ReadStructuredDataRecord();

        protected virtual double ReadViewportCoordinate(MetafileReader reader) => reader.ReadViewportCoordinate();
        protected virtual double ReadVDC(MetafileReader reader) => reader.ReadVdc();

        protected virtual string ReadString(MetafileReader reader) => reader.ReadString();
        // TODO: difference between S and SF? charset/escape code handling? 
        protected virtual string ReadStringFixed(MetafileReader reader) => reader.ReadString();

        protected virtual double ReadReal(MetafileReader reader) => reader.ReadReal();
        protected virtual int ReadIndex(MetafileReader reader) => reader.ReadIndex();
        protected virtual int ReadInteger(MetafileReader reader) => reader.ReadInteger();

        protected virtual int ReadSigned8BitInteger(MetafileReader reader) => reader.ReadInteger(1, false);
        protected virtual int ReadSigned16BitInteger(MetafileReader reader) => reader.ReadInteger(2, false);
        protected virtual int ReadSigned32BitInteger(MetafileReader reader) => reader.ReadInteger(4, false);

        protected virtual int ReadUnsigned8BitInteger(MetafileReader reader) => reader.ReadInteger(1, true);
        protected virtual int ReadUnsigned16BitInteger(MetafileReader reader) => reader.ReadInteger(2, true);
        protected virtual int ReadUnsigned32BitInteger(MetafileReader reader) => reader.ReadInteger(4, true);

        protected virtual int ReadName(MetafileReader reader) => reader.ReadName();
        protected virtual int ReadEnum(MetafileReader reader) => reader.ReadEnum();

        protected virtual MetafileColor ReadDirectColor(MetafileReader reader) => reader.ReadDirectColor();
        protected virtual MetafileColor ReadIndexedColor(MetafileReader reader) => reader.ReadIndexedColor();
        protected virtual int ReadColorComponent(MetafileReader reader) => reader.ReadColorValue();

        // TODO: does the bitstream really go until the end of this full SDR?
        protected virtual byte[] ReadBitStream(MetafileReader reader) => reader.ReadBitstream();
    }
}
