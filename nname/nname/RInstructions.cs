using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;

namespace nname
{
    class RInstructions
    {
        class RefToDef<R, F>
        {
            public R Ref;
            public F Def;
        }
        private ModuleDefMD module;

        private List<RefToDef<MemberRef, FieldDef>> f2d = new List<RefToDef<MemberRef, FieldDef>>();
        private List<RefToDef<MemberRef, MethodDef>> m2d = new List<RefToDef<MemberRef, MethodDef>>();
        private List<RefToDef<MethodSpec, MethodDef>> m2d2 = new List<RefToDef<MethodSpec, MethodDef>>();

        public RInstructions(ModuleDefMD module)
        {
            this.module = module;
            foreach (var type in module.GetTypes())
            {
                foreach (var m in type.Methods)
                {
                    if (m.Body == null) { continue; }
                    foreach (var mi in m.Body.Instructions)
                    {
                        if (mi != null && mi.Operand is MemberRef)
                        {
                            MemberRef mr = (MemberRef)mi.Operand;
                            if (mr.IsFieldRef)
                            {
                                f2d.Add(new RefToDef<MemberRef, FieldDef> { Ref = mr, Def = mr.ResolveField() });
                            }
                            else if (mr.IsMethodRef)
                            {
                                m2d.Add(new RefToDef<MemberRef, MethodDef> { Ref = mr, Def = mr.ResolveMethod() });
                            }
                        }
                        if (mi != null && mi.Operand is MethodSpec)
                        {
                            MethodSpec ms = (MethodSpec)mi.Operand;
                            m2d2.Add(new RefToDef<MethodSpec, MethodDef> { Ref = ms, Def = ms.ResolveMethodDef() });
                        }
                    }
                }
            }
        }

        internal void Apply()
        {
            foreach (var e in f2d)
            {
                e.Ref.Name = e.Def.Name;
            }
            foreach (var e in m2d)
            {
                e.Ref.Name = e.Def.Name;
            }
            foreach (var e in m2d2)
            {
                e.Ref.Name = e.Def.Name;
            }
        }
    }
}
