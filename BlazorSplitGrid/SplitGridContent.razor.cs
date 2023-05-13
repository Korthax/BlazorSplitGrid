using BlazorSplitGrid.Elements;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorSplitGrid;

public partial class SplitGridContent : ComponentBase
{
    [CascadingParameter]
    public SplitGrid SplitGrid { get; set; } = null!;

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    public string Classes => AttributeBuilder.New()
        .Append("split-grid-content")
        .Append(Class)
        .Build();

    public string Styles => AttributeBuilder.New()
        .Append(Style)
        .Build();

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", Classes);
        builder.AddAttribute(2, "style", Styles);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}
