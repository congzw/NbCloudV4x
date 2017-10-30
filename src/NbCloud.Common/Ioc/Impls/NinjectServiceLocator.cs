using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject;
using Ninject.Infrastructure;
using Ninject.Planning.Bindings;

namespace NbCloud.Common.Ioc.Impls
{
    /// <summary>
    /// NinjectServiceLocator
    /// </summary>
    public class NinjectServiceLocator : ServiceLocatorImplBase
    {
        public Func<IKernel> KernelFunc { get; private set; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="kernelFunc"></param>
        public NinjectServiceLocator(Func<IKernel> kernelFunc)
        {
            KernelFunc = kernelFunc;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            var kernel = KernelFunc.Invoke();
            if (key == null)
            {
                return kernel.TryGet(serviceType);
            }
            return kernel.TryGet(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            var kernel = KernelFunc.Invoke();
            return kernel.GetAll(serviceType);
        }

        private static ICollection<Type> _registedTypes;
        /// <summary>
        /// 所有注册到ioc的类型 
        /// </summary>
        public override bool IsRegistered(Type type)
        {
            if (_registedTypes == null)
            {
                var kernel = KernelFunc.Invoke();
                var field = typeof (KernelBase).GetField("bindings", BindingFlags.Instance | BindingFlags.NonPublic);
                var bindingsMap = (Multimap<Type, IBinding>) field.GetValue(kernel);
                _registedTypes = bindingsMap.Keys;
            }
            return _registedTypes.Contains(type);
        }

        public override bool IsRegistered<T>()
        {
            return IsRegistered(typeof (T));
        }
    }
}
