using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CgmInfo.Utilities
{
    public static class MetafileDescriptionParser
    {
        // handles parameters such as "key:value""key:value" and "key:value","key:value"
        private static readonly Regex DescriptionQuotedParameterRegex = new Regex(
            @"\s*(?<qt>[""'])(?<key>[^:]+?):(?<value>.+?)\k<qt>(?:[;,]?|$)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // handles parameters such as key:value,key:value
        private static readonly Regex DescriptionCommaSeparatedParameterRegex = new Regex(
            @"\s*(?<key>[^:]+?):(?<value>[^,]+?)(?:,|$)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Attempts to parse <paramref name="description"/> to return a list of key/value pairs
        /// contained in <paramref name="description"/>.
        /// </summary>
        /// <param name="description">A <see cref="Commands.MetafileDescriptor.MetafileDescription.Description"/></param>
        /// <returns>
        /// A Dictionary with key/value pairs representing <paramref name="description"/>,
        /// or an empty dictionary when the expression could not be parsed.
        /// </returns>
        public static Dictionary<string, string> ParseDescripton(string description)
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
