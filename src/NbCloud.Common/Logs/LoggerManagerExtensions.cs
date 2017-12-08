namespace NbCloud.Common.Logs
{
    public static class LoggerManagerExtensions
    {
        public static ILogger GetLogger<T>(this ILoggerManager loggerManager)
        {
            return loggerManager.GetLogger(typeof(T));
        }

        public static ILogger GetLogger(this ILoggerManager loggerManager)
        {
            var defaultLoggerName = LoggerConfig.Resolve().DefaultLoggerName;
            return loggerManager.GetLogger(defaultLoggerName);
        }
    }
}