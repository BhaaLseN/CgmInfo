namespace CgmInfo.Utilities
{
    public class MetafileColorCMYK : MetafileColor
    {
        public int Cyan { get; }
        public int Magenta { get; }
        public int Yellow { get; }
        public int Black { get; }

        public MetafileColorCMYK(int cyan, int magenta, int yellow, int black)
        {
            Cyan = cyan;
            Magenta = magenta;
            Yellow = yellow;
            Black = black;
        }
        public override ARGB ToARGB()
        {
            double c = Cyan / 255.0;
            double m = Magenta / 255.0;
            double y = Yellow / 255.0;
            double k = Black / 255.0;

            double r = c * (1.0 - k) + k;
            double g = m * (1.0 - k) + k;
            double b = y * (1.0 - k) + k;

            r = (1.0 - r) * 255.0 + 0.5;
            g = (1.0 - g) * 255.0 + 0.5;
            b = (1.0 - b) * 255.0 + 0.5;

            int red = (int)r;
            int green = (int)g;
            int blue = (int)b;

            return new ARGB { Alpha = 255, Red = red, Green = green, Blue = blue };
        }
        protected override string GetStringValue()
        {
            return string.Format("C={0} M={1} Y={2} K={3}", Cyan, Magenta, Yellow, Black);
        }
    }
}
