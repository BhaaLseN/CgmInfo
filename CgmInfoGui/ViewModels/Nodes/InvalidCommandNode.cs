using CgmInfo.Commands;

namespace CgmInfoGui.ViewModels.Nodes;

public class InvalidCommandNode : NodeBase
{
    private readonly InvalidCommand _invalidCommand;

    public InvalidCommandNode(InvalidCommand invalidCommand)
    {
        _invalidCommand = invalidCommand;

        string commandIdentifier = invalidCommand.IsTextEncoding ? invalidCommand.ElementName : $"{invalidCommand.ElementClass}/{invalidCommand.ElementId}";
        DisplayName = $"Error reading {commandIdentifier}: {invalidCommand.Exception.Message}";
    }

    public override string DisplayName { get; }
}
