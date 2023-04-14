using System.Collections.Generic;
using System.Linq;

namespace MyFireCoreLibraries;
public static class Extensions
{
    public static bool SafeHasRows<T>(this IEnumerable<T> list)
    {
        return list != null && list.Any();
    }
    public static bool SafeHasRows<T>(this IList<T> list)
    {
        return list != null && list.Any();
    }
}