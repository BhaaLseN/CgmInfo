using System.Collections.Generic;
using CgmInfo.Commands;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.7]
    internal static class AttributeWriter
    {
        public static void LineBundleIndex(MetafileWriter writer, Command command)
        {
            var lineBundleIndex = (LineBundleIndex)command;
            // P1: (index) line bundle index
            writer.WriteIndex(lineBundleIndex.Index);
        }

        public static void LineType(MetafileWriter writer, Command command)
        {
            var lineType = (LineType)command;
            // P1: (index) line type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            writer.WriteIndex(lineType.Index);
        }

        public static void LineWidth(MetafileWriter writer, Command command)
        {
            var lineWidth = (LineWidth)command;
            // P1: (size specification) line width: see Part 1, subclause 7.1 for its form.
            //      line width is affected by LINE WIDTH SPECIFICATION MODE
            writer.WriteSizeSpecification(lineWidth.Width, writer.Descriptor.LineWidthSpecificationMode);
        }

        public static void LineColor(MetafileWriter writer, Command command)
        {
            var lineColor = (LineColor)command;
            // P1: (colour) line colour
            writer.WriteColor(lineColor.Color);
        }

        public static void MarkerBundleIndex(MetafileWriter writer, Command command)
        {
            var markerBundleIndex = (MarkerBundleIndex)command;
            // P1: (index) marker bundle index
            writer.WriteIndex(markerBundleIndex.Index);
        }

        public static void MarkerType(MetafileWriter writer, Command command)
        {
            var markerType = (MarkerType)command;
            // P1: (index) marker type: the following values are standardized:
            //      1 dot
            //      2 plus
            //      3 asterisk
            //      4 circle
            //      5 cross
            //      >5 reserved for registered values
            //      negative for private use
            writer.WriteIndex(markerType.Index);
        }

        public static void MarkerSize(MetafileWriter writer, Command command)
        {
            var markerSize = (MarkerSize)command;
            // P1: (size specification) marker size: see Part 1, subclause 7.1 for its form.
            //      marker size is affected by MARKER SIZE SPECIFICATION MODE
            writer.WriteSizeSpecification(markerSize.Size, writer.Descriptor.MarkerSizeSpecificationMode);
        }

        public static void MarkerColor(MetafileWriter writer, Command command)
        {
            var markerColor = (MarkerColor)command;
            // P1: (colour) marker colour
            writer.WriteColor(markerColor.Color);
        }

        public static void TextBundleIndex(MetafileWriter writer, Command command)
        {
            var textBundleIndex = (TextBundleIndex)command;
            // P1: (index) text bundle index
            writer.WriteIndex(textBundleIndex.Index);
        }

        public static void TextFontIndex(MetafileWriter writer, Command command)
        {
            var textFontIndex = (TextFontIndex)command;
            // P1: (index) text font index
            writer.WriteIndex(textFontIndex.Index);
        }

        public static void TextPrecision(MetafileWriter writer, Command command)
        {
            var textPrecision = (TextPrecision)command;
            // P1: (enumerated) text precision: valid values are
            //      0 string
            //      1 character
            //      2 stroke
            writer.WriteEnum(textPrecision.Precision);
        }

        public static void CharacterExpansionFactor(MetafileWriter writer, Command command)
        {
            var characterExpansionFactor = (CharacterExpansionFactor)command;
            // P1: (real) character expansion factor
            writer.WriteReal(characterExpansionFactor.Factor);
        }

        public static void CharacterSpacing(MetafileWriter writer, Command command)
        {
            var characterSpacing = (CharacterSpacing)command;
            // P1: (real) additional inter-character space
            writer.WriteReal(characterSpacing.AdditionalIntercharacterSpace);
        }

        public static void TextColor(MetafileWriter writer, Command command)
        {
            var textColor = (TextColor)command;
            // P1: (colour) text colour
            writer.WriteColor(textColor.Color);
        }

        public static void CharacterHeight(MetafileWriter writer, Command command)
        {
            var characterHeight = (CharacterHeight)command;
            // P1: (vdc) character height
            writer.WriteVdc(characterHeight.Height);
        }

        public static void CharacterOrientation(MetafileWriter writer, Command command)
        {
            var characterOrientation = (CharacterOrientation)command;
            // P1: (vdc) X character up component
            writer.WriteVdc(characterOrientation.Up.X);
            // P2: (vdc) Y character up component
            writer.WriteVdc(characterOrientation.Up.Y);
            // P3: (vdc) X character base component
            writer.WriteVdc(characterOrientation.Base.X);
            // P4: (vdc) Y character base component
            writer.WriteVdc(characterOrientation.Base.Y);
        }

        public static void TextPath(MetafileWriter writer, Command command)
        {
            var textPath = (TextPath)command;
            // P1: (enumerated) text path: valid values are:
            //      0 right
            //      1 left
            //      2 up
            //      3 down
            writer.WriteEnum(textPath.Path);
        }

        public static void TextAlignment(MetafileWriter writer, Command command)
        {
            var textAlignment = (TextAlignment)command;
            // P1: (enumerated) horizontal alignment: valid values are:
            //      0 normal horizontal
            //      1 left
            //      2 centre
            //      3 right
            //      4 continuous horizontal
            writer.WriteEnum(textAlignment.Horizontal);
            // P2: (enumerated) vertical alignment
            //      0 normal vertical
            //      1 top
            //      2 cap
            //      3 half
            //      4 base
            //      5 bottom
            //      6 continuous vertical
            writer.WriteEnum(textAlignment.Vertical);
            // P3: (real) continuous horizontal alignment
            writer.WriteReal(textAlignment.HorizontalContinuousAlignment);
            // P4: (real) continuous vertical alignment
            writer.WriteReal(textAlignment.VerticalContinuousAlignment);
        }

        public static void CharacterSetIndex(MetafileWriter writer, Command command)
        {
            var characterSetIndex = (CharacterSetIndex)command;
            // P1: (index) character set index
            writer.WriteIndex(characterSetIndex.Index);
        }

        public static void AlternateCharacterSetIndex(MetafileWriter writer, Command command)
        {
            var alternateCharacterSetIndex = (AlternateCharacterSetIndex)command;
            // P1: (index) alternate character set index
            writer.WriteIndex(alternateCharacterSetIndex.Index);
        }

        public static void FillBundleIndex(MetafileWriter writer, Command command)
        {
            var fillBundleIndex = (FillBundleIndex)command;
            // P1: (index) fill bundle index
            writer.WriteIndex(fillBundleIndex.Index);
        }

        public static void InteriorStyle(MetafileWriter writer, Command command)
        {
            var interiorStyle = (InteriorStyle)command;
            // P1: (enumerated) interior style: valid values are
            //      0 hollow
            //      1 solid
            //      2 pattern
            //      3 hatch
            //      4 empty
            //      5 geometric pattern
            //      6 interpolated
            writer.WriteEnum(interiorStyle.Style);
        }

        public static void FillColor(MetafileWriter writer, Command command)
        {
            var fillColor = (FillColor)command;
            // P1: (colour) fill colour
            writer.WriteColor(fillColor.Color);
        }

        public static void HatchIndex(MetafileWriter writer, Command command)
        {
            var hatchIndex = (HatchIndex)command;
            // P1: (index) hatch index: the following values are standardized:
            //      1 horizontal
            //      2 vertical
            //      3 positive slope
            //      4 negative slope
            //      5 horizontal/vertical crosshatch
            //      6 positive/negative slope crosshatch
            //      >6 reserved for registered values
            //      negative for private use
            writer.WriteIndex(hatchIndex.Index);
        }

        public static void PatternIndex(MetafileWriter writer, Command command)
        {
            var patternIndex = (PatternIndex)command;
            // P1: (index) pattern index
            writer.WriteIndex(patternIndex.Index);
        }

        public static void EdgeBundleIndex(MetafileWriter writer, Command command)
        {
            var edgeBundleIndex = (EdgeBundleIndex)command;
            // P1: (index) edge bundle index
            writer.WriteIndex(edgeBundleIndex.Index);
        }

        public static void EdgeType(MetafileWriter writer, Command command)
        {
            var edgeType = (EdgeType)command;
            // P1: (integer) edge type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            // TODO: all other enumerated types use index, but this one uses integer. typo in spec?
            writer.WriteIndex(edgeType.Index);
        }

        public static void EdgeWidth(MetafileWriter writer, Command command)
        {
            var edgeWidth = (EdgeWidth)command;
            // P1: (size specification) edge width: see part 1, subclause 7.1 for its form.
            //      edge width is affected by EDGE WIDTH SPECIFICATION MODE
            writer.WriteSizeSpecification(edgeWidth.Width, writer.Descriptor.EdgeWidthSpecificationMode);
        }

        public static void EdgeColor(MetafileWriter writer, Command command)
        {
            var edgeColor = (EdgeColor)command;
            // P1: (colour) edge colour
            writer.WriteColor(edgeColor.Color);
        }

        public static void EdgeVisibility(MetafileWriter writer, Command command)
        {
            var edgeVisibility = (EdgeVisibility)command;
            // P1: (enumerated) edge visibility: valid values are
            //      0 off
            //      1 on
            writer.WriteEnum(edgeVisibility.Visibility);
        }

        public static void FillReferencePoint(MetafileWriter writer, Command command)
        {
            var fillReferencePoint = (FillReferencePoint)command;
            // P1: (point) fill reference point
            writer.WritePoint(fillReferencePoint.ReferencePoint);
        }

        public static void PatternTable(MetafileWriter writer, Command command)
        {
            var patternTable = (PatternTable)command;
            // P1: (index) pattern table index
            writer.WriteIndex(patternTable.Index);
            // P2: (integer) nx, the dimension of colour array in the direction of the PATTERN SIZE width vector
            writer.WriteInteger(patternTable.Width);
            // P3: (integer) ny, the dimension of colour array in the direction of the PATTERN SIZE height vector
            writer.WriteInteger(patternTable.Height);
            // P4: (integer) local colour precision: valid values are as for the local colour precision parameter of CELL ARRAY.
            // TODO: do we want to store the original local color precision?
            writer.WriteInteger(0);
            // P5: (colour array) pattern definition
            foreach (var color in patternTable.Colors)
                writer.WriteColor(color);
        }

        public static void PatternSize(MetafileWriter writer, Command command)
        {
            var patternSize = (PatternSize)command;
            // NOTE: Pattern size may only be 'absolute' (VDC) in Version 1 and 2 metafiles. In Version 3 and 4 metafiles it may be
            //       expressed in any of the modes which can be selected with INTERIOR STYLE SPECIFICATION MODE.
            // TODO: since this writer does not check any conformance, we assume version 4.
            const int metafileVersion = 4;
            var specificationMode = metafileVersion < 3 ? WidthSpecificationModeType.Absolute : writer.Descriptor.InteriorStyleSpecificationMode;
            // P1: (size specification) pattern height vector, x component: see part 1, subclause 7.1 for its form.
            writer.WriteSizeSpecification(patternSize.Height.X, specificationMode);
            // P2: (size specification) pattern height vector, y component: see part 1, subclause 7.1 for its form.
            writer.WriteSizeSpecification(patternSize.Height.Y, specificationMode);
            // P3: (size specification) pattern width vector, x component: see part 1, subclause 7.1 for its form.
            writer.WriteSizeSpecification(patternSize.Width.X, specificationMode);
            // P4: (size specification) pattern width vector, y component: see part 1, subclause 7.1 for its form.
            writer.WriteSizeSpecification(patternSize.Width.Y, specificationMode);
        }

        public static void ColorTable(MetafileWriter writer, Command command)
        {
            var colorTable = (ColorTable)command;
            // P1: (colour index) starting colour table index
            writer.WriteColorIndex(colorTable.StartIndex);
            // P2: (direct colour list) list of direct colour values (>3-tuples or 4-tuples of direct colour components (CCO))
            foreach (var color in colorTable.Colors)
                writer.WriteDirectColor(color);

            writer.Descriptor.UpdateColorTable(colorTable);
        }

        public static void AspectSourceFlags(MetafileWriter writer, Command command)
        {
            var aspectSourceFlags = (AspectSourceFlags)command;
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
            // TODO: Text encoding has pseudo-ASF values that combine multiple flags;
            //       this needs to be specialized when we want to write those.
            foreach (var (asfType, asfValue) in aspectSourceFlags.Values)
            {
                writer.WriteEnum(asfType);
                writer.WriteEnum(asfValue);
            }
        }

        public static void PickIdentifier(MetafileWriter writer, Command command)
        {
            var pickIdentifier = (PickIdentifier)command;
            // P1: (name) pick identifier
            writer.WriteName(pickIdentifier.Identifier);
        }

        public static void LineCap(MetafileWriter writer, Command command)
        {
            var lineCap = (LineCap)command;
            // P1: (index) line cap indicator: the following values are standardized:
            //      1 unspecified
            //      2 butt
            //      3 round
            //      4 projecting square
            //      5 triangle
            //      >5 reserved for registered values
            writer.WriteIndex(lineCap.LineCapIndicator);
            // P2: (index) dash cap indicator: valid values are
            //      1 unspecified
            //      2 butt
            //      3 match
            //      >3 reserved for registered values
            writer.WriteIndex(lineCap.DashCapIndicator);
        }

        public static void LineJoin(MetafileWriter writer, Command command)
        {
            var lineJoin = (LineJoin)command;
            // P1: (index) line join indicator: the following values are standardized:
            //      1 unspecified
            //      2 mitre
            //      3 round
            //      4 bevel
            //      >4 reserved for registered values
            writer.WriteIndex(lineJoin.Index);
        }

        public static void LineTypeContinuation(MetafileWriter writer, Command command)
        {
            var lineTypeContinuation = (LineTypeContinuation)command;
            // P1: (index) continuation mode: the following values are standardized:
            //      1 unspecified
            //      2 continue
            //      3 restart
            //      4 adaptive continue
            //      >4 reserved for registered values
            writer.WriteIndex(lineTypeContinuation.Index);
        }

        public static void LineTypeInitialOffset(MetafileWriter writer, Command command)
        {
            var lineTypeInitialOffset = (LineTypeInitialOffset)command;
            // P1: (real) line pattern offset
            writer.WriteReal(lineTypeInitialOffset.Offset);
        }

        public static void RestrictedTextType(MetafileWriter writer, Command command)
        {
            var restrictedTextType = (RestrictedTextType)command;
            // P1: (index) restriction type: the following values are standardized:
            //      1 basic
            //      2 boxed-cap
            //      3 boxed-all
            //      4 isotropic-cap
            //      5 isotropic-all
            //      6 justified
            //      >6 reserved for registered values
            writer.WriteIndex(restrictedTextType.Index);
        }

        public static void InterpolatedInterior(MetafileWriter writer, Command command)
        {
            var interpolatedInterior = (InterpolatedInterior)command;
            // P1: (index) style: valid values are
            //      1 parallel
            //      2 elliptical
            //      3 triangular
            //      >3 reserved for registered values
            writer.WriteIndex(interpolatedInterior.Index);

            // Legal values of the style parameter are positive integers. [ISO/IEC 8632-1 7.7.43]
            // Values greater than 3 are reserved for future standardization and registration.
            if (interpolatedInterior.Index < 1 || interpolatedInterior.Index > 3)
                return;

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

            // P2: (2n(size specification)) reference geometry: see part 1, subclause 7.1 for its form.
            foreach (var point in interpolatedInterior.ReferenceGeometry)
            {
                writer.WriteSizeSpecification(point.X, writer.Descriptor.InteriorStyleSpecificationMode);
                writer.WriteSizeSpecification(point.Y, writer.Descriptor.InteriorStyleSpecificationMode);
            }

            // P3: (integer) number of stages (=m)
            writer.WriteInteger(interpolatedInterior.StageDesignators.Length);
            // P4: (real) array of m stage designators
            foreach (double stage in interpolatedInterior.StageDesignators)
                writer.WriteReal(stage);
            // P5: (colour) array of k colour specifiers: k=3 for triangular, m+1 otherwise.
            foreach (var color in interpolatedInterior.ColorSpecifiers)
                writer.WriteColor(color);
        }

        public static void EdgeCap(MetafileWriter writer, Command command)
        {
            var edgeCap = (EdgeCap)command;
            // P1: (index) edge cap indicator: the following values are standardized:
            //      1 unspecified
            //      2 butt
            //      3 round
            //      4 projected square
            //      5 triangle
            //      >5 reserved for registered values
            writer.WriteIndex(edgeCap.EdgeCapIndicator);
            // P2: (index) dash cap indicator: valid values are
            //      1 unspecified
            //      2 butt
            //      3 match
            //      >3 reserved for registered values
            writer.WriteIndex(edgeCap.DashCapIndicator);
        }

        public static void EdgeJoin(MetafileWriter writer, Command command)
        {
            var edgeJoin = (EdgeJoin)command;
            // P1: (index) edge join indicator: the following values are standardized:
            //      1 unspecified
            //      2 mitre
            //      3 round
            //      4 bevel
            //      >4 reserved for registered values
            writer.WriteIndex(edgeJoin.Index);
        }

        public static void EdgeTypeContinuation(MetafileWriter writer, Command command)
        {
            var edgeTypeContinuation = (EdgeTypeContinuation)command;
            // P1: (index) continuation mode: the following values are standardized:
            //      1 unspecified
            //      2 continue
            //      3 restart
            //      4 adaptive continue
            //      >4 reserved for registered values
            writer.WriteIndex(edgeTypeContinuation.Index);
        }

        public static void EdgeTypeInitialOffset(MetafileWriter writer, Command command)
        {
            var edgeTypeInitialOffset = (EdgeTypeInitialOffset)command;
            // P1: (real) edge pattern offset
            writer.WriteReal(edgeTypeInitialOffset.Offset);
        }

        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
