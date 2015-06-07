using System;

namespace CgmInfo.Parameters
{
    public class IntegerMetafileDescriptorParameter : MetafileDescriptorParameter
    {
        public IntegerMetafileDescriptorParameter(int value, MetafileDescriptorType type)
            : base(value, type)
        {
        }

        public int ValueType
        {
            get { return (int)Value; }
        }

        public override void Accept<T>(IMetafileDescriptorParameterVisitor<T> visitor, T parameter)
        {
            visitor.VisitInteger(this, parameter);
        }
    }
}
