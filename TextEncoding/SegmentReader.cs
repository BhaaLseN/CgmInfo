using System;
using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.Segment;

namespace CgmInfo.TextEncoding
{
    internal static class SegmentReader
    {
        public static CopySegment CopySegment(MetafileReader reader)
        {
            return new CopySegment(reader.ReadName(), reader.ReadMatrix(), ParseTransformationApplication(reader.ReadEnum()));
        }
        public static InheritanceFilter InheritanceFilter(MetafileReader reader)
        {
            var items = new List<InheritanceFilterItem>();
            while (reader.HasMoreData(2))
            {
                items.Add(new InheritanceFilterItem(
                    ParseFilterDesignator(reader.ReadEnum()),
                    ParseFilterSetting(reader.ReadEnum())));
            }
            return new InheritanceFilter(items.ToArray());
        }
        public static ClipInheritance ClipInheritance(MetafileReader reader)
        {
            return new ClipInheritance(ParseInheritanceType(reader.ReadEnum()));
        }
        public static SegmentTransformation SegmentTransformation(MetafileReader reader)
        {
            return new SegmentTransformation(reader.ReadName(), reader.ReadMatrix());
        }
        public static SegmentHighlighting SegmentHighlighting(MetafileReader reader)
        {
            return new SegmentHighlighting(reader.ReadName(), ParseHighlighting(reader.ReadEnum()));
        }
        public static SegmentDisplayPriority SegmentDisplayPriority(MetafileReader reader)
        {
            return new SegmentDisplayPriority(reader.ReadName(), reader.ReadInteger());
        }
        public static SegmentPickPriority SegmentPickPriority(MetafileReader reader)
        {
            return new SegmentPickPriority(reader.ReadName(), reader.ReadInteger());
        }

        private static SegmentTransformationApplication ParseTransformationApplication(string token)
        {
            // assume "no" unless its "yes"
            if (token.ToUpperInvariant() == "YES")
                return SegmentTransformationApplication.Yes;
            return SegmentTransformationApplication.No;
        }
        private static readonly Dictionary<string, InheritanceFilterDesignator> FilterDesignatorMapping = BuildFilterDesignatorMapping();
        private static Dictionary<string, InheritanceFilterDesignator> BuildFilterDesignatorMapping()
        {
            var mapping = new Dictionary<string, InheritanceFilterDesignator>(StringComparer.OrdinalIgnoreCase);
            var ifdType = typeof(InheritanceFilterDesignator);
            foreach (var filter in Enum.GetValues(ifdType).OfType<InheritanceFilterDesignator>())
            {
                mapping[TextTokenAttribute.GetToken(filter)!] = filter;
            }
            return mapping;
        }
        private static InheritanceFilterDesignator ParseFilterDesignator(string token)
        {
            if (!FilterDesignatorMapping.TryGetValue(token.ToUpperInvariant(), out var designator))
                designator = 0;
            return designator;
        }
        private static InheritanceFilterSetting ParseFilterSetting(string token)
        {
            // assume "state list" unless its "segment"
            if (token.ToUpperInvariant() == "SEG")
                return InheritanceFilterSetting.Segment;
            return InheritanceFilterSetting.StateList;
        }
        private static ClipInheritanceType ParseInheritanceType(string token)
        {
            // assume "state list" unless its "intersection"
            if (token.ToUpperInvariant() == "INTERSECTION")
                return ClipInheritanceType.Intersection;
            return ClipInheritanceType.StateList;
        }
        private static Highlighting ParseHighlighting(string token)
        {
            // assume "normal" unless its "highlighted"
            if (token.ToUpperInvariant() == "HIGHL")
                return Highlighting.Highlighted;
            return Highlighting.Normal;
        }
    }
}
