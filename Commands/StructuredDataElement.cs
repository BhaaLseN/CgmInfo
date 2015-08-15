using CgmInfo.Commands.Enums;

namespace CgmInfo.Commands
{
    public class StructuredDataElement
    {
        public StructuredDataElement(DataTypeIndex type, object[] values)
        {
            Type = type;
            Values = values;
        }

        public DataTypeIndex Type { get; private set; }
        public object[] Values { get; private set; }
    }
}
