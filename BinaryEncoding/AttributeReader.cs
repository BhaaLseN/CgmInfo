using System.Collections.Generic;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.7]
    internal static class AttributeReader
    {
        public static LineBundleIndex LineBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line bundle index
            return new LineBundleIndex(reader.ReadIndex());
        }

        public static LineType LineType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            return new LineType(reader.ReadIndex());
        }

        public static LineWidth LineWidth(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) line width: see Part 1, subclause 7.1 for its form.
            //      line width is affected by LINE WIDTH SPECIFICATION MODE
            return new LineWidth(reader.ReadSizeSpecification(reader.Descriptor.LineWidthSpecificationMode));
        }

        public static LineColor LineColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) line colour
            return new LineColor(reader.ReadColor());
        }

        public static MarkerBundleIndex MarkerBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) marker bundle index
            return new MarkerBundleIndex(reader.ReadIndex());
        }

        public static MarkerType MarkerType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) marker type: the following values are standardized:
            //      1 dot
            //      2 plus
            //      3 asterisk
            //      4 circle
            //      5 cross
            //      >5 reserved for registered values
            //      negative for private use
            return new MarkerType(reader.ReadIndex());
        }

        public static MarkerSize MarkerSize(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) marker size: see Part 1, subclause 7.1 for its form.
            //      marker size is affected by MARKER SIZE SPECIFICATION MODE
            return new MarkerSize(reader.ReadSizeSpecification(reader.Descriptor.MarkerSizeSpecificationMode));
        }

        public static MarkerColor MarkerColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) marker colour
            return new MarkerColor(reader.ReadColor());
        }

        public static TextBundleIndex TextBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) text bundle index
            return new TextBundleIndex(reader.ReadIndex());
        }

        public static TextFontIndex TextFontIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) text font index
            return new TextFontIndex(reader.ReadIndex());
        }

        public static TextPrecision TextPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) text precision: valid values are
            //      0 string
            //      1 character
            //      2 stroke
            return new TextPrecision(reader.ReadEnum<TextPrecisionType>());
        }

        public static CharacterExpansionFactor CharacterExpansionFactor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) character expansion factor
            return new CharacterExpansionFactor(reader.ReadReal());
        }

        public static CharacterSpacing CharacterSpacing(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) additional inter-character space
            return new CharacterSpacing(reader.ReadReal());
        }

        public static TextColor TextColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) text colour
            return new TextColor(reader.ReadColor());
        }

        public static CharacterHeight CharacterHeight(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) character height
            return new CharacterHeight(reader.ReadVdc());
        }

        public static CharacterOrientation CharacterOrientation(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) X character up component
            // P2: (vdc) Y character up component
            // P3: (vdc) X character base component
            // P4: (vdc) Y character base component
            return new CharacterOrientation(reader.ReadPoint(), reader.ReadPoint());
        }

        public static TextPath TextPath(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) text path: valid values are:
            //      0 right
            //      1 left
            //      2 up
            //      3 down
            return new TextPath(reader.ReadEnum<TextPathType>());
        }

        public static TextAlignment TextAlignment(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) horizontal alignment: valid values are:
            //      0 normal horizontal
            //      1 left
            //      2 centre
            //      3 right
            //      4 continuous horizontal
            // P2: (enumerated) vertical alignment
            //      0 normal vertical
            //      1 top
            //      2 cap
            //      3 half
            //      4 base
            //      5 bottom
            //      6 continuous vertical
            // P3: (real) continuous horizontal alignment
            // P4: (real) continuous vertical alignment
            return new TextAlignment(reader.ReadEnum<HorizontalTextAlignment>(), reader.ReadEnum<VerticalTextAlignment>(), reader.ReadReal(), reader.ReadReal());
        }

        public static CharacterSetIndex CharacterSetIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) character set index
            return new CharacterSetIndex(reader.ReadIndex());
        }

        public static AlternateCharacterSetIndex AlternateCharacterSetIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) alternate character set index
            return new AlternateCharacterSetIndex(reader.ReadIndex());
        }

        public static FillBundleIndex FillBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) fill bundle index
            return new FillBundleIndex(reader.ReadIndex());
        }

        public static InteriorStyle InteriorStyle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) interior style: valid values are
            //      0 hollow
            //      1 solid
            //      2 pattern
            //      3 hatch
            //      4 empty
            //      5 geometric pattern
            //      6 interpolated
            return new InteriorStyle(reader.ReadEnum<InteriorStyleType>());
        }

        public static FillColor FillColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) fill colour
            return new FillColor(reader.ReadColor());
        }

        public static HatchIndex HatchIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) hatch index: the following values are standardized:
            //      1 horizontal
            //      2 vertical
            //      3 positive slope
            //      4 negative slope
            //      5 horizontal/vertical crosshatch
            //      6 positive/negative slope crosshatch
            //      >6 reserved for registered values
            //      negative for private use
            return new HatchIndex(reader.ReadIndex());
        }

        public static PatternIndex PatternIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) pattern index
            return new PatternIndex(reader.ReadIndex());
        }

        public static EdgeBundleIndex EdgeBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) edge bundle index
            return new EdgeBundleIndex(reader.ReadIndex());
        }

        public static EdgeType EdgeType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) edge type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            // TODO: all other enumerated types use index, but this one uses integer. typo in spec?
            return new EdgeType(reader.ReadIndex());
        }

        public static EdgeWidth EdgeWidth(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) edge width: see part 1, subclause 7.1 for its form.
            //      edge width is affected by EDGE WIDTH SPECIFICATION MODE
            return new EdgeWidth(reader.ReadSizeSpecification(reader.Descriptor.EdgeWidthSpecificationMode));
        }

        public static EdgeColor EdgeColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) edge colour
            return new EdgeColor(reader.ReadColor());
        }

        public static EdgeVisibility EdgeVisibility(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) edge visibility: valid values are
            //      0 off
            //      1 on
            return new EdgeVisibility(reader.ReadEnum<OnOffIndicator>());
        }

        public static FillReferencePoint FillReferencePoint(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) fill reference point
            return new FillReferencePoint(reader.ReadPoint());
        }

        public static PatternTable PatternTable(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) pattern table index
            // P2: (integer) nx, the dimension of colour array in the direction of the PATTERN SIZE width vector
            // P3: (integer) ny, the dimension of colour array in the direction of the PATTERN SIZE height vector
            // P4: (integer) local colour precision: valid values are as for the local colour precision parameter of CELL ARRAY.
            // P5: (colour array) pattern definition
            int index = reader.ReadIndex();
            int nx = reader.ReadInteger();
            int ny = reader.ReadInteger();
            int localColorPrecision = reader.ReadInteger();
            if (localColorPrecision == 0)
            {
                if (reader.Descriptor.ColorSelectionMode == ColorModeType.Direct)
                    localColorPrecision = reader.Descriptor.ColorPrecision;
                else
                    localColorPrecision = reader.Descriptor.ColorIndexPrecision;
            }
            // might be either 1/2/4 or 8/16/32 here; but we want byte-sizes in ReadColor
            if (localColorPrecision >= 8)
                localColorPrecision /= 8;

            var colors = new List<MetafileColor>();
            int count = nx * ny;
            while (reader.HasMoreData() && count --> 0)
                colors.Add(reader.ReadColor(localColorPrecision));

            return new PatternTable(index, nx, ny, colors.ToArray());
        }

        public static PatternSize PatternSize(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) pattern height vector, x component: see part 1, subclause 7.1 for its form.
            // P2: (size specification) pattern height vector, y component: see part 1, subclause 7.1 for its form.
            // P3: (size specification) pattern width vector, x component: see part 1, subclause 7.1 for its form.
            // P4: (size specification) pattern width vector, y component: see part 1, subclause 7.1 for its form.

            // NOTE: Pattern size may only be 'absolute' (VDC) in Version 1 and 2 metafiles. In Version 3 and 4 metafiles it may be
            //       expressed in any of the modes which can be selected with INTERIOR STYLE SPECIFICATION MODE.
            var specificationMode = reader.Properties.Version < 3 ? WidthSpecificationModeType.Absolute : reader.Descriptor.InteriorStyleSpecificationMode;
            return new PatternSize(
                new MetafilePoint(reader.ReadSizeSpecification(specificationMode), reader.ReadSizeSpecification(specificationMode)),
                new MetafilePoint(reader.ReadSizeSpecification(specificationMode), reader.ReadSizeSpecification(specificationMode)));
        }

        public static ColorTable ColorTable(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour index) starting colour table index
            // P2: (direct colour list) list of direct colour values (>3-tuples or 4-tuples of direct colour components (CCO))
            int startIndex = reader.ReadColorIndex();
            var colors = new List<MetafileColor>();
            while (reader.HasMoreData(3)) // at least 3 color components with at least 1 byte each
                colors.Add(reader.ReadDirectColor());
            return new ColorTable(startIndex, colors.ToArray());
        }

        public static AspectSourceFlags AspectSourceFlags(MetafileReader reader, CommandHeader commandHeader)
        {
            // ASPECT SOURCE FLAGS: has up to 18 parameter-pairs, corresponding to each attribute that may be
            // bundled; each parameter-pair contains the ASF type and the ASF value:
            // (enumerated) ASF type; valid values are
            //      0 line type ASF
            //      1 line width ASF
            //      2 line colour ASF
            //      3 marker type ASF
            //      4 markersizeASF
            //      5 marker colour ASF
            //      6 text font index ASF
            //      7 text precision ASF
            //      8 character expansion factor ASF
            //      9 character spacing ASF
            //      10 text colour ASF
            //      11 interior style ASF
            //      12 fill colour ASF
            //      13 hatch index ASF
            //      14 pattern index ASF
            //      15 edge type ASF
            //      16 edge width ASF
            //      17 edge colour ASF
            // (enumerated) ASF value; valid values are
            //      0 individual
            //      1 bundled
            var asf = new Dictionary<AspectSourceFlagsType, AspectSourceFlagsValue>();
            while (reader.HasMoreData(2))
                asf[reader.ReadEnum<AspectSourceFlagsType>()] = reader.ReadEnum<AspectSourceFlagsValue>();
            return new AspectSourceFlags(asf);
        }

        public static PickIdentifier PickIdentifier(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) pick identifier
            return new PickIdentifier(reader.ReadName());
        }

        public static LineCap LineCap(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line cap indicator: the following values are standardized:
            //      1 unspecified
            //      2 butt
            //      3 round
            //      4 projecting square
            //      5 triangle
            //      >5 reserved for registered values
            // P2: (index) dash cap indicator: valid values are
            //      1 unspecified
            //      2 butt
            //      3 match
            //      >3 reserved for registered values
            return new LineCap(reader.ReadIndex(), reader.ReadIndex());
        }

        public static LineJoin LineJoin(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line join indicator: the following values are standardized:
            //      1 unspecified
            //      2 mitre
            //      3 round
            //      4 bevel
            //      >4 reserved for registered values
            return new LineJoin(reader.ReadIndex());
        }

        public static LineTypeContinuation LineTypeContinuation(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) continuation mode: the following values are standardized:
            //      1 unspecified
            //      2 continue
            //      3 restart
            //      4 adaptive continue
            //      >4 reserved for registered values
            return new LineTypeContinuation(reader.ReadIndex());
        }

        public static LineTypeInitialOffset LineTypeInitialOffset(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) line pattern offset
            return new LineTypeInitialOffset(reader.ReadReal());
        }

        public static RestrictedTextType RestrictedTextType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) restriction type: the following values are standardized:
            //      1 basic
            //      2 boxed-cap
            //      3 boxed-all
            //      4 isotropic-cap
            //      5 isotropic-all
            //      6 justified
            //      >6 reserved for registered values
            return new RestrictedTextType(reader.ReadIndex());
        }

        public static InterpolatedInterior InterpolatedInterior(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) style: valid values are
            //      1 parallel
            //      2 elliptical
            //      3 triangular
            //      >3 reserved for registered values
            // P2: (2n(size specification)) reference geometry: see part 1, subclause 7.1 for its form.
            // P3: (integer) number of stages (=m)
            // P4: (real) array of m stage designators
            // P5: (colour) array of k colour specifiers: k=3 for triangular, m+1 otherwise.

            int style = reader.ReadIndex();
            var referenceGeometry = new List<MetafilePoint>();
            var stageDesignators = new List<double>();
            var colorSpecifiers = new List<MetafileColor>();

            // Legal values of the style parameter are positive integers. [ISO/IEC 8632-1 7.7.43]
            // Values greater than 3 are reserved for future standardization and registration.
            if (style >= 1 && style <= 3)
            {
                // parallel: the number of scalars shall be 2. The FILL REFERENCE POINT is one defining
                //      point of a reference line. A second defining point of the reference line is defined by
                //      the 2 scalars, which are respectively the x and y offset of the second point from the
                //      FILL REFERENCE POINT.
                // elliptical: the number of scalars shall be 4. The FILL REFERENCE POINT is the centre of a
                //      reference ellipse. The first pair of scalars are respectively the x and y offset from
                //      the FILL REFERENCE POINT to the first CDP of ellipse and the second pair are
                //      respectively the x and y offset from the FILL REFERENCE POINT to the second
                //      CDP of ellipse.
                // triangular: the number of scalars shall be 4. The first pair of scalars are respectively the x and
                //      y offset from the FILL REFERENCE POINT to the second corner of a reference
                //      triangle and the second pair are respectively the x and y offset from the FILL
                //      REFERENCE POINT to the third corner of the reference triangle. The number of
                //      stages shall be 0 and the list of stage designators shall be empty.
                int geoCount;
                if (style == 1)
                    geoCount = 2;
                else
                    geoCount = 4;
                for (int i = 0; i < geoCount / 2; i++)
                {
                    double rgX = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
                    double rgY = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
                    referenceGeometry.Add(new MetafilePoint(rgX, rgY));
                }

                int numberOfStages = reader.ReadInteger();
                for (int i = 0; i < numberOfStages; i++)
                    stageDesignators.Add(reader.ReadReal());

                int numberOfColors = style == 3 ? 3 : numberOfStages + 1;
                for (int i = 0; i < numberOfColors; i++)
                    colorSpecifiers.Add(reader.ReadColor());
            }
            return new InterpolatedInterior(style, referenceGeometry.ToArray(), stageDesignators.ToArray(), colorSpecifiers.ToArray());
        }

        public static EdgeCap EdgeCap(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) edge cap indicator: the following values are standardized:
            //      1 unspecified
            //      2 butt
            //      3 round
            //      4 projected square
            //      5 triangle
            //      >5 reserved for registered values
            // P2: (index) dash cap indicator: valid values are
            //      1 unspecified
            //      2 butt
            //      3 match
            //      >3 reserved for registered values
            return new EdgeCap(reader.ReadIndex(), reader.ReadIndex());
        }

        public static EdgeJoin EdgeJoin(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) edge join indicator: the following values are standardized:
            //      1 unspecified
            //      2 mitre
            //      3 round
            //      4 bevel
            //      >4 reserved for registered values
            return new EdgeJoin(reader.ReadIndex());
        }

        public static EdgeTypeContinuation EdgeTypeContinuation(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) continuation mode: the following values are standardized:
            //      1 unspecified
            //      2 continue
            //      3 restart
            //      4 adaptive continue
            //      >4 reserved for registered values
            return new EdgeTypeContinuation(reader.ReadIndex());
        }

        public static EdgeTypeInitialOffset EdgeTypeInitialOffset(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) edge pattern offset
            return new EdgeTypeInitialOffset(reader.ReadReal());
        }
    }
}
