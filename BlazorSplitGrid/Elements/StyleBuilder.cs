namespace BlazorSplitGrid.Elements;

internal class StyleBuilder
{
    private const char Separator = ';';

    private readonly List<string> _values;

    public static StyleBuilder New()
    {
        return new StyleBuilder(new List<string>());
    }
    
    public static StyleBuilder For(string value)
    {
        var values = value
            .Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        return new StyleBuilder(values);
    }

    private StyleBuilder(List<string> values)
    {
        _values = values;
    }

    public StyleBuilder Append(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            _values.Add(value.Trim().TrimEnd(';'));

        return this;
    }

    public StyleBuilder Append(IEnumerable<string> values)
    {
        foreach (var value in values)
            Append(value);

        return this;
    }

    public StyleBuilder ConditionalAppend(Func<bool> condition, string? value)
    {
        if (condition() && !string.IsNullOrWhiteSpace(value))
            _values.Add(value.Trim().TrimEnd(Separator));

        return this;
    }

    public string Build()
    {
        return _values.Count != 0 ? $"{string.Join($"{Separator} ", _values)}{Separator}" : string.Empty;
    }

    public override string ToString()
    {
        return Build();
    }
}
