[![Version](https://img.shields.io/nuget/v/BlazorSplitGrid?style=plastic)](https://www.nuget.org/packages/BlazorSplitGrid/)
[![Nuget downloads](https://img.shields.io/nuget/dt/BlazorSplitGrid?label=nuget%20downloads&logo=nuget&style=plastic)](https://www.nuget.org/packages/BlazorSplitGrid/)
# Blazor Split Grid

A basic Blazor component wrapper around [Split Grid](https://github.com/nathancahill/split).

## Getting Started

### Quick Installation Guide

1. Install the Nuget Package
   ```
   dotnet add package BlazorSplit
   ```
2. Add namespace reference to your `_Imports.razor`
   ```
   @using BlazorSplitGrid
   ```

### Optional CSS

You can add basic gutter styling by including the following link in your html head section

```html
<link href="_content/BlazorSplitGrid/BlazorSplitGrid.min.css" rel="stylesheet" />
```

## Usage

Blazor Split Grid will automatically generate the track number and classes for your gutters so all you need to specify is whether the gutter is a row or column.

```html
<SplitGrid SnapOffset="0" DragInterval="1" MaxSize="850">
    <SplitGridContent></SplitGridContent>
    <SplitGridColumn />
    <SplitGridContent></SplitGridContent>
    <SplitGridContent></SplitGridContent>
    <SplitGridRow />
    <SplitGridContent></SplitGridContent>
</SplitGrid>
```

### Example

See the [example project](./BlazorSplitGrid.Example) for a more comprehensive example.

## API Reference

### SplitGrid
You are able to specify all the [Split Grid](https://github.com/nathancahill/split/tree/master/packages/split-grid#options) options on the top level `SplitGrid` component except the row and column gutters.

| Attribute          |         Type         |            Default            | Description                                                                                                                                                                                                                                        |
|--------------------|:--------------------:|:-----------------------------:|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| MinSize            |         int          |               0               | The minimum size in pixels for all tracks.                                                                                                                                                                                                         |
| MaxSize            |         int          |           Infinity            | The maximum size in pixels for all tracks.                                                                                                                                                                                                         |
| ColumnMinSize      |         int          |            MinSize            | The minimum size in pixels for all tracks.                                                                                                                                                                                                         |
| RowMinSize         |         int          |            MinSize            | The minimum size in pixels for all tracks.                                                                                                                                                                                                         |
| ColumnMaxSize      |         int          |            MaxSize            | The maximum size in pixels for all tracks.                                                                                                                                                                                                         |
| RowMaxSize         |         int          |            MaxSize            | The maximum size in pixels for all tracks.                                                                                                                                                                                                         |
| ColumnMinSizes     | Dictionary<int, int> |         ColumnMinSize         | An object keyed by track index, with values set to the minimum size in pixels for the track at that index. Allows individual minSizes to be specified by track. Note this option is plural with an s, while the two fallback options are singular. |
| RowMinSizes        | Dictionary<int, int> |          RowMinSize           | object keyed by track index, with values set to the minimum size in pixels for the track at that index. Allows individual minSizes to be specified by track. Note this option is plural with an s, while the two fallback options are singular.    |
| ColumnMaxSizes     | Dictionary<int, int> |         ColumnMaxSize         | An object keyed by track index, with values set to the maximum size in pixels for the track at that index. Allows individual maxSizes to be specified by track. Note this option is plural with an s, while the two fallback options are singular. |
| RowMaxSizes        | Dictionary<int, int> |          RowMaxSize           | An object keyed by track index, with values set to the maximum size in pixels for the track at that index. Allows individual maxSizes to be specified by track. Note this option is plural with an s, while the two fallback options are singular. |
| SnapOffset         |         int          |              30               | Snap to minimum size at this offset in pixels. Set to 0 to disable snap.                                                                                                                                                                           |
| ColumnSnapOffset   |         int          |          SnapOffset           | Snap to minimum size at this offset in pixels. Set to 0 to disable snap.                                                                                                                                                                           |
| RowSnapOffset      |         int          |          SnapOffset           | Snap to minimum size at this offset in pixels. Set to 0 to disable snap.                                                                                                                                                                           |
| DragInterval       |         int          |               1               | Drag this number of pixels at a time. Defaults to 1 for smooth dragging, but can be set to a pixel value to give more control over the resulting sizes.                                                                                            |
| ColumnDragInterval |         int          |         DragInterval          | Drag this number of pixels at a time. Defaults to 1 for smooth dragging, but can be set to a pixel value to give more control over the resulting sizes.                                                                                            |
| RowDragInterval    |         int          |         DragInterval          | Drag this number of pixels at a time. Defaults to 1 for smooth dragging, but can be set to a pixel value to give more control over the resulting sizes.                                                                                            |
| Cursor             |        string        | 'col-resize' and 'row-resize' | Cursor to show while dragging. Defaults to 'col-resize' for column gutters and 'row-resize' for row gutters.                                                                                                                                       |
| ColumnCursor       |        string        |         'col-resize'          | Cursor to show while dragging.                                                                                                                                                                                                                     |
| RowCursor          |        string        |         'row-resize'          | Cursor to show while dragging.                                                                                                                                                                                                                     |
| OnDrag             |    DragEventArgs     |               -               | Called continuously on drag. For process intensive code, add a debounce function to rate limit this callback. gridTemplateStyle is the computed CSS value for grid-template-column or grid-template-row, depending on direction.                   |
| OnDragStart        |    DragEventArgs     |               -               | Called on drag start.                                                                                                                                                                                                                              |
| OnDragEnd          |    DragEventArgs     |               -               | Called on drag end.                                                                                                                                                                                                                                |

### SplitGridRow / SplitGridColumn

You can specify the MinSize and MaxSize on the gutters which is automatically added to the ColumnMinSizes/RowMinSizes and ColumnMaxSizes/RowMaxSizes properties.

| Attribute | Type | Default  | Description                                |
|-----------|:----:|:--------:|--------------------------------------------|
| MinSize   | int  |    0     | The minimum size in pixels for this track. |
| MaxSize   | int  | Infinity | The maximum size in pixels for this track. |
