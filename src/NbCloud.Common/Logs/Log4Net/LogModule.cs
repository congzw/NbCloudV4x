using System;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;

namespace NbCloud.Common.Logs.Log4Net
{
    public class LogProvider : IProvider<ILogger>
    {
        #region IProvider 成员

        public object Create(IContext context)
        {
            Type scopeType = null;
            if (context.Request.ParentRequest != null)
            {
                scopeType = context.Request.ParentRequest.Service;
            }
            else
            {
                scopeType = typeof (LogProvider);
            }
            var loggerManager = context.Kernel.Get<ILoggerManager>();
            var logger = loggerManager.GetLogger(scopeType);
            return logger;
        }

        public Type Type
        {
            get { return typeof(ILoggerManager); }
        }

        #endregion
    }

    public class LogModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = this.Kernel;
            if (kernel != null)
            {
                kernel.Bind<ILoggerManager>().ToConstant(Log4NetConfig.LoggerManagerLazy.Value).InSingletonScope();
                kernel.Bind<ILogger>().ToProvider<LogProvider>();
            }
        }
    }
}
