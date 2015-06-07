using System;

namespace CgmInfo.Parameters
{
#if DEBUG
    [System.Diagnostics.DebuggerDisplay("{Type} '{Value}'")]
#endif
    public abstract class MetafileDescriptorParameter : Parameter
    {
        public MetafileDescriptorParameter(object value, MetafileDescriptorType type)
            : base(value)
        {
            Type = type;
        }
        public MetafileDescriptorType Type { get; private set; }
        public abstract void Accept<T>(IMetafileDescriptorParameterVisitor<T> visitor, T parameter);
    }
    public enum MetafileDescriptorType
    {
        MetafileVersion = 1, // METAFILE VERSION (I)
        MetafileDescription = 2, // METAFILE DESCRIPTION (SF)
        VdcType = 3, // VDC TYPE (E)
        IntegerPrecision = 4, // INTEGER PRECISION (I)
        RealPrecision = 5, // REAL PRECISION (E, 2I)
        IndexPrecision = 6, // INDEX PRECISION (I)
        ColourPrecision = 7, // COLOUR PRECISION (I)
        ColourIndexPrecision = 8, // COLOUR INDEX PRECISION (I)
        MaximumColourIndex = 9, // MAXIMUM COLOUR INDEX (CI)
        ColourValueExtent = 10, // COLOUR VALUE EXTENT (2CD or 6R)
        MetafileElementList = 11, // METAFILE ELEMENT LIST (I, 2nIX)
        MetafileDefaultsReplacement = 12, // METAFILE DEFAULTS REPLACEMENT (Metafile elements)
        FontList = 13, // FONT LIST (nSF)
        CharacterSetList = 14, // CHARACTER SET LIST (n(E, SF))
        CharacterCodingAnnouncer = 15, // CHARACTER CODING ANNOUNCER (E)
        NamePrecision = 16, // NAME PRECISION (I)
        MaximumVdcExtent = 17, // MAXIMUM VDC EXTENT (2P)
        SegmentPriorityExtent = 18, // SEGMENT PRIORTY EXTENT (2I)
        ColourModel = 19, // COLOUR MODEL (IX)
        ColourCalibration = 20, // COLOUR CALIBRATION (IX, 3R, 18R, I, 6nCCO, I, mCD, 3mR)
        FontProperties = 21, // FONT PROPERTIES (n[IX, I, SDR])
        GlyphMapping = 22, // GLYPH MAPPING (IX, E, SF, I, IX, SDR)
        SymbolLibraryList = 23, // SYMBOL LIBRARY LIST (nSF)
        PictureDirectory = 24, // PICTURE DIRECTORY (E, n(SF, 2[ldt]))
    }
}
