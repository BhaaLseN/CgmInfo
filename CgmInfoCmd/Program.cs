using System;
using System.IO;
using CgmInfo;
using CgmInfo.Binary;
using CgmInfo.Parameters;

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

            using (var reader = new MetafileReader(fileName))
            {
                var printVisitor = new PrintMetafileDescriptorParameterVisitor();
                while (reader.Next())
                {
                    if (reader.State == MetafileState.BeginMetafile)
                    {
                        Console.WriteLine("{0} - {1}", reader.Parameters[0], fileName);
                    }
                    else if (reader.State == MetafileState.MetafileDescriptor)
                    {
                        foreach (object parameter in reader.Parameters)
                        {
                            var mdp = parameter as MetafileDescriptorParameter;
                            if (mdp == null)
                                Console.WriteLine(parameter);
                            else
                                mdp.Accept(printVisitor, 1);
                        }
                    }
                }
            }
        }
    }
}
