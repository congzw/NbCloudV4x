using System;
using System.IO;

namespace NbCloud.Common.Logs.Log4Net
{
    public class Log4NetLoggerManager : ILoggerManager
    {
        public Log4NetLoggerManager()
        {
            Name = "Log4Net";
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        public string Name { get; set; }

        public ILogger GetLogger(string name)
        {
            var nameFix = !string.IsNullOrWhiteSpace(name) ? name : LoggerConfig.Resolve().DefaultLoggerName;
            var logger = log4net.LogManager.GetLogger(nameFix);
            var log4NetLogger = new Log4NetLogger(logger,nameFix);
            return log4NetLogger;
        }
        
        public ILogger GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            var log4NetLogger = new Log4NetLogger(logger, type.Name);
            return log4NetLogger;
        }
    }
}