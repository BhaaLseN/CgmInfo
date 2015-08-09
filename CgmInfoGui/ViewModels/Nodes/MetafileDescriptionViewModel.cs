using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class MetafileDescriptionViewModel : NodeBase
    {
        public MetafileDescriptionViewModel(string description)
        {
            Description = description;

            var entries = ParseDescripton(description);
            if (entries.Any())
            {
                var original = new SimpleNode(string.Format("Original String: [{0} characters]", description.Length));
                original.Nodes.Add(new SimpleNode(description));
                Nodes.Add(original);
                Nodes.AddRange(entries.Select(k => new SimpleNode(string.Format("{0}: {1}", k.Key, k.Value))));
            }
        }
        public string Description { get; }

        public override string DisplayName
        {
            get
            {
                if (Nodes.Any())
                    // show the number of individual parameters; excluding the "original string" one
                    return string.Format("METAFILE DESCRIPTION: [{0} entries]", Nodes.Count - 1);
                else
                    return string.Format("METAFILE DESCRIPTION: {0}", Description);
            }
        }

        // handles parameters such as "key:value""key:value" and "key:value","key:value"
        private static readonly Regex DescriptionQuotedParameterRegex = new Regex(
            @"\s*(?<qt>[""'])(?<key>[^:]+?):(?<value>.+?)\k<qt>(?:[;,]?|$)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // handles parameters such as key:value,key:value
        private static readonly Regex DescriptionCommaSeparatedParameterRegex = new Regex(
            @"\s*(?<key>[^:]+?):(?<value>[^,]+?)(?:,|$)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Dictionary<string, string> ParseDescripton(string description)
        {
            var ret = new Dictionary<string, string>();

            var matches = DescriptionQuotedParameterRegex.Matches(description);
            if (matches.Count == 0)
                matches = DescriptionCommaSeparatedParameterRegex.Matches(description);
            foreach (var match in matches.OfType<Match>())
            {
                if (match.Success)
                    ret[match.Groups["key"].Value] = match.Groups["value"].Value;
            }

            return ret;
        }
    }
}
