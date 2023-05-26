namespace BlazorSplitGrid.Models;

public class GutterItem
{
    public string Id { get; set; }
    public int Track { get; set; }
    public int Size { get; set; }

    public GutterItem(string id, int track, int size)
    {
        Id = id;
        Track = track;
        Size = size;
    }
}
