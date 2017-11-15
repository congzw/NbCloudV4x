using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using NbCloud.Common;
using NbCloud.Common.AmbientScopes.Ninjects;
using NbCloud.Common.Data;
using NbCloud.Common.Data.Provider.Nhibernate;
using NbCloud.Common.Logs;
using NbCloud.Common.NHibernates;
using NHibernate;
using Ninject;
using Ninject.Modules;

namespace NbCloud.Web.Ninjects
{
    /// <summary>
    /// 默认的标准约定，不需要定制的部分
    /// 为了方便其他Module进行定制(例如Rebind)，确保首先加载此NinjectModule
    /// </summary>
    internal class NinjectModuleForConvention : NinjectModule
    {
        public override void Load()
        {
            var kernel = this.Kernel;
            kernel.SetupAmbientScope();
            SetupForCommon(kernel);
            SetupForNhibernate(kernel);
            var findAppAssemblies = FindAppAssemblies();
            SetupForConvention(findAppAssemblies, kernel);
            SetupForWeb(kernel);
        }

        private void SetupForCommon(IKernel kernel)
        {
            ////read from config
            //var enabled = MockDateHelper.Enabled();
            //if (enabled)
            //{

            //    MockDateHelper.Init();
            //    kernel.Bind<IMyDateHelper>().To<MockDateHelper>().InSingletonScope();
            //    ResolveAsSingleton<MyDateHelper, IMyDateHelper>.SetFactoryFunc(() => kernel.Get<IMyDateHelper>());
            //}
        }

        private void SetupForNhibernate(IKernel kernel)
        {
            var mySessionFactory = MySessionFactory.Resolve();
            kernel.Bind<IMySessionFactory>().ToConstant(mySessionFactory).InSingletonScope();

            //session
            NhRepositoryContext.ShouldReleaseByHand = false; //let ninject do the jobs
            kernel.Bind<ISession>().ToMethod(x => GetSession(kernel)).InAmbientOrRequestScope()
                .OnActivation(OnActivationSessionLog).OnDeactivation(OnDeactivationSessionLog);
            kernel.Bind<IRepositoryContext>().To<NhRepositoryContext>().InScope(ctx => ctx.Kernel.Get<ISession>())
                .OnActivation(OnActivationUowLog).OnDeactivation(OnDeactivationUowLog);

            kernel.Bind<ITransactionManager>().ToMethod(ctx => ctx.Kernel.Get<IRepositoryContext>());
            kernel.Bind(typeof(IRepository<,>)).To(typeof(NhRepository<,>)).InScope(ctx => ctx.Kernel.Get<IRepositoryContext>());
        }
        
        private ISession GetSession(IKernel kernel)
        {
            #region todo session provider

            //var sessionFactory = Kernel.Get<ISessionFactory>();
            //ISession session = null;
            //var connectionProviders = Kernel.GetAll<IConnectionProvider>().ToList();
            //if (connectionProviders.Any())
            //{
            //    var connections = connectionProviders.Select(x => x.GetConnection()).Where(x => x != null).ToList();
            //    if (connections.Count() > 1)
            //    {
            //        throw new Exception("多个IConnectionProvider提供了数据库连接");
            //    }
            //    var connection = connections.FirstOrDefault();
            //    if (connection != null)
            //    {
            //        connection.Open();
            //        session = sessionFactory.OpenSession(connection);
            //    }
            //}
            //if (session == null)
            //{
            //    session = sessionFactory.OpenSession();
            //}

            #endregion

            var sessionFactory = kernel.Get<IMySessionFactory>();
            var session = sessionFactory.OpenSession();
            //Auto的性能影响特别明显
            //这里是为了解决并发查询的效率问题，数据库经常被阻塞异常
            session.FlushMode = FlushMode.Commit;
            return session;
        }
        private void SetupForConvention(IList<Assembly> findAppAssemblies, IKernel kernel)
        {
            var implementingClasses = findAppAssemblies.SelectMany(x => x.GetExportedTypes().Where(t => !t.IsAbstract && typeof(IDependency).IsAssignableFrom(t)));

            foreach (var implementingClass in implementingClasses)
            {
                var allInterfaces = implementingClass.GetInterfaces();
                if (allInterfaces.Any(x => typeof(ISingletonDependency).IsAssignableFrom(x)))
                {
                    kernel.Bind(allInterfaces).To(implementingClass).InSingletonScope();
                }
                else if (allInterfaces.Any(x => typeof(IUnitOfWorkDependency).IsAssignableFrom(x)))
                {
                    kernel.Bind(allInterfaces).To(implementingClass).InAmbientOrRequestScope();
                }
                else if (allInterfaces.Any(x => typeof(ITransientDependency).IsAssignableFrom(x)))
                {
                    kernel.Bind(allInterfaces).To(implementingClass).InTransientScope();
                }
                else
                {
                    kernel.Bind(allInterfaces).To(implementingClass).InAmbientOrRequestScope();
                }
            }
        }

        private void SetupForWeb(IKernel kernel)
        {
            //bind by Ninject.Web.Common
            //Kernel.Bind<BundleCollection>().ToMethod(x => BundleTable.Bundles);
            //Kernel.Bind<RouteCollection>().ToMethod(x => RouteTable.Routes);
            //Kernel.Bind<HttpContextBase>().ToMethod(x => new HttpContextWrapper(HttpContext.Current));
            #region ref source

            //https://github.com/ninject/Ninject.Web.Common/blob/master/src/Ninject.Web.Common/WebCommonNinjectModule.cs
            //public class WebCommonNinjectModule : GlobalKernelRegistrationModule<OnePerRequestHttpModule>
            //{
            //    public override void Load()
            //    {
            //        base.Load();
            //        this.Bind<System.Web.Routing.RouteCollection>().ToConstant(System.Web.Routing.RouteTable.Routes);
            //        this.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            //        this.Bind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            //    }
            //}
            #endregion
            kernel.Rebind<IIdentity>().ToMethod(x => HttpContext.Current.User.Identity);
            kernel.Rebind<HttpSessionStateBase>().ToMethod(x => new HttpSessionStateWrapper(HttpContext.Current.Session));
            kernel.Rebind<HttpServerUtilityBase>().ToMethod(x => new HttpServerUtilityWrapper(HttpContext.Current.Server));
        }

        private static IList<Assembly> FindAppAssemblies()
        {
            var myProjectHelper = MyProjectHelper.Resolve();
            var loadAppAssemblies = myProjectHelper.LoadAppAssemblies();
            return loadAppAssemblies;
        }
        
        #region logs
        private void OnActivationSessionLog(ISession session)
        {
            LogMessage(string.Format("<<<<<<< session created || {0}", session.GetHashCode()));
        }
        private void OnDeactivationSessionLog(ISession session)
        {
            LogMessage(string.Format("                        || {0} session disposed >>>>>>>>>>", session.GetHashCode()));
        }
        private void OnActivationUowLog(IRepositoryContext context)
        {
            LogMessage(string.Format("<<<<<<< repositoryContext created || {0}", context.GetHashCode()));
        }
        private void OnDeactivationUowLog(IRepositoryContext context)
        {
            LogMessage(string.Format("                                  || {0} repositoryContext disposed >>>>>>>>>>", context.GetHashCode()));
        }

        private void LogMessage(string message)
        {
            //if (MyDebugHelper.IsDebugMode())
            //{
            //    var myHttpContextHelper = new MyHttpContextHelper();
            //    string url;
            //    myHttpContextHelper.IsRequestAvailable(out url);
            //    UtilsLogger.LogMessage(string.Format("[{0}] > [{1}] > {2}", this.GetType().Name, url, message));
            //}
            MyLogHelper.Resolve().Debug(this.GetType(), message);
        }

        #endregion
    }
}