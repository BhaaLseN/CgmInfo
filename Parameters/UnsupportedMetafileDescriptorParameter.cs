namespace CgmInfo.Parameters
{
    public class UnsupportedMetafileDescriptorParameter : MetafileDescriptorParameter
    {
        public UnsupportedMetafileDescriptorParameter(byte[] rawContent, MetafileDescriptorType type)
            : base(rawContent, type)
        {
        }

        public override void Accept<T>(IMetafileDescriptorParameterVisitor<T> visitor, T parameter)
        {
            visitor.VisitUnsupported(this, parameter);
        }
    }
}
