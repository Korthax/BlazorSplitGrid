namespace BlazorSplitGrid.Models;

public class DragEventArgs
{
    public string? GridTemplateStyle { get; }
    public Direction Direction { get; }
    public int Track { get; }

    public DragEventArgs(Direction direction, int track, string? gridTemplateStyle = null)
    {
        GridTemplateStyle = gridTemplateStyle;
        Direction = direction;
        Track = track;
    }
}
