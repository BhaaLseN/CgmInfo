using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CgmInfo.Commands;
using CgmInfo.Traversal;

namespace CgmInfoCmd
{
    public class StatsReplaceProxy<T, TContext> : DispatchProxy where T : ICommandVisitor<TContext>, new()
    {
        private bool _preventActualMethodCalls;
        private readonly T _target;

        private Dictionary<string, Dictionary<string, int>> _stats = new Dictionary<string, Dictionary<string, int>>();

        public ICommandVisitor<TContext> GetTransparentProxy(bool preventActualMethodCalls)
        {
            var proxy = Create<ICommandVisitor<TContext>, StatsReplaceProxy<T, TContext>>();
            // share instance fields; since the created proxy is NOT the same instance as this one.
            var @this = ((StatsReplaceProxy<T, TContext>)proxy);
            @this._preventActualMethodCalls = preventActualMethodCalls;
            @this._stats = _stats;
            return proxy;
        }

        public StatsReplaceProxy()
        {
            _target = new T();
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            CollectStats(args.OfType<Command>().FirstOrDefault());
            object retVal = _preventActualMethodCalls ? null : targetMethod.Invoke(_target, args);
            return retVal;
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

            using var fileStream = File.OpenWrite(statsFileName);
            using var writer = new StreamWriter(fileStream);
            foreach (var group in _stats.OrderBy(g => g.Key))
            {
                writer.WriteLine("{0}: {1} elements", group.Key, group.Value.Count);
                foreach (var element in group.Value.OrderBy(g => g.Key))
                {
                    writer.WriteLine("\t{0}, {1}: {2}", group.Key, element.Key, element.Value);
                }
            }
        }
        private void CollectStats(Command command)
        {
            if (command == null)
                return;

            if (command is UnsupportedCommand unsupportedCommand && unsupportedCommand.IsTextEncoding)
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
