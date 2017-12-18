using System;

namespace NbCloud.Common.Logs.Log4Net
{
    public static class Log4NetConfig
    {
        public static readonly Lazy<ILoggerManager> LoggerManagerLazy = new Lazy<ILoggerManager>(() => new Log4NetLoggerManager());
        private static Func<ILoggerManager> _loggerManagerLazyBackup = null;
        
        public static void Enabled()
        {
            if (_loggerManagerLazyBackup == null)
            {
                _loggerManagerLazyBackup = LoggerManager.Resolve;
            }
            LoggerManager.Resolve = () => LoggerManagerLazy.Value;
        }
        public static void Disabled()
        {
            if (_loggerManagerLazyBackup != null)
            {
                _loggerManagerLazyBackup = LoggerManager.Resolve;
            }
        }
    }
}