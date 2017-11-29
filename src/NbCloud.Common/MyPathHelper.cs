using System;
using System.Reflection;
using NbCloud.Common.Collections.Extensions;

namespace NbCloud.Common
{
    public interface IMyPathHelper
    {
        /// <summary>
        /// 获取Bin目录
        /// </summary>
        /// <returns></returns>
        string GetBinDirectory();

        /// <summary>
        /// 合并路径
        /// c:\\test + a.b => c:\\test\\a.b
        /// c:\\test\\ + a.b => c:\\test\\a.b
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="subPath"></param>
        /// <returns></returns>
        string JoinPath(string basePath, string subPath);
    }

    public class MyPathHelper : IMyPathHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyPathHelper> _resolve = () => ResolveAsSingleton.Resolve<MyPathHelper, IMyPathHelper>();
        public static Func<IMyPathHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
        
        public string GetBinDirectory()
        {
            //xxx\bin => app
            //xxx => web
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory.IndexOf("\\bin", StringComparison.OrdinalIgnoreCase) != -1)
            {
                //非web不需要额外修正，是否更好的判断方案？ todo
                return baseDirectory;
            }

            string temp = FixDirPath(AppDomain.CurrentDomain.BaseDirectory);
            string dirPath = string.Format(@"{0}{1}", temp, "bin");
            return dirPath;
        }

        /// <summary>
        /// 连接路径
        /// c:\\test + a.b => c:\\test\\a.b
        /// c:\\test\\ + a.b => c:\\test\\a.b
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public string JoinPath(string basePath, string subPath)
        {
            string temp = FixDirPath(basePath);
            string fixedPath = string.Format(@"{0}{1}", temp, subPath);
            return fixedPath;
        }

        //如果后面没有\\，补上\\
        private static string FixDirPath(string value)
        {
            var temp = value.EndsWith("\\") ? value : value + "\\";
            return temp;
        }
    }
}