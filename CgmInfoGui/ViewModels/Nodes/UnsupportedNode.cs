using CgmInfo.Commands;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class UnsupportedNode : NodeBase
    {
        private readonly UnsupportedCommand _unsupportedCommand;
        public UnsupportedNode(UnsupportedCommand unsupportedCommand)
        {
            _unsupportedCommand = unsupportedCommand;
        }
        public override string DisplayName
        {
            get
            {
                if (_unsupportedCommand.IsTextEncoding)
                    return string.Format("{0} (Parameters: '{1}')", _unsupportedCommand.ElementName, _unsupportedCommand.RawParameters);
                else
                    return string.Format("Element Class={0}, Id={1}", _unsupportedCommand.ElementClass, _unsupportedCommand.ElementId);
            }
        }
    }
}
