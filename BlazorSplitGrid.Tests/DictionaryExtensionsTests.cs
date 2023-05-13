using BlazorSplitGrid.Extensions;
using BlazorSplitGrid.Models;
using FluentAssertions;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class DictionaryExtensionsTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 3)]
    [InlineData(2, 5)]
    [InlineData(10, 21)]
    public void ShouldReturnTheNextTrackToTheNextOddNumber(int itemCount, int expectedNextTrack)
    {
        var dictionary = new Dictionary<string, GutterItem>();
        for (var i = 0; i < itemCount; i++)
            dictionary.Add($"{i}", new GutterItem("id", 0));

        var result = dictionary.NextTrack();
        result.Should().Be(expectedNextTrack);
    }

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
