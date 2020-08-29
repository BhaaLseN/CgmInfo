using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace CgmInfoGui.Visuals
{
    public class VisualRoot : ICollection<VisualBase>, IEnumerable<VisualBase>
    {
        private readonly List<VisualBase> _visuals = new List<VisualBase>();

        public IEnumerable<VisualBase> Visuals
        {
            get { return _visuals; }
        }

        public VisualContainer NoContainer { get; } = new VisualContainer("(no container)");
        public List<VisualContainer> Containers { get; } = new List<VisualContainer>();

        public Rect VdcExtent { get; set; } = Rect.Empty;
        public Rect GeometryExtent { get; set; } = Rect.Empty;

        // direction of X/Y, which might not be left/down as a canvas takes it
        public double DirectionX { get; set; } = 1.0;
        public double DirectionY { get; set; } = 1.0;

        public VisualRoot()
        {
            Containers.Add(NoContainer);
        }
        #region IEnumerable/ICollection

        public int Count => _visuals.Count;
        public bool IsReadOnly => ((ICollection<VisualBase>)_visuals).IsReadOnly;

        public void Add(VisualBase item)
        {
            _visuals.Add(item);
            item.ParentContainer = NoContainer;
        }

        public void Clear() => _visuals.Clear();
        public bool Contains(VisualBase item) => _visuals.Contains(item);
        public void CopyTo(VisualBase[] array, int arrayIndex) => _visuals.CopyTo(array, arrayIndex);
        public IEnumerator<VisualBase> GetEnumerator() => ((IEnumerable<VisualBase>)_visuals).GetEnumerator();
        public bool Remove(VisualBase item) => _visuals.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<VisualBase>)_visuals).GetEnumerator();

        #endregion
    }
}
