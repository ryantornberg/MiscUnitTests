using System.Collections.Generic;

namespace MiscUnitTests.TimeRange
{
    public static class ExtensionMethods
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            return new HashSet<T>(items);
        }
    }
}
