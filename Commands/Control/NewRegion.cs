using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("NEWREGION")]
    public class NewRegion : Command
    {
        public NewRegion()
            : base(3, 10)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlNewRegion(this, parameter);
        }
    }
}
