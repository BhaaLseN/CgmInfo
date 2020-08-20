using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("AUXCOLR")]
    public class AuxiliaryColor : Command
    {
        public AuxiliaryColor(MetafileColor color)
            : base(3, 3)
        {
            Color = color;
        }

        public MetafileColor Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlAuxiliaryColor(this, parameter);
        }
    }
}
