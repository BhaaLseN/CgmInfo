using System.Linq;
using CgmInfo.Utilities;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class MetafileDescriptionViewModel : NodeBase
    {
        public MetafileDescriptionViewModel(string description)
        {
            Description = description;

            var entries = MetafileDescriptionParser.ParseDescription(description);
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
    }
}
