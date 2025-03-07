using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Controls;

public class VisualCanvas : ItemsControl
{
    public static readonly StyledProperty<Rect> VdcExtentProperty =
        AvaloniaProperty.Register<VisualCanvas, Rect>(nameof(VdcExtent));
    public static readonly StyledProperty<Rect> GeometryExtentProperty =
        AvaloniaProperty.Register<VisualCanvas, Rect>(nameof(GeometryExtent));
    public static readonly StyledProperty<double> DirectionXProperty =
        AvaloniaProperty.Register<VisualCanvas, double>(nameof(DirectionX), defaultValue: 1.0);
    public static readonly StyledProperty<double> DirectionYProperty =
        AvaloniaProperty.Register<VisualCanvas, double>(nameof(DirectionY), defaultValue: 1.0);

    public static readonly StyledProperty<Brush> VdcExtentBrushProperty =
        AvaloniaProperty.Register<VisualCanvas, Brush>(nameof(VdcExtentBrush));
    public static readonly StyledProperty<Pen> VdcExtentPenProperty =
        AvaloniaProperty.Register<VisualCanvas, Pen>(nameof(VdcExtentPen), defaultValue: new Pen(Brushes.Red, 1));
    public static readonly StyledProperty<Brush> GeometryExtentBrushProperty =
        AvaloniaProperty.Register<VisualCanvas, Brush>(nameof(GeometryExtentBrush));
    public static readonly StyledProperty<Pen> GeometryExtentPenProperty =
        AvaloniaProperty.Register<VisualCanvas, Pen>(nameof(GeometryExtentPen), defaultValue: new Pen(Brushes.Green, 1));

    public Brush VdcExtentBrush
    {
        get { return GetValue(VdcExtentBrushProperty); }
        set { SetValue(VdcExtentBrushProperty, value); }
    }
    public Pen VdcExtentPen
    {
        get { return GetValue(VdcExtentPenProperty); }
        set { SetValue(VdcExtentPenProperty, value); }
    }

    public Brush GeometryExtentBrush
    {
        get { return GetValue(GeometryExtentBrushProperty); }
        set { SetValue(GeometryExtentBrushProperty, value); }
    }
    public Pen GeometryExtentPen
    {
        get { return GetValue(GeometryExtentPenProperty); }
        set { SetValue(GeometryExtentPenProperty, value); }
    }

    public Rect VdcExtent
    {
        get { return GetValue(VdcExtentProperty); }
        set { SetValue(VdcExtentProperty, value); }
    }

    public Rect GeometryExtent
    {
        get { return GetValue(GeometryExtentProperty); }
        set { SetValue(GeometryExtentProperty, value); }
    }
    public double DirectionX
    {
        get { return GetValue(DirectionXProperty); }
        set { SetValue(DirectionXProperty, value); }
    }
    public double DirectionY
    {
        get { return GetValue(DirectionYProperty); }
        set { SetValue(DirectionYProperty, value); }
    }

    private VisualBase[]? _visuals;
    private bool _needsRedraw = true;

    public VisualCanvas()
    {
        ResetSize();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ItemsSourceProperty)
            OnItemsSourceChanged(change);
        else if (change.Property == VdcExtentProperty)
            OnVdcExtentChanged(change);
        else if (change.Property == GeometryExtentProperty)
            OnGeometryExtentChanged(change);
    }
    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is not VisualRoot vm)
            return;

        UpdateVisuals(vm.Visuals);
    }
    protected virtual void OnItemsSourceChanged(AvaloniaPropertyChangedEventArgs args)
    {
        ResetSize();

        if (args.NewValue is List<VisualBase> newList)
        {
            UpdateVisuals(newList);
        }
    }
    protected virtual void OnVdcExtentChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is Rect newExtent)
        {
            UpdateSize(newExtent);
        }
    }

    private void ResetSize()
    {
        // avoid having NaN so the re-calculation can do its job
        Height = Width = 1;
    }
    private void UpdateSize(Rect newExtent)
    {
        // TODO: does this even make sense with visuals and coordinates below zero?
        double heightPos = Math.Max(newExtent.Top, 0) + newExtent.Height;
        double widthPos = Math.Max(newExtent.Left, 0) + newExtent.Width;
        const double margin = 50d;

        if (heightPos > Height - margin)
            Height = heightPos + margin;
        if (widthPos > Width - margin)
            Width = widthPos + margin;
    }

    protected virtual void OnGeometryExtentChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is Rect newExtent)
        {
            UpdateSize(newExtent);
        }
    }
    private void UpdateVisuals(IEnumerable<VisualBase> list)
    {
        _visuals = [.. list];
        _needsRedraw = true;
        InvalidateVisual();
    }
    private void DrawVisuals()
    {
        if (!_needsRedraw || _visuals is null)
            return;

        _needsRedraw = false;
        Items.Clear();
        var visualContext = new VisualContext(GeometryExtent, DirectionX, DirectionY);
        foreach (var item in _visuals)
        {
            var drawingVisual = new DrawingGroup();
            // TODO: do we need to remember the original visual?
            using (var context = drawingVisual.Open())
                item.DrawTo(context, visualContext);
            Items.Add(new DrawingVisual(item, drawingVisual));
        }
    }

    public override void Render(DrawingContext drawingContext)
    {
        var paperBrush = new SolidColorBrush(VisualBase.GetBackgroundColor());
        // TODO: this should only be the VDC extent, since the image defines the paper size.
        var paperSize = VdcExtent.Union(GeometryExtent);
        drawingContext.FillRectangle(paperBrush, paperSize);

        DrawVisuals();

        foreach (var item in Items.OfType<DrawingVisual>())
            item.Drawing.Draw(drawingContext);

        drawingContext.DrawRectangle(VdcExtentBrush, VdcExtentPen, VdcExtent);
        drawingContext.DrawRectangle(GeometryExtentBrush, GeometryExtentPen, GeometryExtent);
        base.Render(drawingContext);
    }

    private record class DrawingVisual(VisualBase Visual, DrawingGroup Drawing);
}
