using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nname
{
    class RProperty
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

            if (!RType.IsPublic(type))
            {
                foreach (var p in type.Properties)
                {
                    if (!IsSpecialName(p, baseNames))
                    {
                        var nn = GetPropertyName(p);
                        Helper.WriteLine(type.Name + "." + p.Name + " -> " + nn);
                        p.Name = nn;
                    }
                }
            }
        }


        Dictionary<string, string> dict = new Dictionary<string, string>();
        private string GetPropertyName(PropertyDef pro)
        {
            string nn;
            if (dict.TryGetValue(pro.Name, out nn))
            {
                return nn;
            }
            nn = "p" + (dict.Count + 1);
            dict.Add(pro.Name, nn);
            return nn;
        }

        private static bool IsSpecialName(PropertyDef pro, HashSet<string> pubNames)
        {
            var pn = pro.Name;
            return pro.IsSpecialName || pro.IsRuntimeSpecialName ||
                pn.Contains(".") ||
                pn.Equals("Item") ||
                pubNames.Contains(pn);

        }


    }
}
