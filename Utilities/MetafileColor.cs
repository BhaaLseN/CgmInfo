namespace CgmInfo.Utilities
{
    public abstract class MetafileColor
    {
        public abstract ARGB ToARGB();
        protected abstract string GetStringValue();

        public override string ToString()
        {
            return GetStringValue();
        }

        public override int GetHashCode() => GetStringValue().GetHashCode();
        public override bool Equals(object obj)
        {
            if (!(obj is MetafileColor metafileColor))
                return false;
            if (metafileColor is MetafileColorIndexed indexedColor)
                return indexedColor.Equals(this);
            return metafileColor.GetStringValue() == GetStringValue();
        }
    }
}
