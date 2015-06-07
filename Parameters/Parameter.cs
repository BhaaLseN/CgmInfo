namespace CgmInfo.Parameters
{
    public abstract class Parameter
    {
        public Parameter(object value)
        {
            Value = value;
        }
        public object Value { get; private set; }
    }
}
