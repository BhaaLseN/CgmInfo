using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Controls;

public partial class SearchPanel : UserControl
{
    public SearchPanel()
    {
        InitializeComponent();
        SearchTextProperty.Changed
            .Throttle(TimeSpan.FromMilliseconds(500))
            // TODO: figure out how to use ObserveOn and friends here (what's the correct Scheduler to use?)
            .Subscribe(_ => Dispatcher.UIThread.Invoke(PerformSearch));
    }

    public static readonly StyledProperty<IEnumerable<NodeBase>?> SourceProperty =
        AvaloniaProperty.Register<SearchPanel, IEnumerable<NodeBase>?>(nameof(Source));
    public IEnumerable<NodeBase>? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public static readonly StyledProperty<List<SearchResultItem>?> ResultsProperty =
        AvaloniaProperty.Register<SearchPanel, List<SearchResultItem>?>(nameof(Results));
    public List<SearchResultItem>? Results
    {
        get => GetValue(ResultsProperty);
        set => SetValue(ResultsProperty, value);
    }

    public static readonly StyledProperty<SearchResultItem?> SelectedResultProperty =
        AvaloniaProperty.Register<SearchPanel, SearchResultItem?>(nameof(SelectedResult));
    public SearchResultItem? SelectedResult
    {
        get => GetValue(SelectedResultProperty);
        set => SetValue(SelectedResultProperty, value);
    }

    public static readonly StyledProperty<string?> SearchTextProperty =
        AvaloniaProperty.Register<SearchPanel, string?>(nameof(SearchText));
    public string? SearchText
    {
        get => GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }

    public static readonly StyledProperty<TreeView> TreeProperty =
        AvaloniaProperty.Register<SearchPanel, TreeView>(nameof(Tree));
    public TreeView Tree
    {
        get => GetValue(TreeProperty);
        set => SetValue(TreeProperty, value);
    }
    private void OnClearClick(object sender, RoutedEventArgs e)
    {
        SearchText = "";
    }
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == SelectedResultProperty)
            UpdateTreeSelection();
        else if (change.Property == SourceProperty && !string.IsNullOrWhiteSpace(SearchText))
            Dispatcher.UIThread.Invoke(PerformSearch);
    }
    private void UpdateTreeSelection()
    {
        var tree = Tree;
        var selectedResult = SelectedResult;
        if (tree is null || selectedResult is null)
            return;

        foreach (var parentNode in selectedResult.ParentPath)
            parentNode.IsExpanded = true;
    }

    private void PerformSearch()
    {
        string? searchText = SearchText;
        var source = Source;
        if (source is null || string.IsNullOrWhiteSpace(searchText))
        {
            Results = null;
            SelectedResult = null;
            return;
        }

        var isMatch = new Regex(Regex.Escape(searchText), RegexOptions.IgnoreCase);
        var matches = FindMatches(source, [], searchText, isMatch);

        Results = matches.ToList();
    }
    private static IEnumerable<SearchResultItem> FindMatches(IEnumerable<NodeBase> items, NodeBase[] parentPath, string searchText, Regex isMatch)
    {
        foreach (var item in items.OfType<NodeBase>().Select((n, i) => new { Node = n, Index = i }))
        {
            NodeBase[] thisParentPath = [.. parentPath, item.Node];
            if (!string.IsNullOrWhiteSpace(item.Node.DisplayName) && isMatch.IsMatch(item.Node.DisplayName))
                yield return new SearchResultItem(item.Node, searchText, thisParentPath);
            foreach (var childMatch in FindMatches(item.Node.Nodes, thisParentPath, searchText, isMatch))
                yield return childMatch;
        }
    }

    public sealed class SearchResultItem
    {
        internal SearchResultItem(NodeBase node, string searchText, NodeBase[] parentPath)
        {
            Node = node;
            ParentPath = parentPath;
            string escapedSearchText = Regex.Escape(searchText);
            // split them all, but make the delimiters stick around
            string[] parts = Regex.Split(node.DisplayName, '(' + escapedSearchText + ')', RegexOptions.IgnoreCase);
            var displayText = new TextBlock { Inlines = [] };
            foreach (string part in parts)
            {
                if (Regex.IsMatch(part, escapedSearchText, RegexOptions.IgnoreCase))
                    // found a match, make it pretty
                    displayText.Inlines.Add(new Run(part) { Classes = { "match" } });
                else
                    // not a match, just add it
                    displayText.Inlines.Add(part);
            }
            DisplayText = displayText;
        }

        public NodeBase Node { get; }
        internal NodeBase[] ParentPath { get; }
        public object DisplayText { get; }
    }
}
