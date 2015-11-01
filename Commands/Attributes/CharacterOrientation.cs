using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class CharacterOrientation : Command
    {
        public CharacterOrientation(PointF up, PointF @base)
            : base(5, 16)
        {
            Base = @base;
            Up = up;
        }

        public PointF Up { get; private set; }
        public PointF Base { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterOrientation(this, parameter);
        }
    }
}
