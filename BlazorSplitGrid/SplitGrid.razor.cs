using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorSplitGrid;

public partial class SplitGrid : ComponentBase
{
    [Parameter]
    public EventCallback? OnDrag { get; set; }

    [Parameter]
    public EventCallback<DragEventArgs>? OnDragStart { get; set; }

    [Parameter]
    public EventCallback<DragEventArgs>? OnDragStop { get; set; }

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter] 
    public int? MinSize { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? MinSizes { get; set; }

    [Parameter] 
    public int? MaxSize { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? MaxSizes { get; set; }

    [Parameter] 
    public int? ColumnMinSize { get; set; }
    
    [Parameter] 
    public int? ColumnMaxSize { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? ColumnMinSizes { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? ColumnMaxSizes { get; set; }
    
    [Parameter] 
    public int? RowMinSize { get; set; }
    
    [Parameter] 
    public int? RowMaxSize { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? RowMinSizes { get; set; }
    
    [Parameter] 
    public Dictionary<int, int>? RowMaxSizes { get; set; }
    
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
    public bool Initialised { get; set; }

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
                RowMinSize = RowMinSize,
                RowMaxSize = RowMaxSize,
                SnapOffset = SnapOffset,
                ColumnSnapOffset = ColumnSnapOffset,
                RowSnapOffset = RowSnapOffset,
                DragInterval = DragInterval,
                RowDragInterval = RowDragInterval,
                ColumnDragInterval = ColumnDragInterval,
                Cursor = Cursor,
                ColumnCursor = ColumnCursor,
                RowCursor = RowCursor,
                HasOnDrag = OnDrag is not null,
                HasOnDragStart = OnDragStart is not null,
                HasOnDragStop = OnDragStop is not null
            };

            await _splitGrid.Initialise(_rows.Values, _columns.Values, options);
            _splitGrid.OnDrag += (_, args) => OnDrag?.InvokeAsync(args);
            _splitGrid.OnDragStart += (_, args) => OnDragStart?.InvokeAsync(args);
            _splitGrid.OnDragStop += (_, args) => OnDragStop?.InvokeAsync(args);

            Initialised = true;
        }
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }

    internal GutterItem AddRow(string selector)
    {
        return _rows[selector] = new GutterItem(selector, _rows.NextTrack());
    }

    internal GutterItem AddColumn(string selector)
    {
        return _columns[selector] = new GutterItem(selector, _columns.NextTrack());
    }
}
