using BlazorSplitGrid.Elements;
using BlazorSplitGrid.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSplitGrid;

public class SplitGridContent : SplitGridComponentBase
{
    [CascadingParameter]
    public SplitGrid SplitGrid { get; set; } = null!;

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public decimal Size { get; set; } = 1;

    private (Track Row, Track Column) _tracks;

    protected override string Classes => ClassBuilder
        .Append("split-grid-content")
        .Append($"split-grid-content-row-{_tracks.Row.Number}")
        .Append($"split-grid-content-column-{_tracks.Column.Number}")
        .Build();

    public async Task SetSize(Direction direction, decimal size)
    {
        await SplitGrid.SetSize(direction, SplitGridId, size);
    }

    protected override void OnInitialized()
    {
        _tracks = SplitGrid.AddContent(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var sequence = 0;
        builder.OpenElement(sequence++, "div");

        if (!string.IsNullOrWhiteSpace(Id))
            builder.AddAttribute(sequence++, "id", Id);

        builder.AddAttribute(sequence++, "class", Classes);
        builder.AddAttribute(sequence++, "style", Styles);
        builder.AddContent(sequence, ChildContent);
        builder.CloseElement();
    }
}
