using System;

namespace NbCloud.Common.Logs
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger logger, object message, Exception exception)
        {
            if (exception == null)
            {
                logger.Debug(message + " [Exception]: null");
                return;
            }
            logger.Debug(message + " [Exception]: " + exception.Message + " [StackTrace]: " + exception.StackTrace);
        }

        public static void DebugFormat(this ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Debug(message);
        }

        public static void DebugFormat(this ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            logger.Debug(message);
        }

        public static void Info(this ILogger logger, object message, Exception exception)
        {
            logger.Info(message + exception.StackTrace);
        }

        public static void InfoFormat(this ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Info(message);
        }

        public static void InfoFormat(this ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            logger.Info(message);
        }

        
        public static void Warn(this ILogger logger, object message, Exception exception)
        {
            logger.Warn(message + exception.StackTrace);
        }

        public static void WarnFormat(this ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Warn(message);
        }

        public static void WarnFormat(this ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            logger.Warn(message);
        }

        public static void Error(this ILogger logger, object message, Exception exception)
        {
            logger.Error(message + exception.StackTrace);
        }

        public static void ErrorFormat(this ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Error(message);
        }

        public static void ErrorFormat(this ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            logger.Error(message);
        }
        

        public static void Fatal(this ILogger logger, object message, Exception exception)
        {
            logger.Fatal(message + exception.StackTrace);
        }

        public static void FatalFormat(this ILogger logger, string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.Fatal(message);
        }

        public static void FatalFormat(this ILogger logger, IFormatProvider provider, string format, params object[] args)
        {
            string message = string.Format(provider, format, args);
            logger.Fatal(message);
        }
    }
}