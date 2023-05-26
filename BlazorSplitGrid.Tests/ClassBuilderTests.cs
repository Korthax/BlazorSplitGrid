using BlazorSplitGrid.Elements;
using FluentAssertions;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class ClassBuilderTests
{
    [Fact]
    public void ShouldJoinMultipleValuesWithASpace()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.Append("one");
        attributeBuilder.Append("two");

        var result = attributeBuilder.Build();
        result.Should().Be("one two");
    }
    
    [Fact]
    public void ShouldBeAbleToAppendCollections()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.Append(new List<string> { "three", "four" });

        var result = attributeBuilder.Build();
        result.Should().Be("three four");
    }
    
    [Fact]
    public void ShouldBeAbleToAppendAdditionalActions()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.Append(new List<Action<ClassBuilder>> { x => x.Append("three") });

        var result = attributeBuilder.Build();
        result.Should().Be("three");
    }
    
    [Fact]
    public void ShouldHandleInitialValue()
    {
        var attributeBuilder = ClassBuilder.For("big small");
        attributeBuilder.Append("tiny");

        var result = attributeBuilder.Build();
        result.Should().Be("big small tiny");
    }

    [Fact]
    public void ShouldNotAppendNullValue()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.Append((string?)null);

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendTheSameValue()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.Append("one");
        attributeBuilder.Append("one");

        var result = attributeBuilder.Build();
        result.Should().Be("one");
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsFalse()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.ConditionalAppend(() => false, "one");

        var result = attributeBuilder.Build();
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotAppendIfConditionIsTrue()
    {
        var attributeBuilder = ClassBuilder.New();
        attributeBuilder.ConditionalAppend(() => true, "one");

        var result = attributeBuilder.Build();
        result.Should().Be("one");
    }
}
