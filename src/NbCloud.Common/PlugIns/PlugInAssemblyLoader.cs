using System;
using System.Collections.Generic;
using System.Reflection;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInAssemblyLoader
    {
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="plugIns"></param>
        /// <returns></returns>
        IList<Assembly> LoadAssemblies(params IPlugIn[] plugIns);
    }

    public class PlugInAssemblyLoader : IPlugInAssemblyLoader, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IPlugInAssemblyLoader> _resolve = () => ResolveAsSingleton.Resolve<PlugInAssemblyLoader, IPlugInAssemblyLoader>();
        public static Func<IPlugInAssemblyLoader> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public IList<Assembly> LoadAssemblies(params IPlugIn[] plugIns)
        {
            throw new NotImplementedException();
        }
    }
}