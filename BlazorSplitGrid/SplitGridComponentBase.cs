using BlazorSplitGrid.Elements;
using Microsoft.AspNetCore.Components;

namespace BlazorSplitGrid;

public abstract class SplitGridComponentBase : ComponentBase
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; }

    [Parameter]
    public string? Style { get; set; }

    public string SplitGridId { get; internal set; }

    protected virtual string Classes => ClassBuilder.Build();
    protected virtual string Styles => StyleBuilder.Build();

    internal StyleBuilder StyleBuilder => StyleBuilder.New()
        .Append(Style);

    internal ClassBuilder ClassBuilder => ClassBuilder.New()
        .Append(SplitGridId)
        .Append(Class)
        .Append(_additionalClasses);

    private readonly List<Action<ClassBuilder>> _additionalClasses;

    protected SplitGridComponentBase()
    {
        SplitGridId = $"split-grid-id-{Guid.NewGuid().ToString()}";
        _additionalClasses = new List<Action<ClassBuilder>>();
    }

    public void AddClass(string className)
    {
        _additionalClasses.Add(classBuilder => classBuilder.Append(className));
    }

    public void RemoveClass(string className)
    {
        _additionalClasses.Add(classBuilder => classBuilder.Remove(className));
    }

    public async Task Refresh()
    {
        await InvokeAsync(StateHasChanged);
    }
}
