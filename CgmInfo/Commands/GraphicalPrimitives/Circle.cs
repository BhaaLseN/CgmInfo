using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("CIRCLE")]
    public class Circle : Command
    {
        public Circle(MetafilePoint center, double radius)
            : base(4, 12)
        {
            Center = center;
            Radius = radius;
        }

        public MetafilePoint Center { get; }
        public double Radius { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircle(this, parameter);
        }
    }
}
