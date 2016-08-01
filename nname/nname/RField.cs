using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nname
{
    class RField
    {
        public void Apply(ModuleDefMD module)
        {
            foreach (var type in module.GetTypes())
            {
                Apply(module, type);

            }
        }

        private void Apply(ModuleDefMD module, TypeDef type)
        {
            var ispub = RType.IsPublic(type);

            foreach (var m in type.Fields)
            {
                if (m.Name.Contains("__")) { continue; }
                if (m.IsPrivate || (m.IsAssembly && (!m.IsFamily)) || (!ispub))
                {
                    fcount++;
                    m.Name = "f" + fcount;
                }
            }
        }

        long fcount = 0;

    }
}
