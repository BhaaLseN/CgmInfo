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
