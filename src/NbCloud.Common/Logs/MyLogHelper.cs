using System;

namespace NbCloud.Common.Logs
{
    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogLevel
    {
        Off = 0,
        Fatal = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 5,
        All = 6
    }

    public interface IMyLogHelper
    {
        /// <summary>
        /// 获取当前配置的级别
        /// </summary>
        /// <returns></returns>
        LogLevel GetLogLevel();

        void Debug(object message);

        void Info(object message);
        //todo
        //void Warn(object message);
        //void Error(object message);
        //void Fatal(object message);

        //void DebugFormat(string message, params object[] args);
        //void InfoFormat(string message, params object[] args);
        //void WarnFormat(string message, params object[] args);
        //void ErrorFormat(string message, params object[] args);
        //void FatalFormat(string message, params object[] args);
    }

    public class MyLogHelper : IMyLogHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyLogHelper> _resolve = () => ResolveAsSingleton.Resolve<MyLogHelper, IMyLogHelper>();
        public static Func<IMyLogHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public LogLevel GetLogLevel()
        {
            //todo adaptee by log4net & config
            return LogLevel.Debug;
        }

        public void Debug(object message)
        {
            //todo adaptee by log4net & config
            UtilsLogger.LogMessage(message.ToString());
        }

        public void Info(object message)
        {
            //todo adaptee by log4net & config
            UtilsLogger.LogMessage(message.ToString());
        }
    }

    public static class MyLogHelperExtensions
    {
        public static void Debug(this IMyLogHelper helper, Type type, object message)
        {
            helper.Debug(string.Format("[{0}] => {1}", type.Name, message));
        }
    }
}
