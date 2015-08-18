using System;
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
            if (layout != null && layout.Root != null)
            {
                using (var reader = layout.CreateReader())
                {
                    var serializer = new XmlLayoutSerializer(_dockingManager);
                    serializer.Deserialize(reader);
                }
            }
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
