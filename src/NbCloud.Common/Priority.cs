using System.Collections.Generic;
using System.Linq;

namespace NbCloud.Common
{
    public interface IPriority
    {
        int Priority();
    }

    public static class PriorityExtensions
    {
        public static int Priority_Minus_10000(this IPriority priority)
        {
            return -10000;
        }
        public static int Priority_Default_0(this IPriority priority)
        {
            return 0;
        }
        public static int Priority_Plus_10000(this IPriority priority)
        {
            return 10000;
        }

        public static IOrderedEnumerable<IPriority> SortByPriority(this IEnumerable<IPriority> items)
        {
            var orderedItems = items.OrderBy(x => x.Priority());
            return orderedItems;
        }
    }
}