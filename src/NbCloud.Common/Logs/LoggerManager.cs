using System;

namespace NbCloud.Common.Logs
{
    public class LoggerManager : ILoggerManager
    {
        #region for di extensions

        private static readonly Lazy<ILoggerManager> _loggerManagerLazy = new Lazy<ILoggerManager>(() => new LoggerManager());

        private static Func<ILoggerManager> _resolve = () => _loggerManagerLazy.Value;
        public static Func<ILoggerManager> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion
        
        public ILogger GetLogger(string name)
        {
            var logger = new Logger();
            if (!string.IsNullOrWhiteSpace(name))
            {
                logger.Category = name;
            }
            else
            {
                logger.Category = "";
            }
            return logger;
        }

        public ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        public ILogger GetLogger(Type type)
        {
            return GetLogger(type.Name);
        }

        public ILogger GetLogger()
        {
            return GetLogger("");
        }
    }
}