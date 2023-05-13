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

You are able to specify all the [Split Grid options](https://github.com/nathancahill/split/tree/master/packages/split-grid#options) on the top level `SplitGrid` component except the row and column gutters.
