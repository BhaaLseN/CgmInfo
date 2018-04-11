using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using CgmInfo.Commands;
using CgmInfo.Traversal;

namespace CgmInfoCmd
{
    class StatsReplaceProxy<T, TContext> : RealProxy where T : ICommandVisitor<TContext>, new()
    {
        private readonly bool _preventActualMethodCalls;
        private readonly T _target;

        private readonly Dictionary<string, Dictionary<string, int>> _stats = new Dictionary<string, Dictionary<string, int>>();

        public new ICommandVisitor<TContext> GetTransparentProxy()
        {
            return (ICommandVisitor<TContext>)base.GetTransparentProxy();
        }

        public StatsReplaceProxy(bool preventActualMethodCalls)
            : base(typeof(ICommandVisitor<TContext>))
        {
            _preventActualMethodCalls = preventActualMethodCalls;
            _target = new T();
        }
        public override IMessage Invoke(IMessage msg)
        {
            var methodMessage = msg as IMethodCallMessage;

            if (methodMessage != null)
            {
                CollectStats(methodMessage.Args.OfType<Command>().FirstOrDefault());
                object retVal = _preventActualMethodCalls ? null : methodMessage.MethodBase.Invoke(_target, methodMessage.Args);
                return new ReturnMessage(retVal, null, 0, methodMessage.LogicalCallContext, methodMessage);
            }

            return msg;
        }
        public void Reset()
        {
            _stats.Clear();
        }
        public void Print(string fileName)
        {
            Console.WriteLine("Stats for {0}", fileName);
            foreach (var group in _stats.OrderBy(g => g.Key))
            {
                Console.WriteLine("{0}: {1} elements", group.Key, group.Value.Count);
            }
        }
        public void SaveTo(string statsFileName)
        {
            if (File.Exists(statsFileName))
                File.Delete(statsFileName);

            using (var fileStream = File.OpenWrite(statsFileName))
            using (var writer = new StreamWriter(fileStream))
            {
                foreach (var group in _stats.OrderBy(g => g.Key))
                {
                    writer.WriteLine("{0}: {1} elements", group.Key, group.Value.Count);
                    foreach (var element in group.Value.OrderBy(g => g.Key))
                    {
                        writer.WriteLine("\t{0}, {1}: {2}", group.Key, element.Key, element.Value);
                    }
                }
            }
        }
        private void CollectStats(Command command)
        {
            if (command == null)
                return;

            var unsupportedCommand = command as UnsupportedCommand;
            if (unsupportedCommand != null && unsupportedCommand.IsTextEncoding)
                AddGroupCount("Unsupported", unsupportedCommand.ElementName);
            else
                AddGroupCount(GetElementClass(command.ElementClass), GetElementId(command.ElementId));
        }
        private void AddGroupCount(string className, string elementName)
        {
            if (!_stats.TryGetValue(className, out var elements))
            {
                elements = new Dictionary<string, int>();
                _stats[className] = elements;
            }
            if (!elements.TryGetValue(elementName, out int count))
                count = 0;
            elements[elementName] = count + 1;
        }
        private string GetElementClass(int elementClass)
        {
            return string.Format("Class {0}", elementClass);
        }
        private string GetElementId(int elementId)
        {
            return string.Format("Id {0}", elementId);
        }
    }
}
