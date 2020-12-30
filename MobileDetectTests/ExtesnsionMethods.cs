using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileDetectTests
{
    internal static class ExtesnsionMethods
    {
        internal static ICollection<KeyValuePair<string, StringValues>> ToStringValuesCollection(this Dictionary<string, string> pairs)
            => pairs
            .Select(s => new KeyValuePair<string, StringValues>(
                s.Key,
                s.Value
             ))
            .ToArray();
    }
}
