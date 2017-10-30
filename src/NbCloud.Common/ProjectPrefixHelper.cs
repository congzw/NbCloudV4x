using System.Linq;

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
    }
    
    /// <summary>
    /// 项目帮助类
    /// </summary>
    public class MyProjectHelper : IMyProjectHelper, IResolveAsSingleton
    {
        #region ioc helpers

        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static IMyProjectHelper Resolve()
        {
            return ResolveAsSingleton<MyProjectHelper, IMyProjectHelper>.Resolve();
        }

        #endregion

        private string _prefix = null;
        public string GetProjectPrefix()
        {
            if (_prefix != null)
            {
                return _prefix;
            }

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
    }
}
