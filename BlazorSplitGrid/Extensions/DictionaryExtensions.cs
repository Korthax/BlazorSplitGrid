using BlazorSplitGrid.Models;

namespace BlazorSplitGrid.Extensions;

internal static class DictionaryExtensions
{
    internal static int NextTrack(this Dictionary<string, GutterItem> self)
    {
        return self.Count * 2 + 1;
    }
    
    internal static Dictionary<string, object> AddIfNotNull(this Dictionary<string, object> self, string key, object? value)
    {
        if (value is null)
            return self;

        self[key] = value;
        return self;
    }
}
