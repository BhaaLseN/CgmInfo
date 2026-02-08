using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGPICBODY")]
    public class BeginPictureBody : Command
    {
        public BeginPictureBody()
            : base(0, 4)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginPictureBody(this, parameter);
        }
    }
}
