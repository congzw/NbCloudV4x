using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NbCloud.Common.PlugIns
{
    internal class FolderPlugInAssemblyHelper
    {
        public IList<Assembly> LoadAssemblies(IPlugIn plugIn)
        {
            var folderPath = GetFolderName(plugIn);

            var searchOption = SearchOption.TopDirectoryOnly;
            var assemblyFiles = Directory
                .EnumerateFiles(folderPath, "*.*", searchOption)
                .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));

            return assemblyFiles.Select(_assemblyLoader.Load).ToList();
        }

        public string GetFolderPath(IPlugIn plugIn)
        {
            var folderName = GetFolderName(plugIn);
            var binDirectory = _myPathHelper.GetBinDirectory();
            return _myPathHelper.JoinPath(binDirectory, folderName);
        }

        private string GetFolderName(IPlugIn plugIn)
        {
            return string.Format("{0}-{1}", plugIn.Code, plugIn.Version);
        }

        private readonly IMyPathHelper _myPathHelper = MyPathHelper.Resolve();
        private readonly IAssemblyLoader _assemblyLoader = AssemblyLoader.Resolve();
    }
}