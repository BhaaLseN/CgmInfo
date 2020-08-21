using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("GEOPATDEF")]
    public class GeometricPatternDefinition : Command
    {
        public GeometricPatternDefinition(int geometricPatternIndex, int segmentIdentifier, MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(2, 19)
        {
            GeometricPatternIndex = geometricPatternIndex;
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
            SegmentIdentifier = segmentIdentifier;
        }

        public int GeometricPatternIndex { get; private set; }
        public int SegmentIdentifier { get; private set; }
        public MetafilePoint FirstCorner { get; private set; }
        public MetafilePoint SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorGeometricPatternDefinition(this, parameter);
        }
    }
}
