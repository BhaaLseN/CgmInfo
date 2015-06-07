namespace CgmInfo.Parameters
{
    public interface IMetafileDescriptorParameterVisitor<T>
    {
        void VisitInteger(IntegerMetafileDescriptorParameter integerParameter, T parameter);
        void VisitString(StringMetafileDescriptorParameter stringParameter, T parameter);
        void VisitRealPrecision(RealPrecisionMetafileDescriptorParameter realPrecisionParameter, T parameter);
        void VisitUnsupported(UnsupportedMetafileDescriptorParameter unsupportedParameter, T parameter);
    }
}
