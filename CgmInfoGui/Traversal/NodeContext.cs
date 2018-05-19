using System.Collections.Generic;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public abstract class NodeContext
    {
        private readonly Stack<NodeBase> _levelStack = new Stack<NodeBase>();
        private readonly List<NodeBase> _rootLevel = new List<NodeBase>();

        public ICollection<NodeBase> RootLevel
        {
            get { return _rootLevel; }
        }

        public NodeBase LastAddedNode { get; protected set; }
        public NodeBase CurrentLevel { get; private set; }

        protected List<NodeBase> CurrentLevelNodes
        {
            get { return CurrentLevel != null ? CurrentLevel.Nodes : _rootLevel; }
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
            if (CurrentLevel != null)
                CurrentLevel.Nodes.Add(node);
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
}
