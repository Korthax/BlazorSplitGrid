namespace BlazorSplitGrid.Models;

public record SizeConstraints
{
    public int Min { get; }
    public int Max { get; }
    public int RowMin { get; }
    public int RowMax { get; }
    public int ColumnMin { get; }
    public int ColumnMax { get; }

    public SizeConstraints(int min, int max, int? rowMin, int? rowMax, int? columnMin, int? columnMax)
    {
        Min = min;
        Max = max;
        RowMin = rowMin ?? min;
        RowMax = rowMax ?? max;
        ColumnMin = columnMin ?? min;
        ColumnMax = columnMax ?? max;
    }
}
