using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NbCloud.Common
{
    /// <summary>
    /// 项目帮助类
    /// </summary>
    public interface IMyProjectHelper
    {
        /// <summary>
        /// 获取项目前缀，例如NbLight
        /// </summary>
        /// <returns></returns>
        string GetProjectPrefix();

        /// <summary>
        /// 获取Bin目录
        /// </summary>
        /// <returns></returns>
        string GetBinDirectory();
    }
    
    /// <summary>
    /// 项目帮助类
    /// </summary>
    public class MyProjectHelper : IMyProjectHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyProjectHelper> _resolve = () => ResolveAsSingleton.Resolve<MyProjectHelper, IMyProjectHelper>();
        public static Func<IMyProjectHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        private string _prefix = null;
        public string GetProjectPrefix()
        {
            if (_prefix != null)
            {
                return _prefix;
            }

            //the only place hard code and will be replace at once!
            var prefix = "NbCloud";
            var ns = this.GetType().Namespace;
            if (ns != null)
            {
                var result = ns.Split('.').FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    prefix = result;
                }
            }
            _prefix = prefix;
            return _prefix;
        }

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

        //如果后面没有\\，补上\\
        private static string FixDirPath(string value)
        {
            var temp = value.EndsWith("\\") ? value : value + "\\";
            return temp;
        }
    }

    public static class MyProjectHelperExtension
    {
        public static IList<Assembly> LoadAppAssemblies(this IMyProjectHelper helper, string folderPath = null, string projectPrefix = null, string[] excludeFileNames = null)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = helper.GetBinDirectory();
            }

            if (string.IsNullOrWhiteSpace(projectPrefix))
            {
                projectPrefix = helper.GetProjectPrefix();
            }
            string patterns = string.Format("{0}.*.dll", projectPrefix);
            var assemblies = FindAssemlbiesInWebBinDirPath(folderPath, patterns, SearchOption.TopDirectoryOnly, excludeFileNames).ToList();
            return assemblies;
        }
        
        //从指定的路径下，加载程序集
        private static List<Assembly> FindAssemlbiesInWebBinDirPath(string folderPath, string match, SearchOption searchOption = SearchOption.TopDirectoryOnly, string[] excludeFiles = null)
        {
            if (string.IsNullOrWhiteSpace(match))
            {
                throw new ArgumentNullException("match");
            }

            var files = GetFiles(folderPath, match, searchOption);
            if (excludeFiles == null)
            {
                return Load(files);
            }
            var filesFix = files.Where(file => !excludeFiles.Any(x => file.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToList();
            return Load(filesFix);
        }

        //加载程序集
        private static List<Assembly> Load(IEnumerable<string> files)
        {
            var metaAssemblies = files.Select(Assembly.LoadFrom).ToList();
            return metaAssemblies;
        }

        //查找所有文件
        private static List<string> GetFiles(string folderPath, string searchPattern, SearchOption searchOption)
        {
            List<string> list = new List<string>();
            string[] temp = Directory.GetFiles(folderPath, searchPattern, searchOption);
            if (temp.Length > 0)
            {
                list.AddRange(temp);
            }
            return list;
        }
    }
}
