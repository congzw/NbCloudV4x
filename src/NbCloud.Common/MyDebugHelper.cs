using System;
using System.Configuration;
using System.Web;

namespace NbCloud.Common
{
    public interface IMyDebugHelper : ISingletonDependency
    {
        /// <summary>
        /// 是否是调试模式
        /// </summary>
        /// <param name="detectHttpRequest"></param>
        /// <returns></returns>
        bool IsDebugMode(bool detectHttpRequest = false);

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="detectHttpRequest"></param>
        void LogDebugMessage(string message, string category = null, bool detectHttpRequest = false);
    }

    public class MyDebugHelper : IMyDebugHelper, IResolveAsSingleton
    {
        private string Config_Common_DebugMode = "Config.Common.DebugMode";
        private bool? _isDebugModeInConfig = null;

        public bool IsDebugMode(bool detectHttpRequest = false)
        {
            if (!_isDebugModeInConfig.HasValue)
            {
                bool valueInConfig = false;
                //如果后台有设置，以config的设置为准
                string settingValue = ConfigurationManager.AppSettings[Config_Common_DebugMode];
                if (!string.IsNullOrWhiteSpace(settingValue))
                {
                    bool.TryParse(settingValue, out valueInConfig);
                }

                _isDebugModeInConfig = valueInConfig;
            }

            if (_isDebugModeInConfig.Value)
            {
                return true;
            }

            //不需要额外检测Url参数
            if (!detectHttpRequest)
            {
                return _isDebugModeInConfig.Value;
            }

            return IsDebugInRequest();
        }

        public void LogDebugMessage(string message, string category = null, bool detectHttpRequest = false)
        {
            if (IsDebugMode(detectHttpRequest))
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    UtilsLogger.LogMessage(string.Format("{0}", message));
                    return;
                }
                UtilsLogger.LogMessage(string.Format("[{0}] > {1}", category, message));
            }
        }

        private bool IsDebugInRequest()
        {
            //如果没有启用，侦测url
            if (HttpContext.Current == null)
            {
                return false;
            }
            var value = HttpContext.Current.Request.Params.Get("debug");
            var debug = "true".Equals(value, StringComparison.OrdinalIgnoreCase);
            return debug;
        }

        #region for ioc extensions

        private static Func<IMyDebugHelper> _resolve = () => ResolveAsSingleton.Resolve<MyDebugHelper, IMyDebugHelper>();
        public static Func<IMyDebugHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
    }
}
