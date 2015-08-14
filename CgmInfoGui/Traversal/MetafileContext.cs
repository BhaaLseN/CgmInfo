using System;
using System.Collections.Generic;
using System.Linq;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class MetafileContext
    {
        private readonly Stack<NodeBase> _levelStack = new Stack<NodeBase>();
        private readonly List<NodeBase> _rootLevel = new List<NodeBase>();

        private NodeBase _currentLevel;

        public IEnumerable<NodeBase> RootLevel
        {
            get { return _rootLevel; }
        }

        public NodeBase AddMetafileDescriptorNode(string format, params object[] args)
        {
            var newNode = new SimpleNode(string.Format(format, args));
            AddDescriptorNode(newNode);
            return newNode;
        }
        public void AddDescriptorNode(NodeBase node)
        {
            var metafile = _rootLevel.OfType<MetafileViewModel>().FirstOrDefault();
            if (metafile == null)
                throw new InvalidOperationException("Got a Metafile Descriptor element without a Metafile");
            metafile.Descriptor.Nodes.Add(node);
        }

        public NodeBase AddNode(string format, params object[] args)
        {
            var newNode = new SimpleNode(string.Format(format, args));
            AddNode(newNode);
            return newNode;
        }
        public void AddNode(NodeBase node)
        {
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
            if (_levelStack.Count > 0)
                _currentLevel = _levelStack.Pop();
        }
    }
}
