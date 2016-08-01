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
            var path = args[0];


            var pathfix = Path.Combine(Path.GetDirectoryName(path), "Fixed");
            Directory.CreateDirectory(pathfix);
            pathfix = Path.Combine(pathfix, Path.GetFileName(path));
            File.Delete(pathfix);

            ModuleDefMD module = ModuleDefMD.Load(path);
            Console.WriteLine(module.Assembly);


            AssemblyResolver asmResolver = new AssemblyResolver();
            ModuleContext modCtx = new ModuleContext(asmResolver);
            asmResolver.DefaultModuleContext = modCtx;
            asmResolver.EnableTypeDefCache = true;

            module.Context = modCtx;
            module.Context.AssemblyResolver.AddToCache(module);


            new RProperty().Apply(module);
            new RMethod().Apply(module);
            new RField().Apply(module);
            new RType().Apply(module);


            module.Write(pathfix);
            module.Dispose();

            var f = File.OpenRead(pathfix);
            MemoryStream ms = new MemoryStream();
            f.CopyTo(ms);
            f.Dispose();
            ms.Position = 0;
            ModuleDefinition cmodule = ModuleDefinition.ReadModule(ms);
            cmodule.Write(File.Create(pathfix));
            Helper.WriteLine(path + " -> " + pathfix);

        }

    }
}
