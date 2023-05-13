namespace BlazorSplitGrid.Elements;

public class AttributeBuilder
{
    private readonly List<string> _classes;

    public static AttributeBuilder New()
    {
        return new AttributeBuilder(new List<string>());
    }
    
    public static AttributeBuilder For(string className)
    {
        return new AttributeBuilder(new List<string> { className });
    }

    private AttributeBuilder(List<string> classes)
    {
        _classes = classes;
    }

    public AttributeBuilder Append(string? className)
    {
        if (!string.IsNullOrWhiteSpace(className))
            _classes.Add(className);

        return this;
    }

    public AttributeBuilder ConditionalAppend(Func<bool> condition, string? className)
    {
        if (condition() && !string.IsNullOrWhiteSpace(className))
            _classes.Add(className);

        return this;
    }

    public string Build()
    {
        return string.Join(' ', _classes);
    }

    public override string ToString()
    {
        return Build();
    }
}
