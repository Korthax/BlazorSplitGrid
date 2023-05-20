using BlazorSplitGrid.Elements;
using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSplitGrid;

public class SplitGridGutter : ComponentBase
{
    [CascadingParameter]
    public SplitGrid SplitGrid { get; set; } = null!;

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public int? MinSize { get; set; }

    [Parameter]
    public int? MaxSize { get; set; }

    [Parameter]
    public Direction Direction { get; init; }

    public string Id { get; }

    private string Classes => AttributeBuilder.New()
        .Append("split-grid-gutter")
        .Append($"split-grid-gutter-{Direction.ToClassName()}")
        .ConditionalAppend(() => _item is not null, $"split-grid-gutter-{Direction.ToClassName()}-{_item!.Track}")
        .Append(Class)
        .Build();

    private GutterItem? _item;

    protected SplitGridGutter()
    {
        Id = $"split-grid-gutter-{Guid.NewGuid().ToString()}";
    }

    protected override void OnInitialized()
    {
        _item = Direction == Direction.Row ? SplitGrid.AddRow(this) : SplitGrid.AddColumn(this);
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "id", Id);
        builder.AddAttribute(2, "class", Classes);
        builder.AddAttribute(3, "style", Style);
        builder.CloseElement();
    }
}
