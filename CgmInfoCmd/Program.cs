using System;
using System.IO;
using CgmInfo;
using CgmInfo.Commands;

namespace CgmInfoCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.Length >= 1 ? args[0] : @"D:\_dev\_work\standards\webcgm20-ts\static10\ALLELM01.cgm";
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File '{0}' does not exist.", fileName);
                return;
            }

            using (var reader = MetafileReader.Create(fileName))
            {
                var printVisitor = new PrintCommandVisitor();
                var printContext = new PrintContext(fileName);
                Command command;
                do
                {
                    command = reader.ReadCommand();
                    if (command != null)
                    {
                        command.Accept(printVisitor, printContext);
                    }
                } while (command != null);
            }
        }
    }
}
