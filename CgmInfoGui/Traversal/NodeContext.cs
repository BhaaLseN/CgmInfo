using System.Collections.Generic;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public abstract class NodeContext
    {
        private readonly Stack<NodeBase> _levelStack = new Stack<NodeBase>();
        private readonly List<NodeBase> _rootLevel = new List<NodeBase>();

        private NodeBase _currentLevel;

        public ICollection<NodeBase> RootLevel
        {
            get { return _rootLevel; }
        }

        public NodeBase LastAddedNode { get; protected set; }

        protected List<NodeBase> CurrentLevelNodes
        {
            get { return _currentLevel != null ? _currentLevel.Nodes : _rootLevel; }
        }

        public NodeBase AddNode(string format, params object[] args)
        {
            var newNode = new SimpleNode(string.Format(format, args));
            AddNode(newNode);
            return newNode;
        }
        public void AddNode(NodeBase node)
        {
            LastAddedNode = node;
            if (_currentLevel != null)
                _currentLevel.Nodes.Add(node);
            else
                _rootLevel.Add(node);
        }

        public NodeBase BeginLevel(string format, params object[] args)
        {
            var newNode = new SimpleNode(string.Format(format, args));
            BeginLevel(newNode);
            return newNode;
        }
        public void BeginLevel(NodeBase levelNode)
        {
            BeginLevel(levelNode, false);
        }
        public void BeginLevel(NodeBase levelNode, bool doNotAddTheNode)
        {
            if (!doNotAddTheNode)
                AddNode(levelNode);
            if (_currentLevel != null)
                _levelStack.Push(_currentLevel);
            _currentLevel = levelNode;
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
                _currentLevel = _levelStack.Pop();
            else
                _currentLevel = null;
        }
    }
}
