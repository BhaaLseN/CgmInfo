using Avalonia;
using Avalonia.Controls;

namespace CgmInfoGui.Controls;

public partial class PropertiesPanel : UserControl
{
    public PropertiesPanel()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<object?> SourceProperty =
        AvaloniaProperty.Register<PropertiesPanel, object?>(nameof(Source));

    public object? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}
