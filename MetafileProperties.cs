namespace CgmInfo
{
    public class MetafileProperties
    {
        public MetafileProperties(bool isBinaryEncoding, long fileSize)
        {
            IsBinaryEncoding = isBinaryEncoding;
            FileSize = fileSize;
        }

        public bool IsBinaryEncoding { get; }
        public long FileSize { get; }
        public string Name { get; internal set; } = null!;
        public int Version { get; internal set; }
        public string? Profile { get; internal set; }
    }
}
