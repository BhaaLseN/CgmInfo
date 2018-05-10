namespace CgmInfo.Utilities
{
    public struct MetafilePoint
    {
        public MetafilePoint(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString() => $"X={X} Y={Y}";
    }
}
