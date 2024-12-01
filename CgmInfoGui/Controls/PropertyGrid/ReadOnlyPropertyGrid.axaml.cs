using Avalonia;
using Avalonia.Controls.Primitives;

namespace CgmInfoGui.Controls.PropertyGrid;

public class ReadOnlyPropertyGrid : TemplatedControl
{
    public static readonly StyledProperty<object> SelectedObjectProperty =
        AvaloniaProperty.Register<ReadOnlyPropertyGrid, object>(nameof(SelectedObject));
    public object SelectedObject
    {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }
}
