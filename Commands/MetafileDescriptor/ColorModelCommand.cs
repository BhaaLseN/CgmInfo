using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("COLRMODEL")]
    public class ColorModelCommand : Command
    {
        public ColorModelCommand(int colorModelValue)
            : base(1, 19)
        {
            ColorModel = (ColorModel)colorModelValue;
        }

        public ColorModel ColorModel { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorColorModel(this, parameter);
        }
    }
}
