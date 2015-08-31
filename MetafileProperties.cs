namespace CgmInfo
{
    public class MetafileProperties
    {
        public MetafileProperties(bool isBinaryEncoding, long fileSize)
        {
            IsBinaryEncoding = isBinaryEncoding;
            FileSize = fileSize;
        }

        public bool IsBinaryEncoding { get; private set; }
        public long FileSize { get; private set; }
        public string Name { get; internal set; }
    }
}
