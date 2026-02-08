namespace CgmInfo.Utilities
{
    public class MetafileColorIndexed : MetafileColor
    {
        private readonly MetafileColor _actualColor;

        public int Index { get; }

        public MetafileColorIndexed(int colorIndex, MetafileColor actualColor)
        {
            _actualColor = actualColor;
            Index = colorIndex;
        }
        public override ARGB ToARGB()
        {
            if (_actualColor == null)
                return default(ARGB);
            return _actualColor.ToARGB();
        }
        protected override string GetStringValue()
        {
            if (_actualColor != null)
                return string.Format("Color Index {0} ({1})", Index, _actualColor);
            else
                return string.Format("Color Index {0} (not known by COLOUR TABLE)", Index);
        }

        public override int GetHashCode() => _actualColor != null ? _actualColor.GetHashCode() : base.GetHashCode();
        public override bool Equals(object obj) => _actualColor != null ? _actualColor.Equals(obj) : base.Equals(obj);
    }
}
