using BlazorSplitGrid.Elements;
using BlazorSplitGrid.Interop;
using BlazorSplitGrid.Models;
using FluentAssertions;
using Moq;
using Snapshooter.Xunit;
using Xunit;

namespace BlazorSplitGrid.Tests;

public class GridTests
{
    private readonly Mock<ISplitGridInterop> _splitGridInterop;
    private IEnumerable<Track> _columnTracks = null!;
    private IEnumerable<Track> _rowTracks = null!;
    private SplitGridOptions _options = null!;

    public GridTests()
    {
        _splitGridInterop = new Mock<ISplitGridInterop>();
        _splitGridInterop
            .Setup(x => x.Initialise(It.IsAny<IEnumerable<Track>>(), It.IsAny<IEnumerable<Track>>(), It.IsAny<SplitGridOptions>()))
            .Callback((IEnumerable<Track> rowTracks, IEnumerable<Track> columnTracks, SplitGridOptions options) =>
            {
                _rowTracks = rowTracks;
                _columnTracks = columnTracks;
                _options = options;
            })
            .Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task ShouldBeAbleToBuildGrid()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());
        grid.AddContent(new SplitGridContent());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());
        grid.AddContent(new SplitGridContent());
        grid.AddContent(new SplitGridContent());
        
        await grid.Initialise(_splitGridInterop.Object);
        _options.Css.Should().MatchSnapshot();
    }
    
    [Fact]
    public async Task ShouldBeAbleToBuildGridWithSingleRow()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());

        await grid.Initialise(_splitGridInterop.Object);
        _options.Css.Should().MatchSnapshot();
    }

    [Fact]
    public async Task ShouldBeAbleToBuildGridWithSingleColumn()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());

        await grid.Initialise(_splitGridInterop.Object);
        _options.Css.Should().MatchSnapshot();
    }

    [Fact]
    public void ShouldBeAbleToUpdateGutterSize()
    {
        var splitGridColumn = new SplitGridColumn();

        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(splitGridColumn);
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Column, splitGridColumn.SplitGridId, "0px");

        var css = grid.Template(Direction.Column);
        css.Should().Be("1fr 0px 1fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateContentSize()
    {
        var splitGridContent = new SplitGridContent();

        var grid = Grid.New(new SplitGrid());
        grid.AddContent(splitGridContent);
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Column, splitGridContent.SplitGridId, "0fr");

        var css = grid.Template(Direction.Column);
        css.Should().Be("0fr 10px 1fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateColumnTrackSize()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Column, 1, "0px");

        var css = grid.Template(Direction.Column);
        css.Should().Be("1fr 0px 1fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateRowTrackSize()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Row, 1, "0px");

        var css = grid.Template(Direction.Row);
        css.Should().Be("1fr 0px 1fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateContentTrackSize()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Row, 0, "0fr");

        var css = grid.Template(Direction.Row);
        css.Should().Be("0fr 10px 1fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateRowSizes()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Row, "0fr 0px 0fr");

        var css = grid.Template(Direction.Row);
        css.Should().Be("0fr 0px 0fr");
    }

    [Fact]
    public void ShouldBeAbleToUpdateColumnSizes()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());

        grid.Update(Direction.Column, "0fr 0px 0fr");

        var css = grid.Template(Direction.Column);
        css.Should().Be("0fr 0px 0fr");
    }

    [Fact]
    public void ShouldBeAbleToGetColumnSizeById()
    {
        var splitGridColumn = new SplitGridColumn();

        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(splitGridColumn);
        grid.AddContent(new SplitGridContent());

        var size = grid.GetSize(Direction.Column, splitGridColumn.SplitGridId);
        size.Should().Be("10px");
    }

    [Fact]
    public void ShouldBeAbleToGetColumnSizeByTrack()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddColumnGutter(new SplitGridColumn());
        grid.AddContent(new SplitGridContent());

        var size = grid.GetSize(Direction.Column, 2);
        size.Should().Be("1fr");
    }

    [Fact]
    public void ShouldBeAbleToGetRowSizeById()
    {
        var splitGridColumn = new SplitGridRow();

        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(splitGridColumn);
        grid.AddContent(new SplitGridContent());

        var size = grid.GetSize(Direction.Row, splitGridColumn.SplitGridId);
        size.Should().Be("10px");
    }

    [Fact]
    public void ShouldBeAbleToGetRowSizeByTrack()
    {
        var grid = Grid.New(new SplitGrid());
        grid.AddContent(new SplitGridContent());
        grid.AddRowGutter(new SplitGridRow());
        grid.AddContent(new SplitGridContent());

        var size = grid.GetSize(Direction.Row, 2);
        size.Should().Be("1fr");
    }
}
