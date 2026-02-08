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

        public int GeometricPatternIndex { get; }
        public int SegmentIdentifier { get; }
        public MetafilePoint FirstCorner { get; }
        public MetafilePoint SecondCorner { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorGeometricPatternDefinition(this, parameter);
        }
    }
}
