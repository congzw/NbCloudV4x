using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NbCloud.Common
{
    /// <summary>
    /// 模型的帮助类
    /// </summary>
    public interface IMyModelHelper
    {
        void TryCopyProperties(Object updatingObj, Object collectedObj, string[] excludeProperties = null, bool ignoreCase = false);
        IList<string> GetPropertyNames<T>();
        IDictionary<string, object> GetPropertyNameValue<T>(T obj);
    }

    /// <summary>
    /// 模型的帮助类
    /// </summary>
    public class MyModelHelper : IMyModelHelper, IResolveAsSingleton
    {
        public void TryCopyProperties(Object updatingObj, Object collectedObj, string[] excludeProperties = null, bool ignoreCase = false)
        {
            if (collectedObj != null && updatingObj != null)
            {
                //获取类型信息
                Type updatingObjType = updatingObj.GetType();
                PropertyInfo[] updatingObjPropertyInfos = updatingObjType.GetProperties();

                Type collectedObjType = collectedObj.GetType();
                PropertyInfo[] collectedObjPropertyInfos = collectedObjType.GetProperties();

                string[] fixedExPropertites = excludeProperties ?? new string[] { };


                var ordinalIgnoreCase = ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
                var ordinalIgnoreCase2 = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

                foreach (PropertyInfo updatingObjPropertyInfo in updatingObjPropertyInfos)
                {
                    foreach (PropertyInfo collectedObjPropertyInfo in collectedObjPropertyInfos)
                    {
                        if (updatingObjPropertyInfo.Name.Equals(collectedObjPropertyInfo.Name, ordinalIgnoreCase2))
                        {
                            if (fixedExPropertites.Contains(updatingObjPropertyInfo.Name, ordinalIgnoreCase))
                            {
                                continue;
                            }

                            object value = collectedObjPropertyInfo.GetValue(collectedObj, null);
                            if (updatingObjPropertyInfo.CanWrite)
                            {
                                updatingObjPropertyInfo.SetValue(updatingObj, value, null);
                            }
                            //try
                            //{
                            //    object value = collectedObjPropertyInfo.GetValue(collectedObj, null);
                            //    updatingObjPropertyInfo.SetValue(updatingObj, value, null);
                            //}
                            //catch (Exception ex)
                            //{
                            //    string temp = ex.Message;
                            //}
                            break;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取所有的Property名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<string> GetPropertyNames<T>()
        {
            var result = new List<string>();
            //获取类型信息
            Type t = typeof(T);
            PropertyInfo[] propertyInfos = t.GetProperties();
            foreach (PropertyInfo var in propertyInfos)
            {
                result.Add(var.Name);
            }
            return result;
        }

        public IDictionary<string, object> GetPropertyNameValue<T>(T obj)
        {
            var result = new Dictionary<string, object>();
            if (obj != null)
            {
                //获取类型信息
                Type t = typeof(T);
                PropertyInfo[] propertyInfos = t.GetProperties();
                foreach (PropertyInfo var in propertyInfos)
                {
                    result.Add(var.Name, var.GetValue(obj, null));
                }
            }
            return result;
        }

        #region ioc helpers

        /// <summary>
        /// 支持动态替换（ResolveAsSingleton）
        /// </summary>
        /// <returns></returns>
        public static IMyModelHelper Resolve()
        {
            return ResolveAsSingleton<MyModelHelper, IMyModelHelper>.Resolve();
        }

        #endregion
    }
}
