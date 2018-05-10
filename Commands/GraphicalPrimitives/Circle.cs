using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Circle : Command
    {
        public Circle(MetafilePoint center, double radius)
            : base(4, 12)
        {
            Center = center;
            Radius = radius;
        }

        public MetafilePoint Center { get; private set; }
        public double Radius { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircle(this, parameter);
        }
    }
}
