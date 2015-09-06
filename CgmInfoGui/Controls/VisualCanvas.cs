using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Controls
{
    public class VisualCanvas : FrameworkElement
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(List<VisualBase>), typeof(VisualCanvas), new PropertyMetadata(OnItemsSourceChanged));
        public static readonly DependencyProperty VdcExtentProperty =
            DependencyProperty.Register(nameof(VdcExtent), typeof(Rect), typeof(VisualCanvas), new PropertyMetadata(Rect.Empty, OnVdcExtentChanged));
        public static readonly DependencyProperty GeometryExtentProperty =
            DependencyProperty.Register(nameof(GeometryExtent), typeof(Rect), typeof(VisualCanvas), new PropertyMetadata(Rect.Empty, OnGeometryExtentChanged));

        public static readonly DependencyProperty VdcExtentBrushProperty =
            DependencyProperty.Register(nameof(VdcExtentBrush), typeof(Brush), typeof(VisualCanvas), new PropertyMetadata(null));
        public static readonly DependencyProperty VdcExtentPenProperty =
            DependencyProperty.Register(nameof(VdcExtentPen), typeof(Pen), typeof(VisualCanvas), new PropertyMetadata(new Pen(Brushes.Red, 1).GetCurrentValueAsFrozen()));
        public static readonly DependencyProperty GeometryExtentBrushProperty =
            DependencyProperty.Register(nameof(GeometryExtentBrush), typeof(Brush), typeof(VisualCanvas), new PropertyMetadata(null));
        public static readonly DependencyProperty GeometryExtentPenProperty =
            DependencyProperty.Register(nameof(GeometryExtentPen), typeof(Pen), typeof(VisualCanvas), new PropertyMetadata(new Pen(Brushes.Green, 1).GetCurrentValueAsFrozen()));

        public Brush VdcExtentBrush
        {
            get { return (Brush)GetValue(VdcExtentBrushProperty); }
            set { SetValue(VdcExtentBrushProperty, value); }
        }
        public Pen VdcExtentPen
        {
            get { return (Pen)GetValue(VdcExtentPenProperty); }
            set { SetValue(VdcExtentPenProperty, value); }
        }

        public Brush GeometryExtentBrush
        {
            get { return (Brush)GetValue(GeometryExtentBrushProperty); }
            set { SetValue(GeometryExtentBrushProperty, value); }
        }
        public Pen GeometryExtentPen
        {
            get { return (Pen)GetValue(GeometryExtentPenProperty); }
            set { SetValue(GeometryExtentPenProperty, value); }
        }

        public List<VisualBase> ItemsSource
        {
            get { return (List<VisualBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Rect VdcExtent
        {
            get { return (Rect)GetValue(VdcExtentProperty); }
            set { SetValue(VdcExtentProperty, value); }
        }

        public Rect GeometryExtent
        {
            get { return (Rect)GetValue(GeometryExtentProperty); }
            set { SetValue(GeometryExtentProperty, value); }
        }

        private readonly VisualCollection _visuals;
        public VisualCanvas()
        {
            _visuals = new VisualCollection(this);
            ResetSize();
        }

        private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((VisualCanvas)obj).OnItemsSourceChanged(args);
        }
        protected virtual void OnItemsSourceChanged(DependencyPropertyChangedEventArgs args)
        {
            _visuals.Clear();
            ResetSize();

            if (args.NewValue is List<VisualBase> newList)
            {
                CreateVisuals(newList);
            }
        }

        private static void OnVdcExtentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((VisualCanvas)obj).OnVdcExtentChanged(args);
        }
        protected virtual void OnVdcExtentChanged(DependencyPropertyChangedEventArgs args)
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
            double heightPos = Math.Max(newExtent.Top, 0) + newExtent.Height;
            double widthPos = Math.Max(newExtent.Left, 0) + newExtent.Width;
            const double margin = 50d;

            if (heightPos > Height - margin)
                Height = heightPos + margin;
            if (widthPos > Width - margin)
                Width = widthPos + margin;
        }

        private static void OnGeometryExtentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((VisualCanvas)obj).OnGeometryExtentChanged(args);
        }
        protected virtual void OnGeometryExtentChanged(DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is Rect newExtent)
            {
                UpdateSize(newExtent);
            }
        }
        private void CreateVisuals(List<VisualBase> list)
        {
            foreach (var item in list)
            {
                var drawingVisual = new DrawingVisualX(item);
                _visuals.Add(drawingVisual);
            }
        }

        protected override int VisualChildrenCount
        {
            get { return _visuals.Count; }
        }
        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(VdcExtentBrush, VdcExtentPen, VdcExtent);
            drawingContext.DrawRectangle(GeometryExtentBrush, GeometryExtentPen, GeometryExtent);
            base.OnRender(drawingContext);
        }

        private sealed class DrawingVisualX : DrawingVisual
        {
            public DrawingVisualX(VisualBase visual)
            {
                Visual = visual;

                var context = RenderOpen();
                visual.DrawTo(context);
                context.Close();
            }
            public VisualBase Visual { get; }
        }
    }
}
