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
            var fileName = Path.GetFileName(path);
            pathfix = Path.Combine(pathfix, fileName);
            File.Delete(pathfix);

            RType.rnamespace = fileName.Substring(0, fileName.IndexOf('.')) + ".ByteCodes";


            ModuleDefMD module = ModuleDefMD.Load(path);
            Console.WriteLine(module.Assembly);


            AssemblyResolver asmResolver = new AssemblyResolver();
            ModuleContext modCtx = new ModuleContext(asmResolver);
            asmResolver.DefaultModuleContext = modCtx;
            asmResolver.EnableTypeDefCache = true;

            module.Context = modCtx;
            module.Context.AssemblyResolver.AddToCache(module);


            RInstructions rinstructions = new RInstructions(module);

            new RField().Apply(module);
            new RProperty().Apply(module);
            new RMethod().Apply(module);

            new RType().Apply(module);

            rinstructions.Apply();

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
