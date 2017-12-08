//using System;

//namespace NbCloud.Common.Logs
//{
//    public class Logger : ILogger
//    {
//        public Logger()
//        {
//            Adapter = "Default";
//            Prefix = PrefixHelper.Prefix;
//        }

//        public string Prefix { get; set; }
//        public string Adapter { get; set; }
//        public string Name { get; set; }

//        public void Debug(object message)
//        {
//            LogMessage(message);
//        }

//        public void Debug(object message, Exception exception)
//        {
//            LogMessage(message);
//        }

//        public void DebugFormat(string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void DebugFormat(string format, object arg0)
//        {
//            string message = string.Format(format, arg0);
//            LogMessage(message);
//        }

//        public void DebugFormat(string format, object arg0, object arg1)
//        {
//            string message = string.Format(format, arg0, arg1);
//            LogMessage(message);
//        }

//        public void DebugFormat(string format, object arg0, object arg1, object arg2)
//        {
//            string message = string.Format(format, arg0, arg1, arg2);
//            LogMessage(message);
//        }

//        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void Info(object message)
//        {
//            LogMessage(message);
//        }

//        public void Info(object message, Exception exception)
//        {
//            LogMessage(message);
//        }

//        public void InfoFormat(string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void InfoFormat(string format, object arg0)
//        {
//            string message = string.Format(format, arg0);
//            LogMessage(message);
//        }

//        public void InfoFormat(string format, object arg0, object arg1)
//        {
//            string message = string.Format(format, arg0, arg1);
//            LogMessage(message);
//        }

//        public void InfoFormat(string format, object arg0, object arg1, object arg2)
//        {
//            string message = string.Format(format, arg0, arg1, arg2);
//            LogMessage(message);
//        }

//        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }
//        public void Warn(object message)
//        {
//            LogMessage(message);
//        }

//        public void Warn(object message, Exception exception)
//        {
//            LogMessage(message);
//        }

//        public void WarnFormat(string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void WarnFormat(string format, object arg0)
//        {
//            string message = string.Format(format, arg0);
//            LogMessage(message);
//        }

//        public void WarnFormat(string format, object arg0, object arg1)
//        {
//            string message = string.Format(format, arg0, arg1);
//            LogMessage(message);
//        }

//        public void WarnFormat(string format, object arg0, object arg1, object arg2)
//        {
//            string message = string.Format(format, arg0, arg1, arg2);
//            LogMessage(message);
//        }

//        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void Error(object message)
//        {
//            LogMessage(message);
//        }

//        public void Error(object message, Exception exception)
//        {
//            LogMessage(message);
//        }

//        public void ErrorFormat(string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void ErrorFormat(string format, object arg0)
//        {
//            string message = string.Format(format, arg0);
//            LogMessage(message);
//        }

//        public void ErrorFormat(string format, object arg0, object arg1)
//        {
//            string message = string.Format(format, arg0, arg1);
//            LogMessage(message);
//        }

//        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
//        {
//            string message = string.Format(format, arg0, arg1, arg2);
//            LogMessage(message);
//        }

//        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void Fatal(object message)
//        {
//            LogMessage(message);
//        }

//        public void Fatal(object message, Exception exception)
//        {
//            LogMessage(message);
//        }

//        public void FatalFormat(string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public void FatalFormat(string format, object arg0)
//        {
//            string message = string.Format(format, arg0);
//            LogMessage(message);
//        }

//        public void FatalFormat(string format, object arg0, object arg1)
//        {
//            string message = string.Format(format, arg0, arg1);
//            LogMessage(message);
//        }

//        public void FatalFormat(string format, object arg0, object arg1, object arg2)
//        {
//            string message = string.Format(format, arg0, arg1, arg2);
//            LogMessage(message);
//        }

//        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            LogMessage(message);
//        }

//        public bool IsDebugEnabled {
//            get { return true; }
//        }
//        public bool IsInfoEnabled
//        {
//            get { return true; }
//        }
//        public bool IsWarnEnabled
//        {
//            get { return true; }
//        }
//        public bool IsErrorEnabled
//        {
//            get { return true; }
//        }
//        public bool IsFatalEnabled
//        {
//            get { return true; }
//        }

//        private void LogMessage(object message)
//        {
//            //todo filter by config
//            var prefix = string.Format("[{0}][{1}][{2}] ", Prefix, Adapter, Name);
//            System.Diagnostics.Trace.WriteLine(prefix + message);
//        }

//    }
//}