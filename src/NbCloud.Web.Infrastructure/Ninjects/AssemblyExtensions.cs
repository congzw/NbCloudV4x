using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

namespace NbCloud.Web.Ninjects
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> GetNinjectModules(this Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(IsLoadableModule)
                .Select(type => Activator.CreateInstance(type) as INinjectModule);
        }

        private static bool IsLoadableModule(Type type)
        {
            return typeof(INinjectModule).IsAssignableFrom(type)
                && !type.IsAbstract
                && !type.IsInterface
                && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}
