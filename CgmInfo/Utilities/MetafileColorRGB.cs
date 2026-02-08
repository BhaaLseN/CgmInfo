namespace CgmInfo.Utilities
{
    public class MetafileColorRGB : MetafileColor
    {
        public int Red { get; }
        public int Green { get; }
        public int Blue { get; }

        public MetafileColorRGB(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public override ARGB ToARGB()
        {
            return new ARGB { Alpha = 255, Red = Red, Green = Green, Blue = Blue };
        }
        protected override string GetStringValue()
        {
            return string.Format("R={0} G={1} B={2}", Red, Green, Blue);
        }
    }
}
