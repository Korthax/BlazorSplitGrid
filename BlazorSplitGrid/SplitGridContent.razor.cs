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

    private GridPosition? _position;

    protected override string Classes => ClassBuilder
        .Append("split-grid-content")
        .ConditionalAppend(() => _position is not null, $"split-grid-content-{_position!.Row}-{_position!.Column}")
        .Build();

    public async Task SetSize(int size)
    {
        await InvokeAsync(StateHasChanged);
    }

    protected override void OnInitialized()
    {
        _position = SplitGrid.AddContent(this);
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
