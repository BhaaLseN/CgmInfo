using System;
using System.Linq;
using System.Reflection;
using CgmInfo.Commands;
using CgmInfo.Traversal;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal;

public class NodeContextInterceptor<TContext> : DispatchProxy
    where TContext : NodeContext
{
    private ICommandVisitor<TContext> _visitor = null!;
    private NodeBase? _lastSeenNode;

    public NodeContextInterceptor() { }

    public ICommandVisitor<TContext> GetTransparentProxy()
    {
        var proxy = Create<ICommandVisitor<TContext>, NodeContextInterceptor<TContext>>();

        return proxy;
    }
    public static ICommandVisitor<TContext> Around(ICommandVisitor<TContext> visitor)
    {
        if (visitor == null)
            throw new ArgumentNullException(nameof(visitor));

        var proxy = Create<ICommandVisitor<TContext>, NodeContextInterceptor<TContext>>();
        var underlyingInstance = (NodeContextInterceptor<TContext>)proxy;
        underlyingInstance._visitor = visitor;
        return proxy;
    }
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (_visitor == null)
            throw new InvalidOperationException($"Interceptor must be created using the static {nameof(Around)} method on an existing visitor.");

        object? retVal = targetMethod?.Invoke(_visitor, args);
        UpdateCommand(args);
        return retVal;
    }

    private void UpdateCommand(object?[]? args)
    {
        // grab the context; every visitor method gets it as its last parameter...or, should.
        var context = args?.OfType<NodeContext>().FirstOrDefault();
        if (context == null)
            return;

        // grab the command; every visitor gets it as its first parameter. Again, should. Not sure why we're here if not.
        var command = args?.OfType<Command>().FirstOrDefault();
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
        return NodeContextInterceptor<TContext>.Around(visitor);
    }
}
