using System;

namespace CgmInfoCmd
{
    public class PrintContext
    {
        public string FileName { get; private set; }

        private Indent _indent = 0;

        public PrintContext(string fileName)
        {
            FileName = fileName;
        }
        public void WriteLine(object value)
        {
            Console.WriteLine(value);
        }
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(_indent + format, args);
        }

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
}
