using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nname
{
    class RMethod
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
            var baseNames = Helper.GetBaseNames(type);


            foreach (var m in type.Methods)
            {
                if (!IsSpecialName(m, baseNames))
                {
                    var nn = GetMethodyName(m);
                    Helper.WriteLine(type.Name + "." + m.Name + " -> " + nn);
                    m.Name = nn;
                    int pc = 1;
                    foreach (var p in m.ParamDefs)
                    {
                        p.Name = "p" + pc;
                        pc++;
                    }
                }
            }

        }


        Dictionary<string, string> dict = new Dictionary<string, string>();
        private string GetMethodyName(MethodDef m)
        {
            string nn;
            if (dict.TryGetValue(m.Name, out nn))
            {
                return nn;
            }
            nn = "m" + (dict.Count + 1);
            dict.Add(m.Name, nn);
            return nn;
        }

        private static bool IsSpecialName(MethodDef m, HashSet<string> pubNames)
        {
            var pn = m.Name;
            bool isPN = m.IsSpecialName || m.IsRuntimeSpecialName ||
                pn.Contains(".") ||
                pn.Contains("_Item") || pn.Contains("Invoke") ||
                pubNames.Contains(pn);
            if (isPN) { return true; }

            var typePub = RType.IsPublic(m.DeclaringType);
            if (typePub)
            {
                if (m.IsFamily) { return true; }// protected
                if (m.IsPublic) { return true; }// Public
            }

            return false;
        }

    }
}
