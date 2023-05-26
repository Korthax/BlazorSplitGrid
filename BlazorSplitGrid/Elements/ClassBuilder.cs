namespace BlazorSplitGrid.Elements;

internal class ClassBuilder
{
    private const string Separator = " ";

    private readonly HashSet<string> _values;

    public static ClassBuilder New()
    {
        return new ClassBuilder(new HashSet<string>());
    }

    public static ClassBuilder For(string value)
    {
        var values = value
            .Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet();

        return new ClassBuilder(values);
    }

    private ClassBuilder(HashSet<string> values)
    {
        _values = values;
    }

    public ClassBuilder Append(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            _values.Add(value.Trim());

        return this;
    }

    public void Remove(string className)
    {
        _values.Remove(className);
    }

    public ClassBuilder Append(IEnumerable<string> values)
    {
        foreach (var value in values)
            Append(value);

        return this;
    }

    public ClassBuilder Append(IEnumerable<Action<ClassBuilder>> values)
    {
        foreach (var value in values)
            value(this);

        return this;
    }

    public ClassBuilder ConditionalAppend(Func<bool> condition, string? value)
    {
        if (condition() && !string.IsNullOrWhiteSpace(value))
            _values.Add(value.Trim());

        return this;
    }

    public string Build()
    {
        return _values.Count != 0 ? string.Join(Separator, _values) : string.Empty;
    }

    public override string ToString()
    {
        return Build();
    }
}
