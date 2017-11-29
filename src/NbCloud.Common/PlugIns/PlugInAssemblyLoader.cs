using System.Collections.Generic;
using System.Reflection;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInAssemblyLoader
    {
        IList<Assembly> LoadAllPlugIns(IList<IPlugIn> plugIns);
    }

    public static class PlugInAssemblyLoader
    {
    }
}