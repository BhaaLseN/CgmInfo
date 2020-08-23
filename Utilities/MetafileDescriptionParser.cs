using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CgmInfo.Utilities
{
    public static class MetafileDescriptionParser
    {
        // handles parameters such as "key:value""key:value"
        private static readonly Regex DescriptionQuotedParameterRegex = new Regex(
            @"\s*""(?<key>[^:]+?):(?<value>.+?)""",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // handles parameters such as "key:value","key:value" or "key:value";"key:value"
        private static readonly Regex DescriptionCommaSeparatedParameterRegex = new Regex(
            @"\s*""(?<key>[^:]+?):(?<value>.+?)""(?:[;,]|$)",
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
        public static Dictionary<string, string> ParseDescription(string description)
        {
            var ret = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var matches = DescriptionCommaSeparatedParameterRegex.Matches(description);
            if (matches.Count < 2)
            {
                // in case one (or no) match came out including commas, try again without commas.
                // the specification does not indicate if and how keyword:value pairs should be delimited,
                // other than specifying that they should be surrounded by double quotes.
                // most commonly, they appear to be delimited by commas; but sometimes appear without delimiter.
                var nonCommaMatches = DescriptionQuotedParameterRegex.Matches(description);
                if (nonCommaMatches.Count > matches.Count)
                    matches = nonCommaMatches;

                // we might still end up with no really good matches here; so we can exit early without attempting to
                // parse the metafile description string. chances are they aren't keyword:value pairs at all if we still
                // have less than two matches (as just a single pair seems unlikely, with most profiles at least specifying
                // a ProfileId and ProfileEd member; often together with ColourClass/Source/Date members)
                if (matches.Count < 2)
                    return ret;
            }
            foreach (var match in matches.OfType<Match>())
            {
                if (match.Success)
                    ret[match.Groups["key"].Value] = match.Groups["value"].Value;
            }

            return ret;
        }
    }
}
