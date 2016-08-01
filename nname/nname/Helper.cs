using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nname
{
    static class Helper
    {
        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static HashSet<String> GetBaseNames(TypeDef tdef)
        {

            var list = GetBaseTypes(tdef);
            //var hs = list.ToHashSet(tdef.DefinitionAssembly);

            HashSet<String> r = new HashSet<string>();
            foreach (var d in list)
            {
                if (d.DefinitionAssembly != tdef.DefinitionAssembly)
                {
                    foreach (var n1 in d.Properties)
                    {
                        r.Add(n1.Name);
                    }
                    foreach (var n2 in d.Methods)
                    {
                        r.Add(n2.Name);
                    }
                }
                else
                {
                    if (RType.IsPublic(d))
                    {
                        foreach (var n1 in d.Properties)
                        {
                            r.Add(n1.Name);
                        }
                        foreach (var n2 in d.Methods)
                        {
                            if (n2.IsFamily || n2.IsPublic)
                            {
                                r.Add(n2.Name);
                            }
                        }
                    }
                }
            }

            return r;
        }
        private static HashSet<T> ToHashSet<T>(this IList<T> self) where T : IType
        {
            HashSet<T> r = new HashSet<T>();
            foreach (var t in self)
            {
                r.Add(t);
            }
            return r;
        }
        private static List<TypeDef> GetBaseTypes(ITypeDefOrRef type)
        {
            List<TypeDef> r = new List<TypeDef>();
            if (type == null)
            {
                return r;
            }


            if (type is TypeDef)
            {
                r.Add((TypeDef)type);
                r.AddRange(GetBaseTypes(((TypeDef)type).BaseType));
            }
            else if (type is TypeRef)
            {
                var t = ((TypeRef)type).Resolve();
                r.Add(t);
                r.AddRange(GetBaseTypes(t.BaseType));
            }
            else if (type is TypeSpec)
            {
                TypeSig ts = ((TypeSpec)type).TypeSig;
                GenericInstSig gts = (GenericInstSig)ts;
                var td = gts.GenericType.TypeDefOrRef;
                r.AddRange(GetBaseTypes(td));
            }

            foreach (var t in r.ToArray())
            {
                foreach (var i in t.Interfaces)
                {
                    r.AddRange(GetBaseTypes(i.Interface));
                }
            }

            return r;

        }


    }
}
