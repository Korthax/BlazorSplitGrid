using System.Diagnostics.CodeAnalysis;
using BlazorSplitGrid.Models;
using Microsoft.JSInterop;

namespace BlazorSplitGrid.Interop;

internal class SplitGridInterop : IAsyncDisposable, ISplitGridInterop
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

    public async Task Initialise(IEnumerable<Track> rowGutters, IEnumerable<Track> columnGutters, SplitGridOptions options)
    {
        var module = await _moduleTask.Value;
        var interopRef = DotNetObjectReference.Create(this);
        _gridInstance = await module.InvokeAsync<IJSObjectReference>("initSplitGrid", rowGutters, columnGutters, options.ToInteroperable(), interopRef);
    }

    public async Task<IJSObjectReference> CreateResizeObserver<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(string dotnetMethodName, DotNetObjectReference<T> interopRef) where T : class =>
        await (await _moduleTask.Value).InvokeAsync<IJSObjectReference>("createResizeObserver", dotnetMethodName, interopRef);

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async Task AddColumnGutter(string querySelector, int track)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("addColumnGutterByQuerySelector", querySelector, track);
    }

    public async Task AddRowGutter(string querySelector, int track)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("addRowGutterByQuerySelector", querySelector, track);
    }

    public async Task RemoveColumnGutter(string querySelector, int track, bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("removeColumnGutterByQuerySelector", querySelector, track, immediate);
    }

    public async Task RemoveRowGutter(string querySelector, int track, bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("removeRowGutterByQuerySelector", querySelector, track, immediate);
    }

    public async Task Destroy(bool immediate = true)
    {
        if (_gridInstance is null)
            return;

        await _gridInstance!.InvokeVoidAsync("destroy", immediate);
    }

    public async Task SetSizes(string querySelector, string templateName, string sizes)
    {
        if (_gridInstance is null || string.IsNullOrWhiteSpace(sizes))
            return;

        await _gridInstance!.InvokeVoidAsync("setGridSizes", querySelector, templateName, sizes);
    }

    public async Task<string> GetSizes(string querySelector, string templateName)
    {
        if (_gridInstance is null)
            return string.Empty;

        return await _gridInstance!.InvokeAsync<string>("getGridSizes", querySelector, templateName);
    }

    [JSInvokable]
    public void OnDragFired(string direction, int track, string? gridTemplateStyle)
    {
        OnDrag?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track, gridTemplateStyle));
    }

    [JSInvokable]
    public void OnDragStartFired(string direction, int track, string? gridTemplateStyle)
    {
        OnDragStart?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track, gridTemplateStyle));
    }

    [JSInvokable]
    public void OnDragEndFired(string direction, int track, string? gridTemplateStyle)
    {
        OnDragStop?.Invoke(this, new DragEventArgs(Enum.Parse<Direction>(direction, true), track, gridTemplateStyle));
    }
}
