using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("CONNEDGE")]
    public class ConnectingEdge : Command
    {
        public ConnectingEdge()
            : base(4, 21)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveConnectingEdge(this, parameter);
        }
    }
}
