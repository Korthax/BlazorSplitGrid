using BlazorSplitGrid.Extensions;
using FluentAssertions;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void ShouldNotAddItemIfDictionaryIsEmpty()
    {
        var items = new Dictionary<string, object>();
        items.AddIfNotNull("key", new Dictionary<int, int>());
        items.Should().HaveCount(0);
    }

    [Fact]
    public void ShouldNotAddItemIfDictionaryHasItems()
    {
        var items = new Dictionary<string, object>();
        items.AddIfNotNull("key", new Dictionary<int, int> { [1] = 1 });
        items.Should().HaveCount(1);
    }

    [Fact]
    public void ShouldNotAddItemIfValueIsNull()
    {
        var items = new Dictionary<string, object>();
        items.AddIfNotNull("key", null);
        items.Should().HaveCount(0);
    }

    [Fact]
    public void ShouldNotAddItemIfValueIsNotNull()
    {
        var items = new Dictionary<string, object>();
        items.AddIfNotNull("key", 1);
        items.Should().HaveCount(1);
    }
}
