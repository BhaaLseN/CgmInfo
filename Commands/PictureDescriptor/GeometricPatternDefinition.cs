using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class GeometricPatternDefinition : Command
    {
        public GeometricPatternDefinition(int geometricPatternIndex, int segmentIdentifier, PointF firstCorner, PointF secondCorner)
            : base(2, 19)
        {
            GeometricPatternIndex = geometricPatternIndex;
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
            SegmentIdentifier = segmentIdentifier;
        }

        public int GeometricPatternIndex { get; private set; }
        public int SegmentIdentifier { get; private set; }
        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorGeometricPatternDefinition(this, parameter);
        }
    }
}
