using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ELLIPSE")]
    public class Ellipse : Command
    {
        public Ellipse(MetafilePoint center, MetafilePoint firstConjugateDiameter, MetafilePoint secondConjugateDiameter)
            : base(4, 17)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
        }

        public MetafilePoint Center { get; private set; }
        public MetafilePoint FirstConjugateDiameter { get; private set; }
        public MetafilePoint SecondConjugateDiameter { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipse(this, parameter);
        }
    }
}
