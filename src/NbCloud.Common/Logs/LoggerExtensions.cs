using System;

namespace NbCloud.Common.Logs
{
    public static class LoggerExtensions
    {
        public static void Debug(ILogger logger, object message, Exception exception)
        {
            logger.Debug(message + exception.StackTrace);
        }

        public static void DebugFormat(ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Debug(message);
        }

        public static void DebugFormat(ILogger logger, string format, object arg0)
        {
            string message = string.Format(format, arg0);
            logger.Debug(message);
        }

        public static void DebugFormat(ILogger logger, string format, object arg0, object arg1)
        {
            string message = string.Format(format, arg0, arg1);
            logger.Debug(message);
        }

        public static void DebugFormat(ILogger logger, string format, object arg0, object arg1, object arg2)
        {
            string message = string.Format(format, arg0, arg1, arg2);
            logger.Debug(message);
        }

        public static void DebugFormat(ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Debug(message);
        }
    }
}