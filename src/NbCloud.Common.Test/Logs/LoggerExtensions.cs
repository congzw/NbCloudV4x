using System;
using System.Globalization;

namespace NbCloud.Common.Logs
{
    public static class LoggerExtensions
    {
        public static void LogSelfInvoke(this ILogger logger)
        {
            var invokeMethodBase = StackTraceHelper.GetInvokeMethodBase(2);
            logger.Debug(invokeMethodBase.Name);
            logger.Info(invokeMethodBase.Name);
            logger.Warn(invokeMethodBase.Name);
            logger.Error(invokeMethodBase.Name);
            logger.Fatal(invokeMethodBase.Name);

            logger.Debug("DebugFormat > " + invokeMethodBase.Name, null);
            logger.Debug("DebugFormat > " + invokeMethodBase.Name, new NbException("Foo"));
            logger.DebugFormat("DebugFormat > {0}", invokeMethodBase.Name);
            logger.DebugFormat(new CultureInfo("en-US"), "DebugFormat > {0} > {1}", invokeMethodBase.Name, DateTime.Now);
            logger.DebugFormat(new CultureInfo("zh-CN"), "DebugFormat > {0} > {1}", invokeMethodBase.Name, DateTime.Now);
        }
    }
}
