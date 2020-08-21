using System;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

namespace CgmInfo
{
    public class MetafileDescriptor
    {
        public MetafileDescriptor()
        {
            // ISO/IEC 8632-3 9
            // also ISO/IEC 8632-1 8
            ColorModel = ColorModel.RGB;
            ColorPrecision = 8;
            ColorIndexPrecision = 8;
            RealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
            IntegerPrecision = 16;
            IndexPrecision = 16;
            NamePrecision = 16;
            VdcType = VdcTypeSpecification.Integer;
            VdcIntegerPrecision = 16;
            VdcRealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
            ColorSelectionMode = ColorModeType.Indexed;
            DeviceViewportSpecificationMode = DeviceViewportSpecificationModeType.FractionOfDrawingSurface;

            LineWidthSpecificationMode = WidthSpecificationModeType.Scaled;
            MarkerSizeSpecificationMode = WidthSpecificationModeType.Scaled;
            EdgeWidthSpecificationMode = WidthSpecificationModeType.Scaled;
            InteriorStyleSpecificationMode = WidthSpecificationModeType.Absolute;
        }

        internal void UpdateColorTable(ColorTable colorTable)
        {
            // easiest case first: whole color table is updated, and it's larger than the current one
            if (colorTable.StartIndex == 0 && colorTable.Colors.Length >= ColorTable.Length)
            {
                _colorTable = colorTable.Colors;
            }
            else
            {
                // partial update; make sure we can fit the whole changes
                if (ColorTable.Length < colorTable.StartIndex + colorTable.Colors.Length)
                    Array.Resize(ref _colorTable, colorTable.StartIndex + colorTable.Colors.Length);

                // update the fields specified by COLOUR TABLE; leave everything else as-is
                colorTable.Colors.CopyTo(_colorTable, colorTable.StartIndex);
            }
        }
        public MetafileColor GetIndexedColor(int colorIndex)
        {
            if (colorIndex < 0 || colorIndex >= ColorTable.Length)
                return null;
            return ColorTable[colorIndex];
        }
        public int GetColorIndex(MetafileColor value)
        {
            int foundIndex = Array.IndexOf(ColorTable, value);
            if (foundIndex == -1)
                throw new ArgumentException($"Could not find color '{value}' in color table", nameof(value));
            return foundIndex;
        }

        private MetafileColor[] _colorTable = new MetafileColor[0];
        public MetafileColor[] ColorTable
        {
            get { return _colorTable; }
        }

        public ColorModel ColorModel { get; internal set; }
        public int ColorPrecision { get; internal set; }
        public int ColorIndexPrecision { get; internal set; }
        public RealPrecisionSpecification RealPrecision { get; internal set; }
        public int IntegerPrecision { get; internal set; }
        public int IndexPrecision { get; internal set; }
        public int NamePrecision { get; internal set; }

        public ColorModeType ColorSelectionMode { get; internal set; }
        public DeviceViewportSpecificationModeType DeviceViewportSpecificationMode { get; internal set; }

        public VdcTypeSpecification VdcType { get; internal set; }
        public int VdcIntegerPrecision { get; internal set; }
        public RealPrecisionSpecification VdcRealPrecision { get; internal set; }

        public WidthSpecificationModeType LineWidthSpecificationMode { get; internal set; }
        public WidthSpecificationModeType MarkerSizeSpecificationMode { get; internal set; }
        public WidthSpecificationModeType EdgeWidthSpecificationMode { get; internal set; }
        public WidthSpecificationModeType InteriorStyleSpecificationMode { get; internal set; }
    }
}
