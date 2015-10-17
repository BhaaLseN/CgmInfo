using System.Drawing;

namespace CgmInfo.Utilities
{
    public class MetafileColorRGB : MetafileColor
    {
        public int Red { get; private set; }
        public int Green { get; private set; }
        public int Blue { get; private set; }

        public MetafileColorRGB(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public override Color GetColor()
        {
            return Color.FromArgb(Red, Green, Blue);
        }
        protected override string GetStringValue()
        {
            return string.Format("R={0} G={1} B={2}", Red, Green, Blue);
        }
    }
}
