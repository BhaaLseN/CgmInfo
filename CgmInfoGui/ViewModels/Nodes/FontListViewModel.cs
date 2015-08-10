using System.Linq;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class FontListViewModel : NodeBase
    {
        public FontListViewModel(FontList fontList)
        {
            Nodes.AddRange(fontList.Fonts.Select(font => new SimpleNode(font)));
        }

        public override string DisplayName
        {
            get { return string.Format("FONT LIST: [{0} entries]", Nodes.Count); }
        }
    }
}
