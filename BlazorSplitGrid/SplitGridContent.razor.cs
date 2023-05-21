using BlazorSplitGrid.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSplitGrid;

public class SplitGridContent : ComponentBase
{
    [CascadingParameter]
    public SplitGrid SplitGrid { get; set; } = null!;

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    public string SplitGridId { get; }

    public string Classes => AttributeBuilder.New()
        .Append(SplitGridId)
        .Append("split-grid-content")
        .Append(Class)
        .Build();

    public string Styles => AttributeBuilder.New()
        .Append(Style)
        .Build();

    public SplitGridContent()
    {
        SplitGridId = $"split-grid-content-{Guid.NewGuid().ToString()}";
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
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
