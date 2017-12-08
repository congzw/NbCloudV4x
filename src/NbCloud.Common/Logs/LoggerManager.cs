using System;

namespace NbCloud.Common.Logs
{
    public class LoggerManager : ILoggerManager
    {
        #region for extensions

        private static readonly Lazy<ILoggerManager> LoggerManagerLazy = new Lazy<ILoggerManager>(() => new LoggerManager());

        private static Func<ILoggerManager> _resolve = () => LoggerManagerLazy.Value;
        public static Func<ILoggerManager> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        public LoggerManager()
        {
            Name = "Default";
        }
        public string Name { get; set; }

        public ILogger GetLogger(string name)
        {
            var logger = new Logger(!string.IsNullOrWhiteSpace(name) ? name : LoggerConfig.Resolve().DefaultLoggerName);
            return logger;
        }
        public ILogger GetLogger(Type type)
        {
            return GetLogger(type.Name);
        }
    }
}