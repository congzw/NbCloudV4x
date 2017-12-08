using System;
using NbCloud.Common.Extensions;

namespace NbCloud.Common.Logs
{
    public class Logger : ILogger
    {
        public Logger(string loggerName)
        {
            if (loggerName == null)
            {
                throw new ArgumentNullException("loggerName");
            }
            Name = loggerName;
        }

        public string Name { get; set; }

        public bool IsDebugEnabled
        {
            get { return true; }
        }
        public bool IsInfoEnabled
        {
            get { return true; }
        }
        public bool IsWarnEnabled
        {
            get { return true; }
        }
        public bool IsErrorEnabled
        {
            get { return true; }
        }
        public bool IsFatalEnabled
        {
            get { return true; }
        }

        public void Debug(object message)
        {
            LogMessage(message);
        }

        public void Info(object message)
        {
            LogMessage(message, LogLevel.Info);
        }

        public void Warn(object message)
        {
            LogMessage(message, LogLevel.Warn);
        }

        public void Error(object message)
        {
            LogMessage(message, LogLevel.Error);
        }

        public void Fatal(object message)
        {
            LogMessage(message, LogLevel.Fatal);
        }

        private void LogMessage(object message, LogLevel level = LogLevel.Debug)
        {
            var manager = LoggerManager.Resolve().Name;
            var prefix = string.Format("[{0,-5}][{1}][{2}][{3}] ", level, this.GetType().GetNamespacePrefix(), manager, Name);
            System.Diagnostics.Trace.WriteLine(prefix + message);
        }
    }
}