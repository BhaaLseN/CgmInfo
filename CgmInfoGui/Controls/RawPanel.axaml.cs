using Avalonia;
using Avalonia.Controls;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Controls;

public partial class RawPanel : UserControl
{
    public RawPanel()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<NodeBase?> SourceProperty =
        AvaloniaProperty.Register<PropertiesPanel, NodeBase?>(nameof(Source));

    public NodeBase? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}
