using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using CgmInfo.Commands;
using CgmInfo.Traversal;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class NodeContextInterceptor<TContext> : RealProxy
        where TContext : NodeContext
    {
        private readonly ICommandVisitor<TContext> _visitor;
        private NodeBase _lastSeenNode;

        public NodeContextInterceptor(ICommandVisitor<TContext> visitor)
            : base(typeof(ICommandVisitor<TContext>))
        {
            _visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IMethodCallMessage methodMessage)
            {
                // call the real method; we need that for the actual node
                object retVal = methodMessage.MethodBase.Invoke(_visitor, methodMessage.Args);
                // attempt to update the currently created node with the command being passed
                UpdateCommand(methodMessage.Args);
                return new ReturnMessage(retVal, null, 0, methodMessage.LogicalCallContext, methodMessage);
            }

            return msg;
        }

        private void UpdateCommand(object[] args)
        {
            // grab the context; every visitor method gets it as its last parameter...or, should.
            var context = args.OfType<NodeContext>().FirstOrDefault();
            if (context == null)
                return;

            // grab the command; every visitor gets it as its first parameter. Again, should. Not sure why we're here if not.
            var command = args.OfType<Command>().FirstOrDefault();
            if (command == null)
                return;

            // don't bother updating something that has already been here right before.
            if (context.LastAddedNode == null || context.LastAddedNode == _lastSeenNode || context.LastAddedNode.Command != null)
                return;

            _lastSeenNode = context.LastAddedNode;
            context.LastAddedNode.Command = command;
        }
    }

    public static class NodeContextExtensions
    {
        public static ICommandVisitor<TContext> WithCommand<TContext>(this ICommandVisitor<TContext> visitor)
            where TContext : NodeContext
        {
            return (ICommandVisitor<TContext>)new NodeContextInterceptor<TContext>(visitor).GetTransparentProxy();
        }
    }
}
