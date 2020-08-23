using System.Collections.Generic;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

namespace CgmInfo.TextEncoding
{
    public class StructuredDataRecordReader
    {
        public StructuredDataRecord Read(MetafileReader reader)
        {
            var elements = new List<StructuredDataElement>();
            while (true)
            {
                var element = ReadElement(reader);
                if (element == null)
                    break;
                elements.Add(element);
            }

            return new StructuredDataRecord(elements);
        }
        private StructuredDataElement ReadElement(MetafileReader reader)
        {
            var type = ReadDataTypeIndex(reader);
            if ((int)type == -1)
                return null;

            int count = reader.ReadInteger();
            object[] values = new object[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = ReadValue(reader, type);
            }
            return new StructuredDataElement(type, values);
        }

        private object ReadValue(MetafileReader reader, DataTypeIndex type) => type switch
        {
            DataTypeIndex.StructuredDataRecord => ReadStructuredDataRecord(reader),
            DataTypeIndex.ColorIndex => ReadIndexedColor(reader),
            DataTypeIndex.ColorDirect => ReadDirectColor(reader),
            DataTypeIndex.Name => ReadName(reader),
            DataTypeIndex.Enumerated => ReadEnum(reader),
            DataTypeIndex.Integer => ReadInteger(reader),
            // TODO: what exactly does reserved mean in terms of advancing position?
            DataTypeIndex.Reserved => null,
            DataTypeIndex.SignedInteger8bit => ReadSigned8BitInteger(reader),
            DataTypeIndex.SignedInteger16bit => ReadSigned16BitInteger(reader),
            DataTypeIndex.SignedInteger32bit => ReadSigned32BitInteger(reader),
            DataTypeIndex.Index => ReadIndex(reader),
            DataTypeIndex.Real => ReadReal(reader),
            DataTypeIndex.String => ReadString(reader),
            DataTypeIndex.StringFixed => ReadStringFixed(reader),
            DataTypeIndex.ViewportCoordinate => ReadViewportCoordinate(reader),
            DataTypeIndex.VDC => ReadVDC(reader),
            DataTypeIndex.ColorComponent => ReadColorComponent(reader),
            DataTypeIndex.UnsignedInteger8bit => ReadUnsigned8BitInteger(reader),
            DataTypeIndex.UnsignedInteger32bit => ReadUnsigned32BitInteger(reader),
            DataTypeIndex.UnsignedInteger16bit => ReadUnsigned16BitInteger(reader),
            DataTypeIndex.BitStream => ReadBitStream(reader),
            // FIXME: how are those implemented?
            DataTypeIndex.ColorList => null,
            _ => null,
        };

        protected virtual StructuredDataRecord ReadStructuredDataRecord(MetafileReader reader) => reader.ReadStructuredDataRecord();

        protected virtual double ReadViewportCoordinate(MetafileReader reader) => reader.ReadViewportCoordinate();
        protected virtual double ReadVDC(MetafileReader reader) => reader.ReadVdc();

        protected virtual string ReadString(MetafileReader reader) => reader.ReadString();
        // TODO: difference between S and SF? charset/escape code handling? 
        protected virtual string ReadStringFixed(MetafileReader reader) => reader.ReadString();

        protected virtual double ReadReal(MetafileReader reader) => reader.ReadReal();
        protected virtual int ReadIndex(MetafileReader reader) => reader.ReadIndex();
        protected virtual int ReadInteger(MetafileReader reader) => reader.ReadInteger();

        protected virtual int ReadSigned8BitInteger(MetafileReader reader) => reader.ReadInteger();
        protected virtual int ReadSigned16BitInteger(MetafileReader reader) => reader.ReadInteger();
        protected virtual int ReadSigned32BitInteger(MetafileReader reader) => reader.ReadInteger();

        protected virtual int ReadUnsigned8BitInteger(MetafileReader reader) => reader.ReadInteger();
        protected virtual int ReadUnsigned16BitInteger(MetafileReader reader) => reader.ReadInteger();
        protected virtual int ReadUnsigned32BitInteger(MetafileReader reader) => reader.ReadInteger();

        protected virtual int ReadName(MetafileReader reader) => reader.ReadName();
        protected virtual string ReadEnum(MetafileReader reader) => reader.ReadEnum();

        protected virtual MetafileColor ReadDirectColor(MetafileReader reader) => reader.ReadDirectColor();
        protected virtual MetafileColor ReadIndexedColor(MetafileReader reader) => reader.ReadIndexedColor();
        protected virtual int ReadColorComponent(MetafileReader reader) => reader.ReadColorValue();

        // TODO: does the bitstream really go until the end of this full SDR?
        protected virtual byte[] ReadBitStream(MetafileReader reader) => reader.ReadBitstream();

        private DataTypeIndex ReadDataTypeIndex(MetafileReader reader)
        {
            string token = reader.ReadString();
            if (!int.TryParse(token, out int val))
                val = -1;
            return (DataTypeIndex)val;
        }
    }
}
