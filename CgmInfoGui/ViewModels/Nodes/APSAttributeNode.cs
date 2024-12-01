using System;
using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfoGui.ViewModels.Nodes;

public class APSAttributeNode : NodeBase
{
    public APSAttributeNode(ApplicationStructureAttribute apsAttribute)
    {
        Name = apsAttribute.AttributeType;
        Nodes.AddRange(apsAttribute.DataRecord.Elements.SelectMany(e => e.Values).Select(e => new SimpleNode(Convert.ToString(e))));

        // pick out a few interresting attribute types that might be useful later on
        object[] allValues = apsAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
        switch (Name.ToUpperInvariant())
        {
            case "NAME":
            case "LAYERNAME":
                if (allValues is [var singleValue])
                    Value = Convert.ToString(singleValue);
                break;
        }
    }
    public string Name { get; }
    // only set when it matches a particularly interesting type
    public string? Value { get; }

    public override string DisplayName => Name;
}
