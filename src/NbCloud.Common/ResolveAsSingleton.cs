using System;

namespace NbCloud.Common
{
    public interface IResolveAsSingleton
    {
    }

    public class ResolveAsSingleton
    {
        private static Func<Type, object> _resolveFunc;
        public static void SetResolve(Func<Type, object> resolve)
        {
            _resolveFunc = resolve;
        }

        /// <summary>
        /// 当前的实例
        /// </summary>
        public static TInterface Resolve<T, TInterface>() where T : IResolveAsSingleton, TInterface, new()
        {
            if (_resolveFunc != null)
            {
                var instance = _resolveFunc(typeof (TInterface));
                if (instance != null)
                {
                    return (TInterface)_resolveFunc(typeof(TInterface));   
                }
            }
            return ResolveAsSingletonHelper<T, TInterface>.Resolve();
        }

        /// <summary>
        /// 重新设置工厂方法（恢复默认）
        /// </summary>
        public static void ResetFactoryFunc<T, TInterface>() where T : IResolveAsSingleton, TInterface, new()
        {
            ResolveAsSingletonHelper<T, TInterface>.ResetFactoryFunc();
        }

        /// <summary>
        /// 重新设置工厂方法
        /// </summary>
        /// <param name="func"></param>
        public static void SetFactoryFunc<T, TInterface>(Func<TInterface> func) where T : IResolveAsSingleton, TInterface, new()
        {
            ResolveAsSingletonHelper<T, TInterface>.SetFactoryFunc(func);
        }

        private class ResolveAsSingletonHelper<T, TInterface> where T : IResolveAsSingleton, TInterface, new()
        {
            #region for ioc extensions

            private static readonly object _lock = new object();
            private static T _instance = default(T);
            private static readonly Lazy<TInterface> LazyInstance = new Lazy<TInterface>(() =>
            {
                lock (_lock)
                {
                    _instance = new T();
                }
                return _instance;
            });

            private static Func<TInterface> _defaultFactoryFunc = () => LazyInstance.Value;
            /// <summary>
            /// 当前的实例
            /// </summary>
            internal static TInterface Resolve()
            {
                var invoke = _defaultFactoryFunc.Invoke();
                return invoke;
            }

            /// <summary>
            /// 重新设置工厂方法（恢复默认）
            /// </summary>
            internal static void ResetFactoryFunc()
            {
                lock (_lock)
                {
                    _defaultFactoryFunc = () => LazyInstance.Value;
                }
            }

            /// <summary>
            /// 重新设置工厂方法
            /// </summary>
            /// <param name="func"></param>
            internal static void SetFactoryFunc(Func<TInterface> func)
            {
                if (func == null)
                {
                    throw new ArgumentNullException("func");
                }
                lock (_lock)
                {
                    _defaultFactoryFunc = func;
                }
            }

            #endregion
        }
    }
}