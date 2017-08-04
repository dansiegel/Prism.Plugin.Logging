using System;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Logging.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> Chunkify(this string str, int chunkSize) =>
            Enumerable.Range(0, str.Length / chunkSize)
                      .Select(i => str.Substring(i * chunkSize, chunkSize));

    }
}