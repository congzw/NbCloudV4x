using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NbCloud.Common.PlugIns
{
    public interface IPlugInFinder
    {
        Func<string> PlugInBaseFolderResolver { get; set; }
        IList<IPlugIn> FindAllPlugIns();
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
            var plugIns = new List<IPlugIn>();
            var plugInBaseFolder = PlugInBaseFolderResolver();
            var plugInFolders = GetPlugInFolders(plugInBaseFolder);
            foreach (var plugInFolder in plugInFolders)
            {
                //find meta file: desc.json
                //ini?
                //plugIns.Add(new PlugIn());
            }
            throw new NotImplementedException();

        }

        private static IList<string> GetPlugInFolders(string plugInBaseFolder)
        {
            var searchOption = SearchOption.TopDirectoryOnly;
            var plugInFolders = Directory
                .EnumerateDirectories(plugInBaseFolder, "*.*", searchOption).ToList();
            return plugInFolders;
        }
    }

    public static class PlugInManagerExtensions
    {
        public static IList<IPlugIn> FindAllPlugIns(this IPlugInManager plugInManager)
        {
            //todo filter by config before register
            var plugIns = PlugInFinder.Resolve().FindAllPlugIns();
            return plugIns;
        }

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
