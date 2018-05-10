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
    }
}
