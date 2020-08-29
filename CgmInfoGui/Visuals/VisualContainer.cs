using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Visuals
{
    public class VisualContainer : NotifyPropertyChangedBase, ICollection<VisualContainer>
    {
        public VisualContainer(ApplicationStructureViewModel apsViewModel)
            : this(apsViewModel?.GenerateDisplayText() ?? "(no APS)")
        {
            ApsViewModel = apsViewModel;
        }

        public VisualContainer(string containerName)
        {
            ContainerName = containerName;
        }
        public List<VisualContainer> Children { get; } = new List<VisualContainer>();

        public VisualContainer Parent { get; private set; }

        private bool _updatingChildren;
        private bool? _isVisible = true;
        public bool? IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (!SetField(ref _isVisible, value))
                    return;
                if (Parent != null && Parent._updatingChildren)
                    return;
                Parent?.UpdateVisibility();
                if (!value.HasValue)
                    return;
                try
                {
                    _updatingChildren = true;
                    Children.ForEach(c => c.IsVisible = value);
                }
                finally
                {
                    _updatingChildren = false;
                }
            }
        }
        private void UpdateVisibility()
        {
            // by design, we should NEVER be in here without child entries (because only children should call it)
            if (!Children.Any())
                return;

            bool? finalVisibility;
            if (Children.Any(v => !v.IsVisible.HasValue))
                finalVisibility = null;
            else if (Children.All(v => v.IsVisible.GetValueOrDefault(defaultValue: true)))
                finalVisibility = true;
            else if (Children.All(v => v.IsVisible.GetValueOrDefault(defaultValue: false)))
                finalVisibility = false;
            else
                finalVisibility = null;

            _isVisible = finalVisibility;
            OnPropertyChanged(nameof(IsVisible));
            Parent?.UpdateVisibility();
        }

        public ApplicationStructureViewModel ApsViewModel { get; }
        public void UpdateContainerName()
        {
            if (ApsViewModel == null)
                return;
            ContainerName = ApsViewModel.GenerateDisplayText();
        }
        private string _containerName;
        public string ContainerName
        {
            get { return _containerName; }
            private set { SetField(ref _containerName, value); }
        }

        public int VisualCount { get; internal set; }

        public int Count => Children.Count;
        public int TotalCount => VisualCount + Count;

        public bool IsReadOnly => ((ICollection<VisualContainer>)Children).IsReadOnly;

        public void Add(VisualContainer item)
        {
            Children.Add(item);
            item.Parent = this;
        }
        public void Clear()
        {
            Children.ForEach(c => c.Parent = null);
            Children.Clear();
        }
        public bool Contains(VisualContainer item) => Children.Contains(item);
        public void CopyTo(VisualContainer[] array, int arrayIndex) => Children.CopyTo(array, arrayIndex);
        public IEnumerator<VisualContainer> GetEnumerator() => Children.GetEnumerator();
        public bool Remove(VisualContainer item)
        {
            bool wasRemoved = Children.Remove(item);
            if (wasRemoved)
                item.Parent = null;
            return wasRemoved;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
