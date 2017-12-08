using System;

namespace NbCloud.Common.Logs
{
    //遗留接口改造
    public interface IMyLogHelper
    {
        void Debug(object message);
        void Debug(Type type, object message);
    }

    public class MyLogHelper : IMyLogHelper, IResolveAsSingleton
    {
        #region for di extensions

        private static Func<IMyLogHelper> _resolve = () => ResolveAsSingleton.Resolve<MyLogHelper, IMyLogHelper>();

        public static Func<IMyLogHelper> Resolve
        {
            get { return _resolve; }
            set { _resolve = value; }
        }

        #endregion

        private Func<ILoggerManager> _loggerManagerFunc = LoggerManager.Resolve;
        public Func<ILoggerManager> LoggerManagerFunc
        {
            get { return _loggerManagerFunc; }
            set { _loggerManagerFunc = value; }
        }

        public void Debug(object message)
        {
            LoggerManagerFunc().GetLogger(this.GetType()).Debug(message);
        }

        public void Debug(Type type, object message)
        {
            LoggerManagerFunc().GetLogger(type).Debug(message);
        }
    }
}
