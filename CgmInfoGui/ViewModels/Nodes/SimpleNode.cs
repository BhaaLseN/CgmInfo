namespace CgmInfoGui.ViewModels.Nodes;

public class SimpleNode(string? name) : NodeBase
{
    public string Name { get; } = name ?? string.Empty;
    public override string DisplayName => Name;
}
