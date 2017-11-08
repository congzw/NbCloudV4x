using Ninject.Activation;
using Ninject.Syntax;

namespace NbCloud.Common.AmbientScopes.Ninjects
{
    public static class AmbientScopeExtensions
    {
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
