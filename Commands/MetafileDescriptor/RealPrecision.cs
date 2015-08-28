using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class RealPrecision : Command
    {
        public RealPrecision(RealRepresentation representationForm, int exponentWidth, int fractionWidth)
            : base(1, 5)
        {
            RepresentationForm = representationForm;
            ExponentWidth = exponentWidth;
            FractionWidth = fractionWidth;

            if (RepresentationForm == RealRepresentation.FloatingPoint)
            {
                if (ExponentWidth == 9 && FractionWidth == 23)
                    Specification = RealPrecisionSpecification.FloatingPoint32Bit;
                else if (ExponentWidth == 12 && FractionWidth == 52)
                    Specification = RealPrecisionSpecification.FloatingPoint64Bit;
            }
            else if (RepresentationForm == RealRepresentation.FixedPoint)
            {
                if (ExponentWidth == 16 && FractionWidth == 16)
                    Specification = RealPrecisionSpecification.FixedPoint32Bit;
                else if (ExponentWidth == 32 && FractionWidth == 32)
                    Specification = RealPrecisionSpecification.FixedPoint64Bit;
            }
        }

        public RealRepresentation RepresentationForm { get; private set; }
        public int ExponentWidth { get; private set; }
        public int FractionWidth { get; private set; }
        public RealPrecisionSpecification Specification { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorRealPrecision(this, parameter);
        }
    }
}
