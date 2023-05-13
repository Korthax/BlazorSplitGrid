using BlazorSplitGrid.Elements;
using FluentAssertions;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class AttributeBuilderTests
{
    [Fact]
    public void ShouldJoinMultipleValuesWithASpace()
    {
        var attributeBuilder = AttributeBuilder.New();
        attributeBuilder.Append("one");
        attributeBuilder.Append("two");

        var result = attributeBuilder.Build();
        result.Should().Be("one two");
    }

    [Fact]
    public void ShouldNotAppendNullValue()
    {
        var attributeBuilder = AttributeBuilder.New();
        attributeBuilder.Append(null);

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsFalse()
    {
        var attributeBuilder = AttributeBuilder.New();
        attributeBuilder.ConditionalAppend(() => false, "one");

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsTrue()
    {
        var attributeBuilder = AttributeBuilder.New();
        attributeBuilder.ConditionalAppend(() => true, "one");

        var result = attributeBuilder.Build();
        result.Should().Be("one");
    }
}
