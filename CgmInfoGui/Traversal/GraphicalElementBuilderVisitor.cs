using System.Linq;
using Avalonia;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Traversal;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Traversal;

public class GraphicalElementBuilderVisitor : CommandVisitor<GraphicalElementContext>
{
    public override void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, GraphicalElementContext parameter)
    {
        parameter.Visuals.VdcExtent = new Rect(maximumVdcExtent.FirstCorner.ToPoint(), maximumVdcExtent.SecondCorner.ToPoint());
    }
    public override void AcceptAttributeLineColor(LineColor lineColor, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.LineColor = lineColor.Color.GetColor();
    }
    public override void AcceptAttributeLineType(LineType lineType, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.LineType = lineType.Index;
    }
    public override void AcceptAttributeLineWidth(LineWidth lineWidth, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.LineWidth = lineWidth.Width;
    }
    public override void AcceptAttributeLineCap(LineCap lineCap, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.DashCap = lineCap.DashCapIndicator;
        parameter.LineAttributes.LineCap = lineCap.LineCapIndicator;
    }
    public override void AcceptAttributeLineJoin(LineJoin lineJoin, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.LineJoin = lineJoin.Index;
    }
    public override void AcceptControlMiterLimit(MiterLimit miterLimit, GraphicalElementContext parameter)
    {
        parameter.LineAttributes.MiterLimit = miterLimit.Limit;
    }

    public override void AcceptGraphicalPrimitivePolyline(Polyline polyline, GraphicalElementContext parameter)
    {
        var line = new LineVisual(polyline.Points.ToPoints(), parameter.LineAttributes.GetPen);
        foreach (var point in polyline.Points.Select(p => p.ToPoint()))
            parameter.IncreaseBounds(point);
        parameter.Add(line);
    }
    public override void AcceptGraphicalPrimitiveRectangle(Rectangle rectangle, GraphicalElementContext parameter)
    {
        var rectVisual = new RectangleVisual(rectangle.FirstCorner.ToPoint(), rectangle.SecondCorner.ToPoint());
        parameter.IncreaseBounds(new Rect(rectangle.FirstCorner.ToPoint(), rectangle.SecondCorner.ToPoint()));
        parameter.Add(rectVisual);
    }

    public override void AcceptGraphicalPrimitiveCircle(Circle circle, GraphicalElementContext parameter)
    {
        var circleVisual = new CircleVisual(circle.Center.ToPoint(), circle.Radius);
        parameter.IncreaseBounds(new Rect(circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius, circle.Radius * 2, circle.Radius * 2));
        parameter.Add(circleVisual);
    }
    public override void AcceptGraphicalPrimitiveEllipse(Ellipse ellipse, GraphicalElementContext parameter)
    {
        var ellipseVisual = new EllipseVisual(ellipse.Center.ToPoint(), ellipse.FirstConjugateDiameter.ToPoint(), ellipse.SecondConjugateDiameter.ToPoint());
        parameter.IncreaseBounds(new Rect(
            ellipseVisual.Center.X - ellipseVisual.RadiusX, ellipseVisual.Center.Y - ellipseVisual.RadiusY,
            ellipseVisual.RadiusX * 2, ellipseVisual.RadiusY * 2));
        parameter.Add(ellipseVisual);
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
        parameter.IncreaseBounds(new Rect(restrictedText.Position.ToPoint(), new Size(restrictedText.DeltaWidth, restrictedText.DeltaHeight)));
        if (restrictedText.Final == FinalFlag.Final)
            parameter.LastText = null;
        else
            parameter.LastText = textVisual;
        parameter.Add(textVisual);
    }
}
