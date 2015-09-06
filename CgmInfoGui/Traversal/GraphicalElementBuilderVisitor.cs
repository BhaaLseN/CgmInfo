using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Traversal;
using CgmInfoGui.Visuals;
using Rect = System.Windows.Rect;

namespace CgmInfoGui.Traversal
{
    public class GraphicalElementBuilderVisitor : CommandVisitor<GraphicalElementContext>
    {
        public override void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, GraphicalElementContext parameter)
        {
            parameter.Visuals.VdcExtent = new Rect(maximumVdcExtent.FirstCorner.ToPoint(), maximumVdcExtent.SecondCorner.ToPoint());
        }

        public override void AcceptGraphicalPrimitiveText(TextCommand text, GraphicalElementContext parameter)
        {
            var textVisual = new TextVisual(text.Text, text.Position.ToPoint());
            parameter.IncreaseBounds(text.Position.ToPoint());
            if (text.Final == FinalFlag.Final)
                parameter.LastText = null;
            else
                parameter.LastText = textVisual;
            parameter.Add(textVisual);
        }
        public override void AcceptGraphicalPrimitiveAppendText(AppendText appendText, GraphicalElementContext parameter)
        {
            if (parameter.LastText == null)
                return;
            var textVisual = parameter.LastText;
            textVisual.Text += appendText.Text;
            if (appendText.Final == FinalFlag.Final)
                parameter.LastText = null;
        }
        public override void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, GraphicalElementContext parameter)
        {
            var textVisual = new TextVisual(restrictedText.Text, restrictedText.Position.ToPoint());
            parameter.IncreaseBounds(new Rect(restrictedText.Position.ToPoint(), new System.Windows.Size(restrictedText.DeltaWidth, restrictedText.DeltaHeight)));
            if (restrictedText.Final == FinalFlag.Final)
                parameter.LastText = null;
            else
                parameter.LastText = textVisual;
            parameter.Add(textVisual);
        }
    }
}
