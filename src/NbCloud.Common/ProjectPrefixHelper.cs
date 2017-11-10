using System;
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
    }


}
