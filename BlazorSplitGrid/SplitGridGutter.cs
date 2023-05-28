using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSplitGrid;

public class SplitGridGutter : SplitGridComponentBase
{
    [CascadingParameter]
    public SplitGrid SplitGrid { get; set; } = null!;

    [Parameter]
    public decimal? MinSize { get; set; }

    [Parameter]
    public decimal? MaxSize { get; set; }

    [Parameter]
    public decimal Size { get; set; } = 10;

    [Parameter]
    public Direction Direction { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    protected override string Classes => ClassBuilder
        .Append("split-grid-gutter")
        .Append($"split-grid-gutter-{Direction.ToClassName()}")
        .ConditionalAppend(() => _item is not null, $"split-grid-gutter-{Direction.ToClassName()}-{_item!.Number}")
        .ConditionalAppend(() => Disabled, "disabled")
        .Build();

    private Track? _item;

    public async Task SetSize(decimal size)
    {
        await SplitGrid.SetSize(Direction, SplitGridId, size);
    }

    public Task Disable()
    {
        Disabled = true;
        return Task.CompletedTask;
    }

    public Task Enable()
    {
        Disabled = false;
        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        _item = Direction == Direction.Row ? SplitGrid.AddRow(this) : SplitGrid.AddColumn(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var sequence = 0;
        builder.OpenElement(sequence++, "div");

        if (!string.IsNullOrWhiteSpace(Id))
            builder.AddAttribute(sequence++, "id", Id);

        builder.AddAttribute(sequence++, "class", Classes);
        builder.AddAttribute(sequence, "style", Styles);
        builder.CloseElement();
    }
}
