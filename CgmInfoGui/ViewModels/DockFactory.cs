using System.Linq;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm;
using Dock.Model.Mvvm.Controls;

namespace CgmInfoGui.ViewModels;

public class DockFactory : Factory
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    public DockFactory(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }
    public override IRootDock CreateLayout()
    {
        var root = CreateRootDock();

        var mainDocuments = new DocumentDock
        {
            VisibleDockables = _mainWindowViewModel.Documents,
            CanCreateDocument = false,
        };
        mainDocuments.ActiveDockable = mainDocuments.DefaultDockable = mainDocuments.VisibleDockables.FirstOrDefault();

        var mainLayout = new ProportionalDock
        {
            Orientation = Orientation.Horizontal,
            VisibleDockables = CreateList<IDockable>(mainDocuments),
            ActiveDockable = mainDocuments,
            DefaultDockable = mainDocuments,
            IsCollapsable = false,
        };

        root.VisibleDockables = CreateList<IDockable>(mainLayout);
        root.DefaultDockable = mainLayout;
        root.ActiveDockable = mainLayout;
        root.IsCollapsable = false;
        return root;
    }
}
