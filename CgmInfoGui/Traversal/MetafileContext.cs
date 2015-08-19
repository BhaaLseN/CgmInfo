using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class MetafileContext : NodeContext
    {
        public NodeBase AddMetafileDescriptorNode(string format, params object[] args)
        {
            var newNode = new SimpleNode(string.Format(format, args));
            AddDescriptorNode(newNode);
            return newNode;
        }
        public void AddDescriptorNode(NodeBase node)
        {
            var metafile = RootLevel.OfType<MetafileViewModel>().FirstOrDefault();
            if (metafile == null)
                throw new InvalidOperationException("Got a Metafile Descriptor element without a Metafile");
            metafile.Descriptor.Nodes.Add(node);
        }

        public void AddUnsupportedNode(UnsupportedCommand unsupportedCommand)
        {
            var level = CurrentLevelNodes;
            var container = level.OfType<UnsupportedContainer>().FirstOrDefault();
            if (container == null)
            {
                container = new UnsupportedContainer();
                level.Insert(0, container);
            }
            container.Add(new UnsupportedNode(unsupportedCommand));
        }
    }
}
