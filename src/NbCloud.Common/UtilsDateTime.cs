using System;

namespace NbCloud.Common
{
    public interface IUtilsDateTime
    {
        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        DateTime Now();
    }

    public class UtilsDateTime : IUtilsDateTime
    {
        public DateTime Now()
        {
            return DateTime.Now; 
        }

        public static IUtilsDateTime Current
        {
            get
            {
                return _defaultFactoryFunc.Invoke();
            }
        }

        /// <summary>
        /// 获取工厂方法
        /// </summary>
        public static Func<IUtilsDateTime> GetFactoryFunc()
        {
            return _defaultFactoryFunc;
        }

        /// <summary>
        /// 重新设置工厂方法
        /// </summary>
        /// <param name="func"></param>
        public static void SetFactoryFunc(Func<IUtilsDateTime> func)
        {
            if (func != null)
            {
                _defaultFactoryFunc = func;
            }
        }

        private static Func<IUtilsDateTime> _defaultFactoryFunc = CreateDefault;
        private static IUtilsDateTime CreateDefault()
        {
            var defaultImpl = new UtilsDateTime();
            return defaultImpl;
        }

        #region for simple use

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTime()
        {
            return Current.Now();
        }

        #endregion
    }
    
    public static class UtilsDateTimeExtensions
    {
        public static string ToDisplay(this DateTime now, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return now.ToString(format);
        }

        public static DateTime Normalize(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }

            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime.ToLocalTime();
            }

            return dateTime;
        }
    }
}