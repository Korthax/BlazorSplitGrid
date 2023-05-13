using BlazorSplitGrid.Models;
using Microsoft.JSInterop;

namespace BlazorSplitGrid;

internal class SplitGridInterop : IAsyncDisposable
{
    private const string ContentPath = "./_content/BlazorSplitGrid/splitGridInterop.js";

    public event EventHandler<DragEventArgs>? OnDragStart;
    public event EventHandler<DragEventArgs>? OnDragStop;
    public event EventHandler<DragEventArgs>? OnDrag;

    private readonly Lazy<ValueTask<IJSObjectReference>> _moduleTask;
    private IJSObjectReference? _gridInstance;

    public SplitGridInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<ValueTask<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>("import", ContentPath));
    }

    public async Task Initialise(IEnumerable<GutterItem> rows, IEnumerable<GutterItem> columns, SplitGridOptions options)
    {
        var module = await _moduleTask.Value;
        var interopRef = DotNetObjectReference.Create(this);
        _gridInstance = await module.InvokeAsync<IJSObjectReference>("initSplitGrid", rows, columns, options.ToInteroperable(), interopRef);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async Task AddColumnGutter(string id, int track)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("addColumnGutterById", id, track);
    }

    public async Task AddRowGutter(string id, int track)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("addRowGutterById", id, track);
    }

    public async Task RemoveColumnGutter(string id, int track, bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("removeColumnGutterById", id, track, immediate);
    }

    public async Task RemoveRowGutter(string id, int track, bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("removeRowGutterById", id, track, immediate);
    }

    public async Task RemoveRowGutter(bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("destroy", immediate);
    }

    [JSInvokable]
    public void OnDragFired(string direction, int track, string gridTemplateStyle)
    {
        OnDrag?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track, gridTemplateStyle));
    }

    [JSInvokable]
    public void OnDragStartFired(string direction, int track)
    {
        OnDragStart?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track));
    }

    [JSInvokable]
    public void OnDragEndFired(string direction, int track)
    {
        OnDragStop?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track));
    }
}
