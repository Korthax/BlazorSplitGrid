namespace BlazorSplitGrid.Models;

public record Track(string Id, int Number, bool IsGutter, decimal Size, decimal InitialSize, decimal MinSize, decimal MaxSize, string Selector)
{
    public string ToSizeString()
    {
        return $"{Size}{(IsGutter ? "px" : "fr")}";
    }
}
