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
    public int? MinContentSize { get; set; }

    [Parameter]
    public int? MaxContentSize { get; set; }

    [Parameter]
    public int Size { get; set; } = 10;

    [Parameter]
    public Direction Direction { get; set; }

    protected override string Classes => ClassBuilder
        .Append("split-grid-gutter")
        .Append($"split-grid-gutter-{Direction.ToClassName()}")
        .ConditionalAppend(() => _item is not null, $"split-grid-gutter-{Direction.ToClassName()}-{_item!.Track}")
        .Build();

    private GutterItem? _item;

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
