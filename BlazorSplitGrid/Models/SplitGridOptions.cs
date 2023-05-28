using BlazorSplitGrid.Extensions;

namespace BlazorSplitGrid.Models;

public class SplitGridOptions
{
    public decimal? MinSize { get; set; }
    public decimal? MaxSize { get; set; }
    public decimal? ColumnMinSize { get; set; }
    public decimal? ColumnMaxSize { get; set; }
    public decimal? RowMinSize { get; set; }
    public decimal? RowMaxSize { get; set; }
    public Dictionary<int, decimal>? ColumnMinSizes { get; set; }
    public Dictionary<int, decimal>? ColumnMaxSizes { get; set; }
    public Dictionary<int, decimal>? RowMinSizes { get; set; }
    public Dictionary<int, decimal>? RowMaxSizes { get; set; }
    public int? SnapOffset { get; set; }
    public int? ColumnSnapOffset { get; set; }
    public int? RowSnapOffset { get; set; }
    public int? DragInterval { get; set; }
    public int? ColumnDragInterval { get; set; }
    public int? RowDragInterval { get; set; }
    public string? Cursor { get; set; }
    public string? ColumnCursor { get; set; }
    public string? RowCursor { get; set; }
    public bool HasOnDrag { get; set; }
    public bool HasOnDragStart { get; set; }
    public bool HasOnDragStop { get; set; }
    public string? Css { get; set; }

    public Dictionary<string, object> ToInteroperable()
    {
        return new Dictionary<string, object>()
            .AddIfNotNull("css", Css)
            .AddIfNotNull("hasOnDrag", HasOnDrag)
            .AddIfNotNull("hasOnDragStart", HasOnDragStart)
            .AddIfNotNull("hasOnDragStop", HasOnDragStop)
            .AddIfNotNull("minSize", MinSize)
            .AddIfNotNull("maxSize", MaxSize)
            .AddIfNotNull("columnMinSize", ColumnMinSize)
            .AddIfNotNull("columnMaxSize", ColumnMaxSize)
            .AddIfNotNull("columnMinSizes", ColumnMinSizes)
            .AddIfNotNull("columnMaxSizes", ColumnMaxSizes)
            .AddIfNotNull("rowMinSize", RowMinSize)
            .AddIfNotNull("rowMaxSize", RowMaxSize)
            .AddIfNotNull("rowMinSizes", RowMinSizes)
            .AddIfNotNull("rowMaxSizes", RowMaxSizes)
            .AddIfNotNull("snapOffset", SnapOffset)
            .AddIfNotNull("columnSnapOffset", ColumnSnapOffset)
            .AddIfNotNull("rowSnapOffset", RowSnapOffset)
            .AddIfNotNull("dragInterval", DragInterval)
            .AddIfNotNull("rowDragInterval", RowDragInterval)
            .AddIfNotNull("columnDragInterval", ColumnDragInterval)
            .AddIfNotNull("cursor", Cursor)
            .AddIfNotNull("columnCursor", ColumnCursor)
            .AddIfNotNull("rowCursor", RowCursor);
    }
}
