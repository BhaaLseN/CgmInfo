using System;
using System.Collections.Generic;
using System.IO;
using CgmInfo;
using CgmInfo.Commands;

namespace CgmInfoCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string target = args.Length >= 1 ? args[0] : @"D:\_dev\_work\standards\webcgm20-ts\static10\ALLELM01.cgm";
            var fileNames = new List<string>();

            if (File.Exists(target))
            {
                fileNames.Add(target);
            }
            else if (Directory.Exists(target))
            {
                fileNames.AddRange(Directory.GetFiles(target, "*.cgm"));
            }
            else
            {
                Console.WriteLine("Target '{0}' does not exist or is neither file nor folder.", target);
                return;
            }

            var statsProxy = new StatsReplaceProxy<PrintCommandVisitor, PrintContext>();
            var printVisitor = statsProxy.GetTransparentProxy(fileNames.Count > 1);
            foreach (string fileName in fileNames)
            {
                statsProxy.Reset();
                using (var reader = MetafileReader.Create(fileName))
                {
                    var printContext = new PrintContext(fileName);
                    Command command;
                    do
                    {
                        command = reader.Read();
                        command?.Accept(printVisitor, printContext);
                    } while (command != null);
                }
                statsProxy.Print(fileName);
                statsProxy.SaveTo(Path.ChangeExtension(fileName, ".stats.txt"));
            }
        }
    }
}
