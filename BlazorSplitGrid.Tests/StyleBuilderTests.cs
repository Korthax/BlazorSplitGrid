using AwesomeAssertions;
using BlazorSplitGrid.Elements;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class StyleBuilderTests
{
    [Fact]
    public void ShouldJoinMultipleValuesWithASpace()
    {
        var attributeBuilder = StyleBuilder.New();
        attributeBuilder.Append("one");
        attributeBuilder.Append("two");

        var result = attributeBuilder.Build();
        result.Should().Be("one; two;");
    }

    [Fact]
    public void ShouldBeAbleToAppendCollections()
    {
        var attributeBuilder = StyleBuilder.New();
        attributeBuilder.Append(new List<string> { "three", "four" });

        var result = attributeBuilder.Build();
        result.Should().Be("three; four;");
    }

    [Fact]
    public void ShouldHandleInitialValue()
    {
        var attributeBuilder = StyleBuilder.For("big; small;");
        attributeBuilder.Append("tiny");

        var result = attributeBuilder.Build();
        result.Should().Be("big; small; tiny;");
    }

    [Fact]
    public void ShouldNotAppendNullValue()
    {
        var attributeBuilder = StyleBuilder.New();
        attributeBuilder.Append((string?)null);

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsFalse()
    {
        var attributeBuilder = StyleBuilder.New();
        attributeBuilder.ConditionalAppend(() => false, "one");

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsTrue()
    {
        var attributeBuilder = StyleBuilder.New();
        attributeBuilder.ConditionalAppend(() => true, "one");

        var result = attributeBuilder.Build();
        result.Should().Be("one;");
    }
}
