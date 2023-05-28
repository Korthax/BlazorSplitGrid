using System.Collections;

namespace BlazorSplitGrid.Extensions;

internal static class EnumerableExtensions
{
    internal static Dictionary<string, object> AddIfNotNull<T>(this Dictionary<string, object> self, string key, T? value) where T : IDictionary
    {
        if (value is null || value.Count == 0)
            return self;

        self[key] = value;
        return self;
    }

    internal static Dictionary<string, object> AddIfNotNull(this Dictionary<string, object> self, string key, object? value)
    {
        if (value is null)
            return self;

        self[key] = value;
        return self;
    }
}
