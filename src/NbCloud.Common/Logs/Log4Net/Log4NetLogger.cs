using System;
using log4net;

namespace NbCloud.Common.Logs.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        internal Log4NetLogger(ILog log, string loggerName)
        {
            _log = log;
            Name = loggerName;
        }

        public string Name { get; set; }
        public void Debug(object message)
        {
            _log.Debug(message);
        }
        public void Info(object message)
        {
            _log.Info(message);
        }
        public void Warn(object message)
        {
            _log.Warn(message);
        }
        public void Error(object message)
        {
            _log.Error(message);
        }
        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }
    }
}
