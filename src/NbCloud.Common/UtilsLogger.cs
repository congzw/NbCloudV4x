using System;
using System.Configuration;
using System.Web;

namespace NbCloud.Common
{
    //类库内部使用的日志组件
    /// <summary>
    /// 类库内部使用的日志组件
    /// （不依赖Log4Net，默认System.Diagnostics.Trace，支持运行时替换）
    /// </summary>
    public class UtilsLogger
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message"></param>
        public static void LogMessage(string message)
        {
            _defaultLogAction.Invoke(message);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="catelog"></param>
        /// <param name="message"></param>
        public static void LogMessage(string catelog, string message)
        {
            _defaultLogAction.Invoke(string.Format("[{0}] {1}", catelog, message));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void LogMessage(Type type, string message)
        {
            _defaultLogAction.Invoke(string.Format("[{0}] {1}", type.Name, message));
        }
        
        /// <summary>
        /// 重新设置日志打印方式
        /// </summary>
        /// <param name="action"></param>
        public static void SetLogFunc(Action<string> action)
        {
            if (action != null)
            {
                _defaultLogAction = action;
            }
        }
        
        private static Action<string> _defaultLogAction = new Action<string>(TraceMessage);
        private static string _prefix = null;
        private static void TraceMessage(string message)
        {
            if (_prefix == null)
            {
                _prefix = GetPrefix();
            }
            System.Diagnostics.Trace.WriteLine(_prefix + message);
        }

        private static string Config_Common_LogPrefix = "Config.Common.LogPrefix";
        private static string GetPrefix()
        {
            //如果后台有设置，以config的设置为准
            var projectPrefix = MyProjectHelper.Resolve().GetProjectPrefix();
            string prefixSeed = string.Format("[{0}][Dev][UtilsLogger]", projectPrefix);
            string preFix = ConfigurationManager.AppSettings[Config_Common_LogPrefix];
            if (!string.IsNullOrWhiteSpace(preFix))
            {
                prefixSeed = prefixSeed.Replace(projectPrefix, preFix);
                //fix config [xxx]
                prefixSeed = string.Format("[{0}]", prefixSeed).Replace("[[", "[").Replace("]]", "]");
            }
            return prefixSeed + " => ";
        }
    }
}
