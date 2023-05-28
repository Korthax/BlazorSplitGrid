using BlazorSplitGrid.Models;

namespace BlazorSplitGrid.Extensions;

internal static class DirectionExtensions
{
    internal static string ToClassName(this Direction self)
    {
        return self.ToString().ToLower();
    }
    
    internal static string ToGridTemplate(this Direction self)
    {
        return self == Direction.Column ? "grid-template-columns" : "grid-template-rows";
    }
}
