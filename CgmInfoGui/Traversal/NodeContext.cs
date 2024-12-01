using System.Collections.Generic;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal;

public abstract class NodeContext
{
    private readonly Stack<NodeBase> _levelStack = new();
    private readonly List<NodeBase> _rootLevel = [];

    public ICollection<NodeBase> RootLevel => _rootLevel;

    public NodeBase? LastAddedNode { get; protected set; }
    public NodeBase? CurrentLevel { get; private set; }

    protected List<NodeBase> CurrentLevelNodes
        => CurrentLevel != null ? CurrentLevel.Nodes : _rootLevel;

    public NodeBase AddNode(string format, params object[] args)
    {
        var newNode = new SimpleNode(string.Format(format, args));
        AddNode(newNode);
        return newNode;
    }
    public void AddNode(NodeBase node)
    {
        LastAddedNode = node;
        CurrentLevelNodes.Add(node);
    }

    public NodeBase BeginLevel(string format, params object[] args)
    {
        var newNode = new SimpleNode(string.Format(format, args));
        BeginLevel(newNode);
        return newNode;
    }
    public void BeginLevel(NodeBase levelNode, bool doNotAddTheNode = false)
    {
        if (!doNotAddTheNode)
            AddNode(levelNode);
        if (CurrentLevel != null)
            _levelStack.Push(CurrentLevel);
        CurrentLevel = levelNode;
    }

    public NodeBase EndLevel(string format, params object[] args)
    {
        var newNode = new SimpleNode(string.Format(format, args));
        EndLevel(newNode);
        return newNode;
    }
    public void EndLevel(NodeBase endNode)
    {
        AddNode(endNode);
        EndLevel();
    }
    public void EndLevel()
    {
        if (_levelStack.Count > 0)
            CurrentLevel = _levelStack.Pop();
        else
            CurrentLevel = null;
    }
}
