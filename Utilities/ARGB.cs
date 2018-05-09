namespace CgmInfo.Utilities
{
    public struct ARGB
    {
        public int Alpha { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public override string ToString() => $"A={Alpha} R={Red} G={Green} B={Blue}";
    }
}
