namespace BlazorSplitGrid.Models;

public class GridPosition
{
    public int Row { get; }
    public int Column { get; }

    public GridPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }
}
