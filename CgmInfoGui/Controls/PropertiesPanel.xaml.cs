using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using CgmInfo.Commands;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfoGui.Converters;
using CgmInfoGui.ViewModels.Nodes;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace CgmInfoGui.Controls
{
    /// <summary>
    /// Interaction logic for PropertiesPanel.xaml
    /// </summary>
    public partial class PropertiesPanel : UserControl
    {
        static PropertiesPanel()
        {
            AddTypeDescriptorOverrides();
        }
        public PropertiesPanel()
        {
            InitializeComponent();
        }

        private static void AddTypeDescriptorOverrides()
        {
            // required overrides to show up correctly in the property grid
            // used by various string lists (such as font list or metafile elements list)
            AddTypeDescriptorOverride<string>();
            // used by structured data records
            AddTypeDescriptorOverride<StructuredDataElement>();
            // used by character set list
            AddTypeDescriptorOverride<CharacterSetListEntry>();
        }
        private static void AddTypeDescriptorOverride<T>() => TypeDescriptor.AddAttributes(typeof(List<T>), new TypeConverterAttribute(typeof(ExpandableIListConverter<T>)), new ExpandableObjectAttribute());

        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(object), typeof(PropertiesPanel), new PropertyMetadata(OnSourceChanged));

        public FlowDocument RawDocument
        {
            get { return (FlowDocument)GetValue(RawDocumentProperty); }
            set { SetValue(RawDocumentProperty, value); }
        }

        public static readonly DependencyProperty RawDocumentProperty =
            DependencyProperty.Register(nameof(RawDocument), typeof(FlowDocument), typeof(PropertiesPanel), new PropertyMetadata(null));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PropertiesPanel)d).OnSourceChanged(e);
        }

        private FlowDocument ConvertUnsupportedCommand(UnsupportedCommand unsupportedCommand)
        {
            if (unsupportedCommand == null)
                return null;

            if (unsupportedCommand.IsTextEncoding)
                return MakeTextDocument(unsupportedCommand);

            return MakeBinaryDocument(unsupportedCommand);
        }

        private FlowDocument MakeTextDocument(UnsupportedCommand unsupportedCommand)
        {
            return new FlowDocument(new Paragraph(new Run(unsupportedCommand.RawParameters)));
        }

        private FlowDocument MakeBinaryDocument(UnsupportedCommand unsupportedCommand)
        {
            var addressCell = new TableCell();
            var hexCell = new TableCell();
            var textCell = new TableCell();

            int baseAddress = 0;
            int itemsPerLine = 16;
            foreach (var slice in Slice(unsupportedCommand.RawBuffer, itemsPerLine))
            {
                addressCell.Blocks.Add(new Paragraph(new Run(string.Format("0x{0:x08}", baseAddress))));

                var hexLine = new Paragraph();
                var textLine = new Paragraph();
                foreach (byte value in slice)
                {
                    var hexChar = new Run(string.Format("{0:X02} ", value));
                    var textChar = new Run((IsPrintable(value) ? (char)value : '.').ToString());

                    // remember each counterpart for later
                    // TODO: highlight counterpart when selecting things
                    hexChar.Tag = textChar;
                    textChar.Tag = hexChar;

                    hexLine.Inlines.Add(hexChar);
                    textLine.Inlines.Add(textChar);
                }

                hexCell.Blocks.Add(hexLine);
                textCell.Blocks.Add(textLine);

                baseAddress += itemsPerLine;
                if (baseAddress >= 0x1000)
                {
                    hexCell.Blocks.Add(new Paragraph(new Italic(new Run(string.Format("({0} more bytes not shown)", unsupportedCommand.RawBuffer.Length - baseAddress)))));
                    break;
                }
            }

            var tableRow = new TableRow();
            tableRow.Cells.Add(addressCell);
            tableRow.Cells.Add(hexCell);
            tableRow.Cells.Add(textCell);

            var tableRowGroup = new TableRowGroup();
            tableRowGroup.Rows.Add(tableRow);

            var table = new Table();
            table.Columns.Add(new TableColumn { Width = new GridLength(10, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(3 * itemsPerLine, GridUnitType.Star) });
            table.Columns.Add(new TableColumn { Width = new GridLength(itemsPerLine, GridUnitType.Star) });
            table.RowGroups.Add(tableRowGroup);

            return new FlowDocument(table);
        }

        private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            RawDocument = ConvertUnsupportedCommand((e.NewValue as NodeBase)?.Command as UnsupportedCommand);
        }

        private void OnPreparePropertyItem(object sender, PropertyItemEventArgs e)
        {
            if (!(e.Item is PropertyItem propertyItem))
                return;

            // allow expanding pretty much everything that isn't a primitive type.
            if (!propertyItem.PropertyType.IsValueType && propertyItem.PropertyType != typeof(object) && propertyItem.PropertyType != typeof(string))
                propertyItem.IsExpandable = true;

            propertyItem.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, ExecuteCopy, CanExecuteCopy));
        }
        private void ExecuteCopy(object sender, ExecutedRoutedEventArgs e)
        {
            var affectedPropertyItem = e.Parameter as PropertyItem ?? sender as PropertyItem;

            // Clipboard is fickle and might throw for no reason. try a bunch of times before giving up.
            int attempts = 5;
            while (attempts --> 0)
            {
                try
                {
                    Clipboard.SetText(affectedPropertyItem?.Value?.ToString() ?? string.Empty);
                    break;
                }
                catch { }
            }
        }
        private void CanExecuteCopy(object sender, CanExecuteRoutedEventArgs e)
        {
            var affectedPropertyItem = e.Parameter as PropertyItem ?? sender as PropertyItem;
            e.CanExecute = affectedPropertyItem != null && affectedPropertyItem.Value != null;
            e.Handled = true;
        }

        private static bool IsPrintable(byte value)
        {
            return !char.IsControl((char)value);
        }
        private static IEnumerable<ArraySegment<T>> Slice<T>(T[] source, int itemsPerSlice)
        {
            for (int start = 0; start < source.Length; start += itemsPerSlice)
            {
                int count = Math.Min(itemsPerSlice, source.Length - start);
                yield return new ArraySegment<T>(source, start, count);
            }
        }
    }
}
