using System.Windows;
using System.Windows.Controls;

namespace CgmInfoGui.Controls
{
    /// <summary>
    /// Interaction logic for PropertiesPanel.xaml
    /// </summary>
    public partial class PropertiesPanel : UserControl
    {
        public PropertiesPanel()
        {
            InitializeComponent();
        }

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(PropertiesPanel), new PropertyMetadata(OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertiesPanel)d).OnSourceChanged(e);
        }
        private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            // TODO: update data template to show either property grid or raw view
        }
    }
}
