using System;
using System.IO;

namespace CgmInfoCmd
{
    public class PrintContext
    {
        public string FileName { get; }

        private Indent _indent = 0;

        public PrintContext(string fileName)
        {
            FileName = fileName;
        }
        public void WriteLine(string format, params object[] args)
        {
            WriteLineInternal(_indent + string.Format(format, args));
        }
        protected virtual void WriteLineInternal(string message) => Console.WriteLine(message);

        public void BeginLevel()
        {
            _indent++;
        }
        public void EndLevel()
        {
            _indent--;
        }

        private sealed class Indent
        {
            private readonly int _depth;
            private Indent(int depth)
            {
                _depth = depth;
            }
            public static implicit operator Indent(int depth)
            {
                return new Indent(depth);
            }
            public static implicit operator string(Indent indent)
            {
                if (indent == null || indent._depth <= 0)
                    return "";
                return new string('\t', indent._depth);
            }
            public static Indent operator ++(Indent indent)
            {
                return new Indent(indent._depth + 1);
            }
            public static Indent operator --(Indent indent)
            {
                if (indent._depth <= 0)
                    return indent;
                return new Indent(indent._depth - 1);
            }
        }
    }
    public class PrintToFileContext : PrintContext, IDisposable
    {
        private readonly StreamWriter _writer;

        public PrintToFileContext(string cgmFileName, string outputFileName)
            : base(cgmFileName)
        {
            _writer = new StreamWriter(outputFileName);
        }

        protected override void WriteLineInternal(string message) => _writer.WriteLine(message);

        private bool _isDisposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _writer.Flush();
                _writer.Dispose();
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
