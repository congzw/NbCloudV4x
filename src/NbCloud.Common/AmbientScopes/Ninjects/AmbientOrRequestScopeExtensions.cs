using System.Linq;
using Ninject.Activation;
using Ninject.Syntax;
using Ninject.Web.Common;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public static class AmbientOrRequestScopeExtensions
    {
        public static IBindingNamedWithOrOnSyntax<T> InAmbientOrRequestScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(GetAmbientOrRequestScope);
        }
        private static object GetAmbientOrRequestScope(IContext ctx)
        {
            var scope = AmbientScope.Current;
            if (scope != null)
            {
                return scope;
            }
            var nscope = GetRequestScope(ctx);
            return nscope;
        }
        private static object GetRequestScope(IContext ctx)
        {
            //Note: Copied from Ninject (https://github.com/ninject/Ninject.Web.Common/blob/master/src/Ninject.Web.Common/RequestScopeExtensionMethod.cs) subject to changes (out of sync with Ninject)
            var nscope = ctx.Kernel.Components.GetAll<INinjectHttpApplicationPlugin>().Select(c => c.GetRequestScope(ctx)).FirstOrDefault(s => s != null);
            return nscope;
        }
    }
}
