using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal;

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
        var metafile = RootLevel.OfType<MetafileNode>().FirstOrDefault();
        if (metafile == null)
            throw new InvalidOperationException("Got a Metafile Descriptor element without a Metafile");
        metafile.Descriptor.Nodes.Add(node);
        LastAddedNode = node;
    }

    public void AddUnsupportedNode(UnsupportedCommand unsupportedCommand)
    {
        var counterNode = RootLevel.OfType<UnsupportedCounter>().FirstOrDefault();
        if (counterNode == null)
        {
            counterNode = new UnsupportedCounter();
            RootLevel.Add(counterNode);
        }
        counterNode.Count(unsupportedCommand);

        var level = CurrentLevelNodes;
        var container = level.OfType<UnsupportedContainer>().FirstOrDefault();
        if (container == null)
        {
            container = new UnsupportedContainer();
            level.Insert(0, container);
        }
        var unsupportedNode = new UnsupportedNode(unsupportedCommand);
        LastAddedNode = unsupportedNode;
        container.Add(unsupportedNode);
    }
}
