using System.Collections.Generic;

namespace NbCloud.Common.Extensions
{
    public static class StringExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> values, string separator = ",")
        {
            if (values == null)
            {
                return string.Empty;
            }
            return string.Join(separator, values);
        }
    }
}
