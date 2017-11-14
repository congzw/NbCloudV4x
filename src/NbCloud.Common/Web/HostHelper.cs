using System;
using System.Web.Hosting;

namespace NbCloud.Common.Web
{
    /// <summary>
    /// 宿主接口
    /// </summary>
    public interface IMyHostHelper
    {
        /// <summary>
        /// 获取主机的物理路径
        /// </summary>
        /// <param name="subFolder"></param>
        /// <returns></returns>
        string GetApplicationPhysicalPath(string subFolder = null);
        /// <summary>
        /// 获取应用的站点名称
        /// </summary>
        /// <returns></returns>
        string GetApplicationName();
        /// <summary>
        /// 获取应用部署的机器名
        /// </summary>
        /// <returns></returns>
        string GetMachineName();
    }

    /// <summary>
    /// 宿主帮助类
    /// </summary>
    public class MyHostHelper : IMyHostHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyHostHelper> _resolve = () => ResolveAsSingleton.Resolve<MyHostHelper, IMyHostHelper>();
        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static Func<IMyHostHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
        
        private static string _applicationPhysicalPath = null;
        public string GetApplicationPhysicalPath(string subFolder = null)
        {
            if (_applicationPhysicalPath == null)
            {
                _applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;
            }
            if (!string.IsNullOrWhiteSpace(subFolder))
            {
                return _applicationPhysicalPath + subFolder.TrimStart('\\').TrimEnd('\\') + "\\";
            }
            return _applicationPhysicalPath;
        }


        private static string _applicationName = null;
        public string GetApplicationName()
        {
            return _applicationName ?? (_applicationName = HostingEnvironment.SiteName);
        }

        private static string _machineName = null;
        public string GetMachineName()
        {
            return _machineName ?? (_machineName = System.Environment.MachineName);
        }
    }
}
