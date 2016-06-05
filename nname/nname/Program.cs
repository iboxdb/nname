using System;
using System.Collections.Generic;

using dnlib.DotNet;

namespace nname
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            var pathfix = path.Substring(0, path.Length - 4) + "_fixed" + path.Substring(path.Length - 4);

            ModuleDefMD module = ModuleDefMD.Load(path);


            Console.WriteLine(path + " -> " + pathfix + " isILOnly=" + module.IsILOnly);
            module.Write(pathfix);

            Console.WriteLine(typeof(dnlib.DotNet.ArraySig));
        }
    }
}
