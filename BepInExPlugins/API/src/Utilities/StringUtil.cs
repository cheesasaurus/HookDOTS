
using System.Collections.Generic;
using System.Linq;

namespace HookDOTS.Utilities;

internal static class StringUtil
{
    public static string StringifyEnumerableOfStrings(IEnumerable<string> strings)
    {
        var quotedStrings = strings.Select(str => $"\"{str}\"");
        return string.Join(", ", quotedStrings);
    }

}