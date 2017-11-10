using NbCloud.Common.AmbientScopes.Plugins;
using Ninject;
using Ninject.Activation;
using Ninject.Syntax;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public static class AmbientScopeExtensions
    {
        public static void SetupAmbientScope(this IKernel kernel)
        {
            kernel.Rebind<AmbientScope>().To<NinjectAmbientScope>();
            kernel.Rebind<IAmbientScopeTrancation>().To<AmbientScopeTrancation>();
            kernel.Rebind<IAmbientScopeTaskHelper>().ToMethod(ctx =>
            {
                return new AmbientScopeTaskHelper(() => ctx.Kernel.Get<IAmbientScopeTrancation>());
            }).InSingletonScope();
        }

        public static IBindingNamedWithOrOnSyntax<T> InAmbientScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(GetAmbientScope);
        }
        private static object GetAmbientScope(IContext ctx)
        {
            var scope = AmbientScope.Current;
            return scope;
        }
    }
    
}
