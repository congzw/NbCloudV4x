using System;
using System.Configuration;
using System.Web;
using NbCloud.Common.Ioc;

namespace NbCloud.Common
{
    public interface IMyDebugHelper : ISingletonDependency
    {
        bool IsDebugMode(bool detectHttpRequest = false);
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

        private static bool IsDebugInRequest()
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
        
        private static Func<IMyDebugHelper> _resolve = () => CoreServiceProvider.LocateService<IMyDebugHelper>() ??
                                                             ResolveAsSingleton<MyDebugHelper, IMyDebugHelper>.Resolve();
        public static Func<IMyDebugHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }
    }
}
