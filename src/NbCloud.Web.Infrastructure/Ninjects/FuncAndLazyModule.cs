using System;
using System.Linq;
using System.Reflection;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Parameters;

namespace NbCloud.Web.Ninjects
{
    /// <summary>
    /// 包含Func和Lazy的注入支持，用于对构造函数性能比较关注的场合
    /// http://stackoverflow.com/questions/4840157/does-ninject-support-func-auto-generated-factory
    /// http://stackoverflow.com/questions/2538132/lazy-loading-with-ninject/2538357#2538357
    /// </summary>
    internal class FuncAndLazyModule : NinjectModule
    {
        public override void Load()
        {
            var kernel = this.Kernel;
            if (kernel != null)
            {
                kernel.Bind(typeof(Func<>)).ToMethod(CreateFunc).When(VerifyFactoryFunction);
                kernel.Bind(typeof(Lazy<>)).ToMethod(ctx =>
                    GetType()
                    .GetMethod("GetLazyProvider", BindingFlags.Instance | BindingFlags.NonPublic)
                        .MakeGenericMethod(ctx.GenericArguments[0])
                        .Invoke(this, new object[] { ctx.Kernel }));
            }
        }

        protected Lazy<T> GetLazyProvider<T>(IKernel kernel)
        {
            return new Lazy<T>(() => kernel.Get<T>());
        }

        private static bool VerifyFactoryFunction(IRequest request)
        {
            var genericArguments = request.Service.GetGenericArguments();
            if (genericArguments.Count() != 1)
            {
                return false;
            }

            if (request.ParentContext == null)
            {
                throw new InvalidOperationException("request.ParentContext Should not be null");
            }
            var instanceType = genericArguments.Single();
            return request.ParentContext.Kernel.CanResolve(new Request(genericArguments[0], null, new IParameter[0], null, false, true)) ||
                   TypeIsSelfBindable(instanceType);
        }

        private static object CreateFunc(IContext ctx)
        {
            if (ctx.GenericArguments == null)
            {
                throw new NullReferenceException("ctx.GenericArguments should not be null");
            }
            var functionFactoryType = typeof(FunctionFactory<>).MakeGenericType(ctx.GenericArguments);
            var ctor = functionFactoryType.GetConstructors().Single();
            var functionFactory = ctor.Invoke(new object[] { ctx.Kernel });
            var methodInfo = functionFactoryType.GetMethod("Create");
            if (methodInfo == null)
            {
                throw new InvalidOperationException("Not find method: Create");
            }
            return methodInfo.Invoke(functionFactory, new object[0]);
        }

        private static bool TypeIsSelfBindable(Type service)
        {
            return !service.IsInterface
                   && !service.IsAbstract
                   && !service.IsValueType
                   && service != typeof(string)
                   && !service.ContainsGenericParameters;
        }

        public class FunctionFactory<T>
        {
            private readonly IKernel _kernel;

            public FunctionFactory(IKernel kernel)
            {
                this._kernel = kernel;
            }

            public Func<T> Create()
            {
                return () => this._kernel.Get<T>();
            }
        }
    }

}