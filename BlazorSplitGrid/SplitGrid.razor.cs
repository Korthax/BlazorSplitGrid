using BlazorSplitGrid.Elements;
using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Interop;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorSplitGrid;

public partial class SplitGrid : SplitGridComponentBase
{
    [Parameter]
    public EventCallback<DragEventArgs> OnDrag { get; set; }

    [Parameter]
    public EventCallback<DragEventArgs> OnDragStart { get; set; }

    [Parameter]
    public EventCallback<DragEventArgs> OnDragStop { get; set; }

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public decimal MinSize { get; set; }

    [Parameter]
    public decimal MaxSize { get; set; } = decimal.MaxValue;

    [Parameter] 
    public int? ColumnMinSize { get; set; }

    [Parameter] 
    public int? ColumnMaxSize { get; set; }

    [Parameter] 
    public int? RowMinSize { get; set; }

    [Parameter] 
    public int? RowMaxSize { get; set; }

    [Parameter] 
    public int? SnapOffset { get; set; }

    [Parameter] 
    public int? ColumnSnapOffset { get; set; }

    [Parameter] 
    public int? RowSnapOffset { get; set; }

    [Parameter] 
    public int? DragInterval { get; set; }
    
    [Parameter] 
    public int? ColumnDragInterval { get; set; }

    [Parameter] 
    public int? RowDragInterval { get; set; }

    [Parameter] 
    public string? Cursor { get; set; }

    [Parameter] 
    public string? ColumnCursor { get; set; }

    [Parameter] 
    public string? RowCursor { get; set; }

    public ElementReference Element { get; set; }

    protected override string Classes => ClassBuilder
        .Append("split-grid")
        .Build();

    private Grid Grid => _grid ??= Grid.New(this);
    private SplitGridInterop? _splitGrid;
    private Grid? _grid;

    protected override Task OnInitializedAsync()
    {
        _splitGrid = new SplitGridInterop(JsRuntime);
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
            await Initialise();
    }

    public async Task Initialise()
    {
        if (_splitGrid is null)
            return;

        await Grid.Initialise(_splitGrid);
        _splitGrid.OnDrag += (_, args) => OnDrag.InvokeAsync(args);
        _splitGrid.OnDragStart +=  async (_, args) => await OnSizesChanged(args, OnDragStart);
        _splitGrid.OnDragStop += async (_, args) => await OnSizesChanged(args, OnDragStop);
    }

    public async Task<Track> AppendColumnGutter(string selector, decimal size)
    {
        if (_splitGrid is null)
            throw new InvalidOperationException("SplitGrid is not initialised");

        var item = Grid.AddColumnGutter(selector, size);
        await _splitGrid.AddColumnGutter(selector, item.Number);
        return item;
    }

    public async Task<Track> AppendRowGutter(string selector, decimal size)
    {
        if (_splitGrid is null)
            throw new InvalidOperationException("SplitGrid is not initialised");

        var item = Grid.AddRowGutter(selector, size);
        await _splitGrid.AddRowGutter(selector, item.Number);
        return item;
    }

    public async Task RemoveColumnGutter(int trackNumber, bool immediate = true)
    {
        if (_splitGrid is null || !Grid.TryRemove(Direction.Column, trackNumber, out var item))
            return;

        await _splitGrid.RemoveColumnGutter(item.Selector, item.Number, immediate);
    }

    public async Task RemoveColumnGutter(string id, bool immediate = true)
    {
        if (_splitGrid is null || !Grid.TryRemove(Direction.Column, id, out var item))
            return;

        await _splitGrid.RemoveColumnGutter(item.Selector, item.Number, immediate);
    }

    public async Task RemoveRowGutter(int trackNumber, bool immediate = true)
    {
        if (_splitGrid is null || !Grid.TryRemove(Direction.Row, trackNumber, out var item))
            return;

        await _splitGrid.RemoveRowGutter(item.Selector, item.Number, immediate);
    }

    public async Task RemoveRowGutter(string id, bool immediate = true)
    {
        if (_splitGrid is null || !Grid.TryRemove(Direction.Row, id, out var item))
            return;

        await _splitGrid.RemoveRowGutter(item.Selector, item.Number, immediate);
    }

    public async Task Destroy(bool immediate = true)
    {
        if (_splitGrid is null)
            return;

        await _splitGrid.Destroy(immediate);
    }

    public async Task SetSize(Direction direction, int track, decimal? size)
    {
        if (_splitGrid is null)
            return;

        Grid.Update(direction, track, size);
        await _splitGrid.SetSizes(".split-grid", direction.ToGridTemplate(), Grid.Template(direction));
    }

    public async Task<decimal?> GetSize(Direction direction, int track, bool refresh = false)
    {
        if (_splitGrid is null)
            return null;

        if (!refresh)
            return Grid.GetSize(direction, track);

        var sizes = await _splitGrid.GetSizes(".split-grid", direction.ToGridTemplate());
        Grid.Update(direction, sizes);
        return Grid.GetSize(direction, track);
    }

    public async Task SetSize(Direction direction, string id, decimal? size)
    {
        if (_splitGrid is null)
            return;

        Grid.Update(direction, id, size);
        await _splitGrid.SetSizes(".split-grid", direction.ToGridTemplate(), Grid.Template(direction));
    }

    public async Task<decimal?> GetSize(Direction direction, string id, bool refresh = false)
    {
        if (_splitGrid is null)
            return null;

        if (!refresh)
            return Grid.GetSize(direction, id);

        var sizes = await _splitGrid.GetSizes(".split-grid", direction.ToGridTemplate());
        Grid.Update(direction, sizes);
        return Grid.GetSize(direction, id);
    }

    public async Task SetSizes(Direction direction, string? sizes)
    {
        if (_splitGrid is null)
            return;

        Grid.Update(direction, sizes);
        await _splitGrid.SetSizes(".split-grid", direction.ToGridTemplate(), Grid.Template(direction));
    }

    public async Task<string> GetSizes(Direction direction, bool refresh = false)
    {
        if (_splitGrid is null)
            return string.Empty;

        if (!refresh)
            return Grid.Template(direction);

        var sizes = await _splitGrid.GetSizes(".split-grid", direction.ToGridTemplate());
        Grid.Update(direction, sizes);
        return Grid.Template(direction);
    }

    public async Task Reset()
    {
        if (_splitGrid is null)
            return;

        Grid.ResetSizes();
        await _splitGrid.SetSizes(".split-grid", "grid-template-rows", Grid.Template(Direction.Row));
        await _splitGrid.SetSizes(".split-grid", "grid-template-columns", Grid.Template(Direction.Column));
    }

    internal Track AddRow(SplitGridGutter gutter)
    {
        return Grid.AddRowGutter(gutter);
    }

    internal Track AddColumn(SplitGridGutter gutter)
    {
        return Grid.AddColumnGutter(gutter);
    }

    public (Track Row, Track Column) AddContent(SplitGridContent content)
    {
        return Grid.AddContent(content);
    }

    private async Task OnSizesChanged(DragEventArgs eventArgs, EventCallback<DragEventArgs> callback)
    {
        if (!string.IsNullOrWhiteSpace(eventArgs.GridTemplateStyle))
            Grid.Update(eventArgs.Direction, eventArgs.GridTemplateStyle);

        await callback.InvokeAsync(eventArgs);
    }
}
