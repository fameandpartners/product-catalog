using System;
using System.Collections.Generic;

namespace Fame.Service.Services
{
    internal static class Util
    {
        internal static KeyValuePair<string, string> CreateKeyValue(IReadOnlyList<string> kvString)
        {
            var key = char.ToLowerInvariant(kvString[0][0]) + kvString[0].Substring(1);
            var value = string.Join(Environment.NewLine, kvString[1].Replace("^", ",").Split("\\n", StringSplitOptions.RemoveEmptyEntries));

            return new KeyValuePair<string, string>(key, value);
        }
    }
}
