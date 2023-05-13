using BlazorSplitGrid.Elements;
using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorSplitGrid;

public partial class SplitGrid : ComponentBase
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
    public int? MinSize { get; set; }

    [Parameter] 
    public int? MaxSize { get; set; }

    [Parameter] 
    public int? ColumnMinSize { get; set; }

    [Parameter] 
    public int? ColumnMaxSize { get; set; }

    [Parameter] 
    public Dictionary<int, int>? ColumnMinSizes { get; set; } = new();

    [Parameter] 
    public Dictionary<int, int>? ColumnMaxSizes { get; set; } = new();

    [Parameter] 
    public int? RowMinSize { get; set; }

    [Parameter] 
    public int? RowMaxSize { get; set; }

    [Parameter] 
    public Dictionary<int, int>? RowMinSizes { get; set; } = new();

    [Parameter]
    public Dictionary<int, int>? RowMaxSizes { get; set; } = new();

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

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    public ElementReference Element { get; set; }

    public string Classes => AttributeBuilder.New()
        .Append("split-grid")
        .Append(Class)
        .Build();

    public string Styles => AttributeBuilder.New()
        .Append(Style)
        .Build();

    private readonly Dictionary<string, GutterItem> _columns = new();
    private readonly Dictionary<string, GutterItem> _rows = new();

    private SplitGridInterop? _splitGrid;

    protected override Task OnInitializedAsync()
    {
        _splitGrid = new SplitGridInterop(JsRuntime);
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && _splitGrid is not null)
        {
            var options = new SplitGridOptions
            {
                MinSize = MinSize,
                MaxSize = MaxSize,
                ColumnMinSize = ColumnMinSize,
                ColumnMaxSize = ColumnMaxSize,
                ColumnMinSizes = ColumnMinSizes,
                ColumnMaxSizes = ColumnMaxSizes,
                RowMinSize = RowMinSize,
                RowMaxSize = RowMaxSize,
                RowMinSizes = RowMinSizes,
                RowMaxSizes = RowMaxSizes,
                SnapOffset = SnapOffset,
                ColumnSnapOffset = ColumnSnapOffset,
                RowSnapOffset = RowSnapOffset,
                DragInterval = DragInterval,
                RowDragInterval = RowDragInterval,
                ColumnDragInterval = ColumnDragInterval,
                Cursor = Cursor,
                ColumnCursor = ColumnCursor,
                RowCursor = RowCursor,
                HasOnDrag = OnDrag.HasDelegate,
                HasOnDragStart = OnDragStart.HasDelegate,
                HasOnDragStop = OnDragStop.HasDelegate
            };

            await _splitGrid.Initialise(_rows.Values, _columns.Values, options);
            _splitGrid.OnDrag += (_, args) => OnDrag.InvokeAsync(args);
            _splitGrid.OnDragStart += (_, args) => OnDragStart.InvokeAsync(args);
            _splitGrid.OnDragStop += (_, args) => OnDragStop.InvokeAsync(args);
        }
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }

    internal GutterItem AddRow(SplitGridGutter gutter)
    {
        var gutterItem = new GutterItem(gutter.Id, _rows.NextTrack());

        if (gutter.MinSize.HasValue && RowMinSizes is not null)
            RowMinSizes[gutterItem.Track] = gutter.MinSize.Value;

        if (gutter.MaxSize.HasValue && RowMaxSizes is not null)
            RowMaxSizes[gutterItem.Track] = gutter.MaxSize.Value;

        return _rows[gutter.Id] = gutterItem;
    }

    internal GutterItem AddColumn(SplitGridGutter gutter)
    {
        var gutterItem = new GutterItem(gutter.Id, _columns.NextTrack());

        if (gutter.MinSize.HasValue && ColumnMinSizes is not null)
            ColumnMinSizes[gutterItem.Track] = gutter.MinSize.Value;

        if (gutter.MaxSize.HasValue && ColumnMaxSizes is not null)
            ColumnMaxSizes[gutterItem.Track] = gutter.MaxSize.Value;

        return _columns[gutter.Id] = gutterItem;
    }
}