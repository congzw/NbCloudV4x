//using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace NbCloud.Web
{
    public class NinjectConfig
    {
        //private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        public static void Setup()
        {
            //DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            //DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            //bootstrapper.Initialize(() => CreateKernel(appAssemblies));
        }
    }
}
