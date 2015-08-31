namespace CgmInfo
{
    public class MetafileProperties
    {
        public MetafileProperties(bool isBinaryEncoding)
        {
            IsBinaryEncoding = isBinaryEncoding;
        }

        public bool IsBinaryEncoding { get; private set; }
    }
}
