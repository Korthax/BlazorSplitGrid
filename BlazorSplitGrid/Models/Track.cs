namespace BlazorSplitGrid.Models;

public record Track(string Id, int Number, bool IsGutter, string Size, string InitialSize, int MinSize, int MaxSize, string Selector)
{
    public string ToSizeString()
    {
        return $"{Size}";
    }
}
