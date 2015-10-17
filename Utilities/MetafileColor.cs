using System.Drawing;

namespace CgmInfo.Utilities
{
    public abstract class MetafileColor
    {
        public abstract Color GetColor();
        protected abstract string GetStringValue();

        public override string ToString()
        {
            return GetStringValue();
        }
    }
}
