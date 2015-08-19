using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace CgmInfoGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var layout = Properties.Settings.Default.DockLayout;
            // don't load an existing layout if panels are missing; otherwise they wont be shown
            if (layout != null && layout.Root != null && !HasMissingPanels(layout))
            {
                using (var reader = layout.CreateReader())
                {
                    var serializer = new XmlLayoutSerializer(_dockingManager);
                    serializer.Deserialize(reader);
                }
            }
        }

        // this list should always have the same ContentIds listed as the XAML.
        private static readonly string[] PanelContentIds = { "MetafileStructure", "ApplicationStructure", "XCF" };
        private static bool HasMissingPanels(XDocument layout)
        {
            var rootPanel = layout.Root.Element("RootPanel");
            if (rootPanel == null)
                return true;
            var paneGroup = rootPanel.Element("LayoutDocumentPaneGroup");
            if (paneGroup == null)
                return true;

            var existingPanels = paneGroup
                .Elements("LayoutDocumentPane")
                .Elements("LayoutDocument")
                .Attributes("ContentId")
                .Select(a => a.Value)
                .ToArray();
            return PanelContentIds.Except(existingPanels).Any();
        }

        protected override void OnClosed(EventArgs e)
        {
            var layout = new XDocument();
            using (var writer = layout.CreateWriter())
            {
                var serializer = new XmlLayoutSerializer(_dockingManager);
                serializer.Serialize(writer);
            }
            Properties.Settings.Default.DockLayout = layout;
            Properties.Settings.Default.Save();

            base.OnClosed(e);
        }
    }
}
