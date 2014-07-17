using System.Collections.Generic;

namespace System
{
    public static class StringExtensions
    {
        public static IEnumerable<string> Sort(this string[] strings)
        {
            Array.Sort(strings);

            return strings;
        }
    }
}
