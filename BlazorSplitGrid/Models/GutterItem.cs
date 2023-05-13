namespace BlazorSplitGrid.Models;

public class GutterItem
{
    public string Id { get; set; }
    public int Track { get; set; }

    public GutterItem(string id, int track)
    {
        Id = id;
        Track = track;
    }
}