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

        //todo
        //void Info(object message);
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

        #region ioc helpers
        
        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static IMyLogHelper Resolve()
        {
            return ResolveAsSingleton<MyLogHelper, IMyLogHelper>.Resolve();
        }

        #endregion
    }
}
