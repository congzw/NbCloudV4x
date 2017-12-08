using System;
using System.Linq;

namespace NbCloud.Common.Extensions
{
    public static class TypeExtensions
    {
        private static string _namespacePrefix = null;
        public static string GetNamespacePrefix(this Type type)
        {
            if (_namespacePrefix != null)
            {
                return _namespacePrefix;
            }
            
            var ns = type.Namespace;
            if (ns != null)
            {
                var result = ns.Split('.').FirstOrDefault();
                _namespacePrefix = !string.IsNullOrWhiteSpace(result) ? result : "NbCloud";
            }
            return _namespacePrefix;
        }
    }
}
