using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class PolygonSet : Command
    {
        public PolygonSet(PointF[] points, EdgeOutFlags[] flags)
            : base(4, 8)
        {
            Points = points;
            Flags = flags;
        }

        public PointF[] Points { get; private set; }
        public EdgeOutFlags[] Flags { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygonSet(this, parameter);
        }
    }
}
