using System;
using System.Collections.Generic;

using dnlib.DotNet;
using Mono.Cecil;
using System.IO;

namespace nname
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Drag a DLL to this EXE");
                    return;
                }
                var path = args[0];

                var pathfix = Path.Combine(Path.GetDirectoryName(path), "Fixed");
                Directory.CreateDirectory(pathfix);
                var fileName = Path.GetFileName(path);
                pathfix = Path.Combine(pathfix, fileName);
                File.Delete(pathfix);

                char[] cs = (fileName.Substring(0, fileName.IndexOf('.')) + ".Bytes").ToCharArray();
                cs[0] = Char.ToUpper(cs[0]);
                RType.rnamespace = new String(cs);


                ModuleDefMD module = ModuleDefMD.Load(path);
                Console.WriteLine(module.Assembly);


                AssemblyResolver asmResolver = new AssemblyResolver();
                ModuleContext modCtx = new ModuleContext(asmResolver);
                asmResolver.DefaultModuleContext = modCtx;
                asmResolver.EnableTypeDefCache = true;

                module.Context = modCtx;
                ((AssemblyResolver)module.Context.AssemblyResolver).AddToCache(module);


                RInstructions rinstructions = new RInstructions(module);

                new RField().Apply(module);
                new RProperty().Apply(module);
                new RMethod().Apply(module);

                new RType().Apply(module);

                rinstructions.Apply();

                new REntryPoint.Apply(module);

                module.Write(pathfix);
                module.Dispose();

                var f = File.OpenRead(pathfix);
                MemoryStream ms = new MemoryStream();
                f.CopyTo(ms);
                f.Dispose();
                ms.Position = 0;
                ModuleDefinition cmodule = ModuleDefinition.ReadModule(ms);

                File.Delete(pathfix);
                using (var tf = File.Create(pathfix))
                {
                    cmodule.Write(tf);
                }
                Helper.WriteLine(path + " -> " + pathfix);
            }
            finally
            {
                Console.ReadLine();
            }
        }

    }
}
