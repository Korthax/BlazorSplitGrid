using BlazorSplitGrid.Models;

namespace BlazorSplitGrid.Interop;

internal interface ISplitGridInterop
{
    Task Initialise(IEnumerable<Track> rowGutters, IEnumerable<Track> columnGutters, SplitGridOptions options);
}
