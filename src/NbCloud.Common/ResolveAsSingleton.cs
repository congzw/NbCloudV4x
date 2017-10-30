using System;

namespace NbCloud.Common
{
    public interface IResolveAsSingleton
    {
    }

    public sealed class ResolveAsSingleton<T, TInterface> where T : IResolveAsSingleton, TInterface, new()
    {
        #region for ioc extensions

        private static readonly Lazy<TInterface> LazyInstance = new Lazy<TInterface>(() => new T());

        private static Func<TInterface> _defaultFactoryFunc = () => LazyInstance.Value;
        /// <summary>
        /// 当前的实例
        /// </summary>
        public static TInterface Resolve()
        {
            var invoke = _defaultFactoryFunc.Invoke();
            return invoke;
        }

        /// <summary>
        /// 重新设置工厂方法（恢复默认）
        /// </summary>
        public static void ResetFactoryFunc()
        {
            _defaultFactoryFunc = () => LazyInstance.Value;
        }

        /// <summary>
        /// 重新设置工厂方法
        /// </summary>
        /// <param name="func"></param>
        public static void SetFactoryFunc(Func<TInterface> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            _defaultFactoryFunc = func;
        }

        #endregion
    }
}