using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nname
{
    class RType
    {
        string ns = "iBoxDB.ByteCodes";


        public void Apply(ModuleDefMD module)
        {
            foreach (var type in module.GetTypes())
            {
                if (!IsPublic(type))
                {
                    if (type.Namespace.Length > 1)
                    {
                        type.Namespace = ns;
                    }
                    type.Name = GetClassName(type);
                }
            }
        }

        internal static bool IsPublic(TypeDef type)
        {
            if (type.DeclaringType == null)
            {
                return type.IsPublic || type.Name.Contains("<") || (type.IsValueType && (!type.IsEnum));
            }
            else
            {
                return (type.IsNestedPublic || type.Name.Contains("<") || (type.IsValueType && (!type.IsEnum)))
                    && IsPublic(type.DeclaringType);
            }
        }

        private static int classCount = 1;
        private static string GetClassName(TypeDef type)
        {
            if (type.IsInterface) { return "I" + +classCount++; }
            if (type.IsEnum) { return "E" + +classCount++; }
            if (type.IsDelegate) { return "D" + +classCount++; }
            return "C" + classCount++;
        }
    }
}
