using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("TRANSPARENCY")]
    public class Transparency : Command
    {
        public Transparency(OnOffIndicator indicator)
            : base(3, 4)
        {
            Indicator = indicator;
        }

        public OnOffIndicator Indicator { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlTransparency(this, parameter);
        }
    }
}
