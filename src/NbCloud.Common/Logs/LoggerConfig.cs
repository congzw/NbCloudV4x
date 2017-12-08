using System;
using NbCloud.Common.Extensions;

namespace NbCloud.Common.Logs
{
    public class LoggerConfig
    {
        #region for extensions

        private static readonly Lazy<LoggerConfig> LazyConfig = new Lazy<LoggerConfig>(() =>
        {
            var loggerConfig = new LoggerConfig
            {
                DefaultLoggerName = typeof(LoggerConfig).GetNamespacePrefix()
            };
            return loggerConfig;
        });

        private static Func<LoggerConfig> _resolve = () => LazyConfig.Value;
        public static Func<LoggerConfig> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        /// <summary>
        /// 默认的日志名称
        /// </summary>
        public string DefaultLoggerName { get; set; }
    }
}