using System.Collections.Generic;
using System.Globalization;
using System.Text;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Enums;

namespace CgmInfo.TextEncoding
{
    internal class ApplicationStructureDescriptorReader
    {
        public static ApplicationStructureAttribute ApplicationStructureAttribute(MetafileReader reader)
        {
            string attributeType = reader.ReadString();
            string sdr = reader.ReadString();
            return new ApplicationStructureAttribute(attributeType, ParseStructuredDataRecord(sdr));
        }

        private readonly string _sdr;
        private int _position;
        private ApplicationStructureDescriptorReader(string sdr)
        {
            _sdr = sdr;
        }
        public static StructuredDataRecord ParseStructuredDataRecord(string sdr)
        {
            var elements = new List<StructuredDataElement>();
            var reader = new ApplicationStructureDescriptorReader(sdr);
            while (true)
            {
                var element = reader.ReadElement();
                if (element == null)
                    break;
                elements.Add(element);
            }

            return new StructuredDataRecord(elements);
        }
        public StructuredDataElement ReadElement()
        {
            var type = ReadDataTypeIndex();
            if ((int)type == -1)
                return null;

            int count = ReadInteger();
            object[] values = new object[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = ReadValue(type);
            }
            return new StructuredDataElement(type, values);
        }

        private object ReadValue(DataTypeIndex type)
        {
            switch (type)
            {
                case DataTypeIndex.Name:
                case DataTypeIndex.String:
                case DataTypeIndex.StringFixed:
                    return ReadToken();

                case DataTypeIndex.ColorIndex:
                case DataTypeIndex.Integer:
                case DataTypeIndex.SignedInteger8bit:
                case DataTypeIndex.SignedInteger16bit:
                case DataTypeIndex.SignedInteger32bit:
                case DataTypeIndex.Index:
                case DataTypeIndex.UnsignedInteger8bit:
                case DataTypeIndex.UnsignedInteger16bit:
                case DataTypeIndex.UnsignedInteger32Bit:
                    return ReadInteger();

                case DataTypeIndex.Real:
                case DataTypeIndex.VDC:
                    return ReadReal();

                case DataTypeIndex.Reserved:
                    // TODO: what exactly does reserved mean in terms of advancing position?
                    return null;

                case DataTypeIndex.StructuredDataRecord:
                case DataTypeIndex.ColorDirect:
                case DataTypeIndex.Enumerated:
                case DataTypeIndex.ViewportCoordinate:
                case DataTypeIndex.ColorComponent:
                case DataTypeIndex.BitStream:
                case DataTypeIndex.ColorList:
                default:
                    // FIXME: implement the other types; at least as soon as they appear somewhere.
                    //        they seem to be rare enough to get away with this...
                    return _sdr;
            }
        }
        private string ReadToken()
        {
            var sb = new StringBuilder();
            while (_position < _sdr.Length)
            {
                char c = _sdr[_position++];
                switch (c)
                {
                    // whitespace; skip them unless there is content (since it also counts as a soft seperator) [ISO/IEC 8632-4 6.2.2]
                    case ' ':
                    case '\r':
                    case '\n':
                    case '\t':
                    // point values might be enclosed in parentheses for readability [ISO/IEC 8632-4 6.3.5]
                    // TODO: spec says there must be exactly two numbers inside the parentheses, or the file is non-conforming.
                    //       this isn't validated at this point, and would require rewriting ReadPoint (and possibly others).
                    // instead, we treat them as soft separators (ie.: return content if there is any, but keep parsing if there is not)
                    case '(':
                    case ')':
                        if (sb.Length > 0)
                            return sb.ToString();
                        break;

                    // comma counts as a hard separator [ISO/IEC 8632-4 6.2.2]
                    case ',':
                        return sb.ToString();

                    // strings; fully read and return them as they are [ISO/IEC 8632-4 6.3.3]
                    case '\'':
                    case '"':
                        char stringDelimiter = c;
                        do
                        {
                            c = _sdr[_position++];

                            // in case the delimiter appears:
                            // either end of string, or double the delimiter to include a literal one
                            if (c == stringDelimiter)
                            {
                                if (_position >= _sdr.Length)
                                    return sb.ToString();

                                char nextChar = _sdr[_position];
                                if (nextChar == stringDelimiter)
                                {
                                    // literal delimiter; append it once, then reset the character to keep the loop going
                                    sb.Append(c);
                                    c = '\x00';
                                }
                                else
                                {
                                    // end of string
                                    break;
                                }
                            }
                            else
                            {
                                // any other character: simply append to the token string
                                sb.Append(c);
                            }

                            if (_position >= _sdr.Length)
                                return sb.ToString();
                        } while (c != stringDelimiter);

                        return sb.ToString();

                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
        private int ReadInteger()
        {
            string token = ReadToken();
            if (!int.TryParse(token, out int ret))
                ret = -1;
            return ret;
        }
        private double ReadReal()
        {
            string token = ReadToken();
            if (!double.TryParse(token, NumberStyles.Any, TextEncodingHelper.Culture, out double ret))
                ret = -1;
            return ret;
        }
        private DataTypeIndex ReadDataTypeIndex()
        {
            return (DataTypeIndex)ReadInteger();
        }
    }
}
