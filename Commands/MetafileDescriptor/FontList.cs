using System.Collections.Generic;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class FontList : Command
    {
        public FontList(IEnumerable<string> fonts)
            : base(1, 13)
        {
            Fonts = fonts;
        }

        public IEnumerable<string> Fonts { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorFontList(this, parameter);
        }
    }
}
