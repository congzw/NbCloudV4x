using System;
using System.IO;

namespace NbCloud.Common.Logs.Log4Net
{
    public class Log4NetLoggerManager : ILoggerManager
    {
        public Log4NetLoggerManager()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }
        
        public ILogger GetLogger<T>()
        {
            var type = typeof (T);
            var logger = log4net.LogManager.GetLogger(type);
            var log4NetLogger = new Log4NetLogger(logger);
            log4NetLogger.Category = type.Name;
            return log4NetLogger;
        }

        public ILogger GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            var log4NetLogger = new Log4NetLogger(logger);
            log4NetLogger.Category = type.Name;
            return log4NetLogger;
        }

        public ILogger GetLogger(string name)
        {
            //fix prefix
            var nameWithPrefix = string.IsNullOrWhiteSpace(name) ? PrefixHelper.Prefix : PrefixHelper.Prefix + "." + name;
            var logger = log4net.LogManager.GetLogger(nameWithPrefix);
            var log4NetLogger = new Log4NetLogger(logger);
            log4NetLogger.Category = name;
            return log4NetLogger;
        }

        public ILogger GetLogger()
        {
            return GetLogger("");
        }
    }
}