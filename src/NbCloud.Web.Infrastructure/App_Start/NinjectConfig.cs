using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using NbCloud.Common;
using NbCloud.Common.Extensions;
using NbCloud.Common.Ioc;
using NbCloud.Common.Ioc.Impls;
using NbCloud.Common.Logs;
using NbCloud.Web.Ninjects;
using Ninject;
using Ninject.Web.Common;

namespace NbCloud.Web
{
    public class NinjectConfig
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
        public static void Setup()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                FixMultiBindings(kernel);
                SetupIocServiceLocator(kernel);
                LoadNinjectModules(kernel);
                return kernel;
            }
            catch (Exception ex)
            {
                kernel.Dispose();
                throw new Exception("初始化CreateKernel发生异常", ex);
            }
        }

        private static void LoadNinjectModules(IKernel kernel, bool throwEx = true)
        {
            var assemblies = LoadProjectAssembly();
            var modules = assemblies.SelectMany(assembly => assembly.GetNinjectModules()).ToList();
            kernel.Load(modules);
            LogMessage(string.Format("load modules count : {0} ", modules.Count));
            string[] moduleNames = modules.Select(x => x.Name).ToArray();
            LogMessage(string.Format("load {0} ninject modules : {1}", moduleNames.Length, moduleNames.JoinToString()));
        }
        
        private static void SetupIocServiceLocator(IKernel kernel)
        {
            kernel.Bind<IServiceLocator>().To<NinjectServiceLocator>().InSingletonScope();
            CoreServiceProvider.CurrentFunc = () => kernel.Get<IServiceLocator>();
        }
        
        //ninject 3.2 bugs fix
        private static void FixMultiBindings(StandardKernel kernel)
        {
            ////fixed bind multi time bugs!!!
            kernel.Rebind<HttpContext>().ToMethod(ctx => HttpContext.Current).InTransientScope();
            kernel.Rebind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();
            kernel.Rebind<RouteCollection>().ToConstant(RouteTable.Routes);
            
            //bind by Ninject.Web.Common
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
            
            //bind by Ninject.Web.Mvc
            #region ref source
            //https://github.com/ninject/Ninject.Web.Mvc/blob/master/src/Ninject.Web.Mvc/MvcModule.cs
            //public class MvcModule : NinjectModule
            //{
            //    public override void Load()
            //    {
            //        this.Kernel.Components.Add<INinjectHttpApplicationPlugin, NinjectMvcHttpApplicationPlugin>();
            //        this.Kernel.Bind<IDependencyResolver>().To<NinjectDependencyResolver>();
            //        this.Kernel.Bind<IFilterProvider>().To<NinjectFilterAttributeFilterProvider>();
            //        this.Kernel.Bind<IFilterProvider>().To<NinjectFilterProvider>();
            //        this.Kernel.Bind<ModelValidatorProvider>().To<NinjectDataAnnotationsModelValidatorProvider>();
            //    }
            //}
            #endregion

            //bind by Ninject.Web.WebApi
            #region ref source

            //https://github.com/ninject/Ninject.Web.WebApi/blob/master/src/Ninject.Web.WebApi/WebApiModule.cs#L39
            //public class WebApiModule : NinjectModule
            //{
            //    /// <summary>
            //    /// Loads the module into the kernel.
            //    /// </summary>
            //    public override void Load()
            //    {
            //        this.Kernel.Components.Add<INinjectHttpApplicationPlugin, NinjectWebApiHttpApplicationPlugin>();
            //        this.Kernel.Components.Add<IWebApiRequestScopeProvider, DefaultWebApiRequestScopeProvider>();

            //        this.Bind<IDependencyResolver>().To<NinjectDependencyResolver>();

            //        this.Bind<IFilterProvider>().To<DefaultFilterProvider>();
            //        this.Bind<IFilterProvider>().To<NinjectFilterProvider>();

            //        this.Bind<ModelValidatorProvider>().To<NinjectDefaultModelValidatorProvider>();
            //        this.Bind<ModelValidatorProvider>().To<NinjectDataAnnotationsModelValidatorProvider>();
            //    }
            //}

            #endregion
        }

        private static IList<Assembly> LoadProjectAssembly()
        {
            var assemblies = MyProjectHelper.Resolve().LoadAppAssemblies();
            LogMessage(string.Format("find assemblies count : {0} ", assemblies.Count));
            int index = 0;
            int indexCount = assemblies.Count;
            foreach (var ninjectAssembly in assemblies)
            {
                index++;
                LogMessage(string.Format("load {0} / {1} assemlby : {2}", index, indexCount, ninjectAssembly.FullName));
            }
            return assemblies;
        }

        private static void LogMessage(string message)
        {
            MyLogHelper.Resolve().Debug(typeof(NinjectConfig), message);
        }
    }
}
