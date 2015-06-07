namespace CgmInfo.Parameters
{
    public class RealPrecisionMetafileDescriptorParameter : MetafileDescriptorParameter
    {
        public RealPrecisionMetafileDescriptorParameter(int realFormat, int realExponent, int realFraction, MetafileDescriptorType type)
            : base(new int[] { realFormat, realExponent, realFraction }, type)
        {
        }
        public int RealFormat
        {
            get { return ((int[])Value)[0]; }
        }
        public int RealExponent
        {
            get { return ((int[])Value)[1]; }
        }
        public int RealFraction
        {
            get { return ((int[])Value)[2]; }
        }

        public override void Accept<T>(IMetafileDescriptorParameterVisitor<T> visitor, T parameter)
        {
            visitor.VisitRealPrecision(this, parameter);
        }
    }
}
