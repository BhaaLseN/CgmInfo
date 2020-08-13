using CgmInfo.Commands;
using CgmInfo.Commands.Segment;
using CgmInfo.Utilities;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.8]
    internal static class SegmentWriter
    {
        public static void CopySegment(MetafileWriter writer, Command command)
        {
            var copySegment = (CopySegment)command;
            // P1: (name) segment identifier
            writer.WriteName(copySegment.SegmentIdentifier);
            // P2: The next 6 values are components of a transformation matrix consisting of a scaling and rotation portion
            // (2 x 2 R) and a translation portion (2 x 1 VDC). In the binary encoding this is expressed as a 2 x 3 matrix of
            // the form:
            //      a11: (real) x scale component
            //      a12: (real) x rotation component
            //      a21: (real) y rotation component
            //      a22: (real) y scale component
            //      a13: (vdc) x translation component
            //      a23: (vdc) y translation component
            WriteMatrix(writer, copySegment.Matrix);
            // P3: (enumerated) segment transformation application: valid values are
            //      0: no
            //      1: yes
            writer.WriteEnum(copySegment.TransformationApplication);
        }
        public static void InheritanceFilter(MetafileWriter writer, Command command)
        {
            var inheritanceFilter = (InheritanceFilter)command;
            foreach (var filterItem in inheritanceFilter.Items)
            {
                // P1: (enumerated list) list of one or more of: (list omitted)
                writer.WriteEnum(filterItem.Designator);
                // P2: (enumerated) setting: valid values are
                //      0 state list
                //      1 segment
                writer.WriteEnum(filterItem.Setting);
            }
        }
        public static void ClipInheritance(MetafileWriter writer, Command command)
        {
            var clipInheritance = (ClipInheritance)command;
            // P1: (enumerated) clip inheritance: valid values are
            //      0 state list
            //      1 intersection
            writer.WriteEnum(clipInheritance.InheritanceType);
        }
        public static void SegmentTransformation(MetafileWriter writer, Command command)
        {
            var segmentTransformation = (SegmentTransformation)command;
            // P1: (name) segment identifier
            writer.WriteName(segmentTransformation.SegmentIdentifier);
            // P2: The next 6 values are components of a transformation matrix consisting of a scaling and rotation portion
            // (2 x 2 R) and a translation portion (2 x 1 VDC). In the binary encoding this is expressed as a 2 x 3 matrix of
            // the form:
            //      a11: (real) x scale component
            //      a12: (real) x rotation component
            //      a21: (real) y rotation component
            //      a22: (real) y scale component
            //      a13: (vdc) x translation component
            //      a23: (vdc) y translation component
            WriteMatrix(writer, segmentTransformation.Matrix);
        }
        public static void SegmentHighlighting(MetafileWriter writer, Command command)
        {
            var segmentHighlighting = (SegmentHighlighting)command;
            // P1: (name) segment identifier
            writer.WriteName(segmentHighlighting.SegmentIdentifier);
            // P2: (enumerated) highlighting: valid values are
            //      0 normal
            //      1 highlighted
            writer.WriteEnum(segmentHighlighting.Highlighting);
        }
        public static void SegmentDisplayPriority(MetafileWriter writer, Command command)
        {
            var segmentDisplayPriority = (SegmentDisplayPriority)command;
            // P1: (name) segment identifier
            writer.WriteName(segmentDisplayPriority.SegmentIdentifier);
            // P2: (integer) segment display priority: valid values are non-negative integers
            writer.WriteInteger(segmentDisplayPriority.Priority);
        }
        public static void SegmentPickPriority(MetafileWriter writer, Command command)
        {
            var segmentPickPriority = (SegmentPickPriority)command;
            // P1: (name) segment identifier
            writer.WriteName(segmentPickPriority.SegmentIdentifier);
            // P2: (integer) segment pick priority: valid values are non-negative integers
            writer.WriteInteger(segmentPickPriority.Priority);
        }
        private static void WriteMatrix(MetafileWriter writer, MetafileMatrix matrix)
        {
            writer.WriteReal(matrix.A11);
            writer.WriteReal(matrix.A12);
            writer.WriteReal(matrix.A21);
            writer.WriteReal(matrix.A22);
            writer.WriteVdc(matrix.A13);
            writer.WriteVdc(matrix.A23);
        }
    }
}
