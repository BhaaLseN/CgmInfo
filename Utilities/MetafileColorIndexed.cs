using System;
using System.Drawing;

namespace CgmInfo.Utilities
{
    public class MetafileColorIndexed : MetafileColor
    {
        public int Index { get; private set; }

        public MetafileColorIndexed(int colorIndex)
        {
            Index = colorIndex;
        }
        public override Color GetColor()
        {
            throw new NotSupportedException("Indexed Color requires a lookup inside the COLOUR TABLE.");
        }
        protected override string GetStringValue()
        {
            return string.Format("Color Index {0}", Index);
        }
    }
}
