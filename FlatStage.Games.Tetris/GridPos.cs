namespace FlatStage.Tetris;

public struct GridPos
{
    public int Row { get; }
    public int Column { get; }

    public GridPos(int row, int column)
    {
        Row = row;
        Column = column;
    }
}