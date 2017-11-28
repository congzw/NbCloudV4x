using System;
using System.Reflection;

namespace NbCloud.Common.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取当前类型的Assembly
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }
    }
}