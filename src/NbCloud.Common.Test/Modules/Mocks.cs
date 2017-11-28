using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NbCloud.Common.Extensions;

namespace NbCloud.Common.Modules
{
    [DependsOn(typeof(ModuleB), typeof(ModuleC))]
    public class ModuleA : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }

    [DependsOn(typeof(ModuleC))]
    public class ModuleB : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }

    public class ModuleC : NbModule
    {
        public override Assembly[] GetAdditionalAssemblies()
        {
            return new[] { typeof(TestMethodAttribute).GetAssembly() };
        }
    }
    
    public class ModuleMain : NbModule
    {
        
    }
}
