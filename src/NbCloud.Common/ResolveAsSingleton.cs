using System;
using System.Text;

namespace NbCloud.Common
{
    public interface IResolveAsSingleton
    {
    }

    public sealed class ResolveAsSingleton<T> where T : IResolveAsSingleton, new() 
    {
        #region for ioc extensions

        private static readonly Lazy<T> LazyInstance = new Lazy<T>(() => new T());

        private static Func<T> _defaultFactoryFunc = () => LazyInstance.Value;
        /// <summary>
        /// 当前的实例
        /// </summary>
        public static T Resolve()
        {
            var invoke = _defaultFactoryFunc.Invoke();
            //AppendLog.AppendLine(invoke.GetHashCode() + ":" + Thread.CurrentThread.ManagedThreadId);
            //Console.WriteLine(invoke.GetHashCode() + ":" + Thread.CurrentThread.ManagedThreadId);
            //File.AppendAllText("D:\\test\\" + invoke.GetHashCode() + "-" + Thread.CurrentThread.ManagedThreadId + ".txt", invoke.GetHashCode().ToString()+",");
            return invoke;
            //return _defaultFactoryFunc.Invoke();
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
        public static void SetFactoryFunc(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            _defaultFactoryFunc = func;
        }

        public static StringBuilder AppendLog = new StringBuilder();

        #endregion 
    }
}