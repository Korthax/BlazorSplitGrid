using BlazorSplitGrid.Models;

namespace BlazorSplitGrid.Extensions;

internal static class DirectionExtensions
{
    internal static string ToClassName(this Direction self)
    {
        return self.ToString().ToLower();
    }
}
