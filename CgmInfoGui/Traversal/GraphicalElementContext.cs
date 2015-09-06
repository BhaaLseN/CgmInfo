using System.Windows;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Traversal
{
    public class GraphicalElementContext
    {
        public TextVisual LastText { get; set; }
        public VisualRoot Visuals { get; } = new VisualRoot();

        public void Add(VisualBase visual)
        {
            Visuals.Add(visual);
        }
        public void IncreaseBounds(Point point)
        {
            IncreaseBounds(new Rect(point, new Size(1, 1)));
        }
        public void IncreaseBounds(Rect rect)
        {
            if (Visuals.GeometryExtent.IsEmpty)
            {
                Visuals.GeometryExtent = rect;
            }
            else
            {
                Visuals.GeometryExtent = Rect.Union(Visuals.GeometryExtent, rect);
            }
        }
    }
}
