namespace BlazorSplitGrid.Models;

public record SizeConstraints
{
    public decimal Min { get; }
    public decimal Max { get; }
    public decimal RowMin { get; }
    public decimal RowMax { get; }
    public decimal ColumnMin { get; }
    public decimal ColumnMax { get; }

    public SizeConstraints(decimal min, decimal max, decimal? rowMin, decimal? rowMax, decimal? columnMin, decimal? columnMax)
    {
        Min = min;
        Max = max;
        RowMin = rowMin ?? min;
        RowMax = rowMax ?? max;
        ColumnMin = columnMin ?? min;
        ColumnMax = columnMax ?? max;
    }
}
