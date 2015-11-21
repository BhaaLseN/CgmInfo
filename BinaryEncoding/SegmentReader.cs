using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.Segment;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.8]
    internal static class SegmentReader
    {
        public static CopySegment CopySegment(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) segment identifier
            // P2: The next 6 values are components of a transformation matrix consisting of a scaling and rotation portion
            // (2 x 2 R) and a translation portion (2 x 1 VDC). In the binary encoding this is expressed as a 2 x 3 matrix of
            // the form:
            //      a11: (real) x scale component
            //      a12: (real) x rotation component
            //      a21: (real) y rotation component
            //      a22: (real) y scale component
            //      a13: (vdc) x translation component
            //      a23: (vdc) y translation component
            // P3: (enumerated) segment transformation application: valid values are
            //      0: no
            //      1: yes
            return new CopySegment(reader.ReadName(), reader.ReadMatrix(), reader.ReadEnum<SegmentTransformationApplication>());
        }
        public static InheritanceFilter InheritanceFilter(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated list) list of one or more of: (list omitted)
            // P2: (enumerated) setting: valid values are
            //      0 state list
            //      1 segment
            var items = new List<InheritanceFilterItem>();
            while (reader.HasMoreData(4)) // 2 per enum
            {
                items.Add(new InheritanceFilterItem(
                    reader.ReadEnum<InheritanceFilterDesignator>(),
                    reader.ReadEnum<InheritanceFilterSetting>()));
            }
            return new InheritanceFilter(items.ToArray());
        }
        public static ClipInheritance ClipInheritance(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) clip inheritance: valid values are
            //      0 state list
            //      1 intersection
            return new ClipInheritance(reader.ReadEnum<ClipInheritanceType>());
        }
        public static SegmentTransformation SegmentTransformation(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) segment identifier
            // P2: The next 6 values are components of a transformation matrix consisting of a scaling and rotation portion
            // (2 x 2 R) and a translation portion (2 x 1 VDC). In the binary encoding this is expressed as a 2 x 3 matrix of
            // the form:
            //      a11: (real) x scale component
            //      a12: (real) x rotation component
            //      a21: (real) y rotation component
            //      a22: (real) y scale component
            //      a13: (vdc) x translation component
            //      a23: (vdc) y translation component
            return new SegmentTransformation(reader.ReadName(), reader.ReadMatrix());
        }
        public static SegmentHighlighting SegmentHighlighting(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) segment identifier
            // P2: (enumerated) highlighting: valid values are
            //      0 normal
            //      1 highlighted
            return new SegmentHighlighting(reader.ReadName(), reader.ReadEnum<Highlighting>());
        }
    }
}
