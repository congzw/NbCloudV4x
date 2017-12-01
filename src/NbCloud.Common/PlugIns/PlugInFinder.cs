using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInFinder
    {
        //Func<string> PlugInBaseFolderResolver { get; set; }
        IList<IPlugIn> FindAllPlugIns();

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="plugIns"></param>
        /// <returns></returns>
        IList<Assembly> LoadAssemblies(params IPlugIn[] plugIns);
    }
    
    public class PlugInFinder : IPlugInFinder, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IPlugInFinder> _resolve = () => ResolveAsSingleton.Resolve<PlugInFinder, IPlugInFinder>();
        public static Func<IPlugInFinder> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public PlugInFinder()
        {
            PlugInBaseFolderResolver = () =>
            {
                var myPathHelper = MyPathHelper.Resolve();
                var binDirectory = myPathHelper.GetBinDirectory();
                return myPathHelper.JoinPath(binDirectory, "PlugIns");
            };
        }

        public Func<string> PlugInBaseFolderResolver { get; set; }

        public IList<IPlugIn> FindAllPlugIns()
        {
            var plugInBaseFolder = PlugInBaseFolderResolver();
            var descFilePaths = GetPlugInDescFilePaths(plugInBaseFolder);
            var plugIns = CreatePlugInFromDescFiles(descFilePaths.ToArray());
            return plugIns;
        }

        public IList<Assembly> LoadAssemblies(params IPlugIn[] plugIns)
        {
            throw new NotImplementedException();
            //var plugInBaseFolder = PlugInBaseFolderResolver();
            //var assemblyFolderPaths = GetPlugInFolderPaths(plugInBaseFolder);
            //foreach (var assemblyFolderPath in assemblyFolderPaths)
            //{
            //    var descFilePaths = GetPlugInDescFilePaths(assemblyFolderPath);
            //}
            //foreach (var plugIn in plugIns)
            //{

            //}

            //var searchOption = SearchOption.TopDirectoryOnly;
            //var assemblyFiles = Directory
            //    .EnumerateFiles(folderPath, "*.*", searchOption)
            //    .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"));

            //return assemblyFiles.Select(_assemblyLoader.Load).ToList();
        }

        private IList<IPlugIn> CreatePlugInFromDescFiles(params string[] descFilePaths)
        {
            var plugIns = new List<IPlugIn>();
            var myIniHelper = MyIniFileHelper.Resolve();
            foreach (var descFilePath in descFilePaths)
            {
                if (!File.Exists(descFilePath))
                {
                    throw new NbException("没有找到插件的描述文件：" + descFilePath);
                }
                var content = File.ReadAllText(descFilePath);
                var iniFlatContent = myIniHelper.LoadIniContentAsFlat(content);
                var code = iniFlatContent.GetItemValue("Code");
                var name = iniFlatContent.GetItemValue("Name");
                var version = iniFlatContent.GetItemValue("Version");
                var description = iniFlatContent.GetItemValue("Description");
                var plugIn = new PlugIn(code, name, version);
                plugIn.Description = description;
                plugIns.Add(plugIn);
            }
            return plugIns;
        }

        private IList<string> GetPlugInDescFilePaths(string plugInBaseFolder)
        {
            var searchOption = SearchOption.AllDirectories;
            var plugInDescFilePaths = Directory
                .GetFiles(plugInBaseFolder, "Description.txt", searchOption).ToList();
            return plugInDescFilePaths;
        }

        private IList<string> GetPlugInFolderPaths(string plugInBaseFolder)
        {
            var searchOption = SearchOption.TopDirectoryOnly;
            var plugInFolders = Directory
                .GetDirectories(plugInBaseFolder, "*", searchOption).ToList();
            return plugInFolders;
        }
    }

    public static class PlugInManagerExtensions
    {
        //public static IList<IPlugIn> FindAllPlugIns(this IPlugInManager plugInManager)
        //{
        //    var plugIns = PlugInFinder.Resolve().FindAllPlugIns();
        //    return plugIns;
        //}

        //public static void AutoRegisterPlugIns(this IPlugInManager plugInManager)
        //{
        //    //todo
        //    var plugIns = plugInManager.FindAllPlugIns();

        //    //todo filter by config before register
        //    foreach (var plugIn in plugIns)
        //    {
        //        plugInManager.Register(plugIn);
        //    }
        //}

        //public static IList<Assembly> LoadAllPlugIns(this IPlugInManager plugInManager)
        //{
        //    //todo filter by config before register
        //    var plugIns = PlugInFinder.Resolve().FindAllPlugIns();
        //    foreach (var plugIn in plugIns)
        //    {
        //        plugInManager.Register(plugIn);
        //    }
        //}
    }
}
