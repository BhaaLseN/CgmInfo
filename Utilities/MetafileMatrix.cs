namespace CgmInfo.Utilities
{
    public sealed class MetafileMatrix
    {
        public MetafileMatrix(double a11, double a12, double a21, double a22, double a13, double a23)
        {
            A11 = a11;
            A12 = a12;

            A21 = a21;
            A22 = a22;

            A13 = a13;
            A23 = a23;

            Elements = new[] { A11, A12, A21, A22, A13, A23 };
        }

        public double A11 { get; }
        public double A12 { get; }

        public double A21 { get; }
        public double A22 { get; }

        public double A13 { get; }
        public double A23 { get; }

        public double[] Elements { get; }
    }
}
