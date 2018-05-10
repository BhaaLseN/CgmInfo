using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Controls
{
    /// <summary>
    /// Interaction logic for SearchPanel.xaml
    /// </summary>
    public partial class SearchPanel : UserControl
    {
        public SearchPanel()
        {
            InitializeComponent();
            Results = new ObservableCollection<SearchResultItem>();
        }

        public ObservableCollection<SearchResultItem> Results
        {
            get { return (ObservableCollection<SearchResultItem>)GetValue(ResultsProperty); }
            set { SetValue(ResultsProperty, value); }
        }

        public static readonly DependencyProperty ResultsProperty =
            DependencyProperty.Register("Results", typeof(ObservableCollection<SearchResultItem>), typeof(SearchPanel), new PropertyMetadata(null));

        public SearchResultItem SelectedResult
        {
            get { return (SearchResultItem)GetValue(SelectedResultProperty); }
            set { SetValue(SelectedResultProperty, value); }
        }

        public static readonly DependencyProperty SelectedResultProperty =
            DependencyProperty.Register("SelectedResult", typeof(SearchResultItem), typeof(SearchPanel), new PropertyMetadata(OnSelectedResultChanged));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(SearchPanel), new PropertyMetadata(OnSearchTextChanged));

        public ItemsControl Source
        {
            get { return (ItemsControl)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ItemsControl), typeof(SearchPanel), new PropertyMetadata(OnSourceChanged));

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SearchPanel)d).OnSearchTextChanged(e);
        }
        private async void OnSearchTextChanged(DependencyPropertyChangedEventArgs e)
        {
            string searchText = SearchText;
            await PerformSearch(searchText);
        }

        private async Task PerformSearch(string searchText)
        {
            var source = Source;
            if (source != null && source.Items != null && !string.IsNullOrWhiteSpace(searchText))
            {
                var matchStyle = FindResource("SearchResultMatch") as Style;
                var results = await Task.Run(() => FindMatches(source.Items, new int[0], searchText, matchStyle));
                Results = new ObservableCollection<SearchResultItem>(results);
            }
            else
            {
                Results = new ObservableCollection<SearchResultItem>();
            }
        }

        private static IEnumerable<SearchResultItem> FindMatches(IEnumerable items, int[] parentPath, string searchText, Style matchStyle)
        {
            foreach (var item in items.OfType<NodeBase>().Select((n, i) => new { Node = n, Index = i }))
            {
                int[] thisParentPath = parentPath.Concat(new int[] { item.Index }).ToArray();
                if (IsMatch(item.Node.DisplayName, searchText))
                    yield return new SearchResultItem(item.Node, searchText, thisParentPath, matchStyle);
                foreach (var childMatch in FindMatches(item.Node.Nodes, thisParentPath, searchText, matchStyle))
                    yield return childMatch;
            }
        }

        private static bool IsMatch(string fullText, string searchText)
        {
            return !string.IsNullOrEmpty(fullText) && Regex.IsMatch(fullText, Regex.Escape(searchText), RegexOptions.IgnoreCase);
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SearchPanel)d).OnSourceChanged(e);
        }
        private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ItemsControl oldValue && oldValue.Items != null)
                ((INotifyCollectionChanged)oldValue.Items).CollectionChanged -= OnSourceCollectionChanged;
            if (e.NewValue is ItemsControl newValue && newValue.Items != null)
                ((INotifyCollectionChanged)newValue.Items).CollectionChanged += OnSourceCollectionChanged;
        }

        private async void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // any action that changes the source collection invalidates the cached result vectors
            // trash the found results to avoid issues here
            // TODO: handle this differently?
            // FIXME: does this even work? data source is hierarchical...
            Results.Clear();
            // trigger another search to make sure the search result isn't empty.
            if (!string.IsNullOrEmpty(SearchText))
                await PerformSearch(SearchText);
        }

        private static void OnSelectedResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SearchPanel)d).OnSelectedResultChanged(e);
        }
        public void OnSelectedResultChanged(DependencyPropertyChangedEventArgs e)
        {
            var source = Source;
            var selectedResult = SelectedResult;
            if (selectedResult != null)
                SelectItemPath(source, selectedResult.ParentPath);
        }

        private static void SelectItemPath(ItemsControl itemsControl, int[] parentPath)
        {
            if (itemsControl == null || !parentPath.Any())
                return;

            int currentIndex = parentPath.First();
            int[] remainingPath = parentPath.Skip(1).ToArray();

            // items might not be generated yet; we can only proceed here if they are...
            if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                if (itemsControl.ItemContainerGenerator.ContainerFromIndex(currentIndex) is ItemsControl container)
                {
                    // keep selecting items until we reached the target
                    if (remainingPath.Any())
                    {
                        SelectItemPath(container, remainingPath);
                    }
                    else
                    {
                        // select the target item and bring it into view (tree view only)
                        if (container is TreeViewItem treeViewItem)
                        {
                            treeViewItem.IsSelected = true;
                            treeViewItem.BringIntoView();
                        }
                    }
                }
            }
            else
            {
                // If the item containers haven't been generated yet, attach an event
                // and wait for the status to change.
                void selectWhenReadyMethod(object ds, EventArgs de)
                {
                    if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                    {
                        itemsControl.ItemContainerGenerator.StatusChanged -= selectWhenReadyMethod;
                        SelectItemPath(itemsControl, parentPath);
                    }
                }

                itemsControl.ItemContainerGenerator.StatusChanged += selectWhenReadyMethod;
                // force expansion (tree view only)
                if (itemsControl is TreeViewItem treeViewItem)
                    treeViewItem.IsExpanded = true;
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            SearchText = "";
        }
    }

    public sealed class SearchResultItem
    {
        private readonly NodeBase _node;
        internal SearchResultItem(NodeBase node, string searchText, int[] parentPath, Style matchStyle)
        {
            _node = node;
            ParentPath = parentPath;
            string escapedSearchText = Regex.Escape(searchText);
            // split them all, but make the delimiters stick around
            string[] parts = Regex.Split(node.DisplayName, '(' + escapedSearchText + ')', RegexOptions.IgnoreCase);
            var displayText = new TextBlock();
            foreach (string part in parts)
            {
                if (Regex.IsMatch(part, escapedSearchText, RegexOptions.IgnoreCase))
                    // found a match, make it pretty
                    displayText.Inlines.Add(new Run(part) { Style = matchStyle });
                else
                    // not a match, just add it
                    displayText.Inlines.Add(part);
            }
            DisplayText = displayText;
        }

        internal int[] ParentPath { get; }
        public object DisplayText { get; }
    }
}
