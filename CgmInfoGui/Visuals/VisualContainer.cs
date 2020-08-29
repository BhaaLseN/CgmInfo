using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CgmInfoGui.ViewModels.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CgmInfoGui.Visuals;

public partial class VisualContainer : ObservableObject, ICollection<VisualContainer>
{
    public VisualContainer(ApplicationStructureNode apsViewModel)
        : this(apsViewModel?.GenerateDisplayText() ?? "(no APS)")
    {
        ApsViewModel = apsViewModel;
    }

    public VisualContainer(string containerName)
    {
        ContainerName = containerName;
    }
    public List<VisualContainer> Children { get; } = [];

    public VisualContainer? Parent { get; private set; }

    private bool _updatingChildren;
    [ObservableProperty]
    private bool? _isVisible = true;
    partial void OnIsVisibleChanged(bool? value)
    {
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
    private void UpdateVisibility()
    {
        // by design, we should NEVER be in here without child entries (because only children should call it)
        if (Children.Count == 0)
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

        IsVisible = finalVisibility;
        Parent?.UpdateVisibility();
    }

    public ApplicationStructureNode? ApsViewModel { get; }
    public void UpdateContainerName()
    {
        if (ApsViewModel == null)
            return;
        ContainerName = ApsViewModel.GenerateDisplayText();
    }
    [ObservableProperty]
    private string _containerName;
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
