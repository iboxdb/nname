using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;

namespace nname
{
    class REntryPoint
    {
        internal class Apply
        {
            private ModuleDefMD module;

            public Apply(ModuleDefMD module)
            {
                this.module = module;
                if (module.EntryPoint == null)
                {
                    Console.WriteLine("Reset EntryPoint, Finding *.IMain2");
                }
                foreach (var type in module.GetTypes())
                {
                    if (type.IsPublic)
                    {
                        foreach (var m in type.Methods)
                        {
                            if (m.IsPublic && m.IsStatic)
                            {
                                if (m.Name.String == "IMain2")
                                {
                                    module.EntryPoint = m;
                                    Console.WriteLine("Set EntryPoint: " + m.DeclaringType.Name.String + "." + m.Name.String);
                                    return;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("No EntryPoint");
            }
        }
    }
}
