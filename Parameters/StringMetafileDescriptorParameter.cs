namespace CgmInfo.Parameters
{
    public class StringMetafileDescriptorParameter : MetafileDescriptorParameter
    {
        public StringMetafileDescriptorParameter(string str, MetafileDescriptorType type)
            : base(str, type)
        {
        }

        public string ValueType
        {
            get { return (string)Value; }
        }

        public override void Accept<T>(IMetafileDescriptorParameterVisitor<T> visitor, T parameter)
        {
            visitor.VisitString(this, parameter);
        }
    }
}
