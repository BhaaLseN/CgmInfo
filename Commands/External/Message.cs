using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.External
{
    [TextToken("MESSAGE")]
    public class Message : Command
    {
        public Message(ActionRequired actionRequired, string messageString)
            : base(7, 1)
        {
            ActionRequired = actionRequired;
            MessageString = messageString;
        }

        public ActionRequired ActionRequired { get; private set; }
        public string MessageString { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptExternalMessage(this, parameter);
        }
    }
}
