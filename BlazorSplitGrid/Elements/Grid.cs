using System.Text;
using BlazorSplitGrid.Interop;
using BlazorSplitGrid.Models;

namespace BlazorSplitGrid.Elements;

internal class Grid
{
    private readonly SizeConstraints _sizeConstraints;
    private readonly SplitGrid _splitGrid;

    private readonly Tracks _columnItems;
    private readonly Tracks _rowItems;

    public static Grid New(SplitGrid splitGrid)
    {
        var sizeConstraints = new SizeConstraints(splitGrid.MinSize, splitGrid.MaxSize, splitGrid.RowMinSize, splitGrid.RowMaxSize, splitGrid.ColumnMinSize, splitGrid.ColumnMaxSize);
        return new Grid(new Tracks(), new Tracks(), sizeConstraints, splitGrid);
    }

    private Grid(Tracks rowItems, Tracks columnItems, SizeConstraints sizeConstraints, SplitGrid splitGrid)
    {
        _rowItems = rowItems;
        _columnItems = columnItems;
        _sizeConstraints = sizeConstraints;
        _splitGrid = splitGrid;
    }

    public Track AddRowGutter(SplitGridGutter gutter)
    {
        var track = new Track(gutter.SplitGridId, _rowItems.Count, true, gutter.Size, gutter.Size, gutter.MinSize ?? _sizeConstraints.RowMin, gutter.MaxSize ?? _sizeConstraints.RowMax, $".{gutter.SplitGridId}");
        _rowItems.Add(track);
        return track;
    }

    public Track AddRowGutter(string selector, string size)
    {
        var track = new Track(selector, _rowItems.Count, true, size, size, _sizeConstraints.RowMin, _sizeConstraints.RowMax, selector);
        _rowItems.Add(track);
        return track;
    }

    public Track AddColumnGutter(SplitGridGutter gutter)
    {
        var track = new Track(gutter.SplitGridId, _columnItems.Count, true, gutter.Size, gutter.Size, gutter.MinSize ?? _sizeConstraints.ColumnMin, gutter.MaxSize ?? _sizeConstraints.ColumnMax, $".{gutter.SplitGridId}");
        _columnItems.Add(track);
        return track;
    }

    public Track AddColumnGutter(string selector, string size)
    {
        var track = new Track(selector, _columnItems.Count, true, size, size, _sizeConstraints.ColumnMin, _sizeConstraints.ColumnMax, selector);
        _columnItems.Add(track);
        return track;
    }

    public (Track Row, Track Column) AddContent(SplitGridContent content)
    {
        var rowTrack = new Track(content.SplitGridId, _rowItems.NextTrack(), false, content.Size, content.Size, _sizeConstraints.RowMin, _sizeConstraints.RowMax, $".{content.SplitGridId}");
        var columnTrack = new Track(content.SplitGridId, _columnItems.NextTrack(), false, content.Size, content.Size, _sizeConstraints.ColumnMin, _sizeConstraints.ColumnMax, $".{content.SplitGridId}");

        if (_rowItems.ShouldAddContent())
            _rowItems.Add(rowTrack);

        if (_columnItems.ShouldAddContent())
            _columnItems.Add(columnTrack);

        return (rowTrack, columnTrack);
    }

    public bool ResetSizes()
    {
        _columnItems.ResetSizes();
        _rowItems.ResetSizes();
        return true;
    }

    public bool Update(Direction direction, int track, string? size)
    {
        var items = direction == Direction.Column ? _columnItems : _rowItems;
        return items.SetSize(track, size);
    }

    public bool Update(Direction direction, string id, string? size)
    {
        var items = direction == Direction.Column ? _columnItems : _rowItems;
        return items.SetSize(id, size);
    }

    public bool Update(Direction direction, Dictionary<int, string> tracks)
    {
        if (tracks.Count == 0)
            return ResetSizes();

        var items = direction == Direction.Column ? _columnItems : _rowItems;
        return tracks.Aggregate(false, (current, track) => items.SetSize(track.Key, track.Value) || current);
    }

    public bool Update(Direction direction, string? sizes)
    {
        if (string.IsNullOrWhiteSpace(sizes))
            return false;

        var tokens = sizes.Split(" ")
            .ToList();

        var updated = false;
        var items = direction == Direction.Column ? _columnItems : _rowItems;
        for (var i = 0; i < tokens.Count; i++)
            updated = items.SetSize(i, tokens[i]) || updated;

        return updated;
    }

    public bool TryRemove(Direction direction, int track, out Track item)
    {
        var items = direction == Direction.Column ? _columnItems : _rowItems;
        return items.TryRemove(track, out item);
    }

    public bool TryRemove(Direction direction, string id, out Track item)
    {
        var items = direction == Direction.Column ? _columnItems : _rowItems;
        return items.TryRemove(id, out item);
    }

    public string? GetSize(Direction direction, int track)
    {
        return direction == Direction.Column
            ? _columnItems.GetSize(track)
            : _rowItems.GetSize(track);
    }

    public string? GetSize(Direction direction, string id)
    {
        return direction == Direction.Column
            ? _columnItems.GetSize(id)
            : _rowItems.GetSize(id);
    }

    public Dictionary<int, string> GetSizes(Direction direction)
    {
        return direction == Direction.Column
            ? _columnItems.ToDictionary(x => x.Number, x => x.Size)
            : _rowItems.ToDictionary(x => x.Number, x => x.Size);
    }

    public string Template(Direction direction)
    {
        return direction == Direction.Column 
            ? _columnItems.BuildTemplate()
            : _rowItems.BuildTemplate();
    }

    internal async Task Initialise(ISplitGridInterop splitGridInterop)
    {
        var cssBuilder = new StringBuilder();
        cssBuilder.AppendLine(".split-grid {");
        cssBuilder.AppendLine("\tdisplay: grid;");
        cssBuilder.AppendLine("\theight: 100%;");

        if(_rowItems.Count > 1)
            cssBuilder.AppendLine($"\tgrid-template-rows: {_rowItems.BuildTemplate()};");

        if(_columnItems.Count > 1)
            cssBuilder.AppendLine($"\tgrid-template-columns: {_columnItems.BuildTemplate()};");

        cssBuilder.AppendLine("}");
        cssBuilder.AppendLine();

        foreach (var track in _rowItems.Where(x => x.IsGutter))
            cssBuilder.AppendLine($".split-grid-gutter-row-{track.Number} {{ grid-row: {track.Number + 1}; }}");

        foreach (var track in _columnItems.Where(x => x.IsGutter))
            cssBuilder.AppendLine($".split-grid-gutter-column-{track.Number} {{ grid-column: {track.Number + 1}; }}");

        var rowGutters = _rowItems
            .Where(x => x.IsGutter)
            .ToList();

        var columnGutters = _columnItems
            .Where(x => x.IsGutter)
            .ToList();

        var options = new SplitGridOptions
        {
            MinSize = _sizeConstraints.Min,
            MaxSize = _sizeConstraints.Max,
            ColumnMinSize = _sizeConstraints.ColumnMin,
            ColumnMaxSize = _sizeConstraints.ColumnMax,
            RowMinSize = _sizeConstraints.RowMin,
            RowMaxSize = _sizeConstraints.RowMax,
            ColumnMinSizes = _columnItems.ToDictionary(x => x.Number, x => x.MinSize),
            ColumnMaxSizes = _columnItems.ToDictionary(x => x.Number, x => x.MaxSize),
            RowMinSizes = _rowItems.ToDictionary(x => x.Number, x => x.MinSize),
            RowMaxSizes = _rowItems.ToDictionary(x => x.Number, x => x.MaxSize),
            Css = cssBuilder.ToString(),
            SnapOffset = _splitGrid.SnapOffset,
            ColumnSnapOffset = _splitGrid.ColumnSnapOffset,
            RowSnapOffset = _splitGrid.RowSnapOffset,
            DragInterval = _splitGrid.DragInterval,
            RowDragInterval = _splitGrid.RowDragInterval,
            ColumnDragInterval = _splitGrid.ColumnDragInterval,
            Cursor = _splitGrid.Cursor,
            ColumnCursor = _splitGrid.ColumnCursor,
            RowCursor = _splitGrid.RowCursor,
            HasOnDrag = _splitGrid.OnDrag.HasDelegate,
            HasOnDragStart = true,
            HasOnDragStop = true
        };

        await splitGridInterop.Initialise(rowGutters, columnGutters, options);
    }
}
