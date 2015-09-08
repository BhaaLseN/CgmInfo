using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class AuxiliaryColor : Command
    {
        public AuxiliaryColor(Color color)
            : base(3, 3)
        {
            Color = color;
        }

        public Color Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlAuxiliaryColor(this, parameter);
        }
    }
}
