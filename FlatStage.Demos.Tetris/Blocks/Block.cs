using System.Collections.Generic;

namespace FlatStage.Tetris;

public abstract class Block
{
    public int Id { get; }

    protected abstract GridPos[][] Tiles { get; }

    public int OffsetCol { get; private set; }

    public int OffsetRow { get; private set; }

    private int StartOffsetCol { get; }

    private int StartOffsetRow { get; }

    private int rotationState;

    protected Block(int id, int startOffsetColumn, int startOffsetRow)
    {
        Id = id;

        StartOffsetCol = startOffsetColumn;
        StartOffsetRow = startOffsetRow;

        OffsetCol = StartOffsetCol;
        OffsetRow = StartOffsetRow;
    }

    public IEnumerable<GridPos> TilePositions()
    {
        return Tiles[rotationState];
    }

    public void RotateCW()
    {
        rotationState = (rotationState + 1) % Tiles.Length;
    }

    public void RotateCCW()
    {
        if (rotationState == 0)
        {
            rotationState = Tiles.Length - 1;
        }
        else
        {
            rotationState--;
        }
    }

    public void Move(int rows, int columns)
    {
        OffsetCol += columns;
        OffsetRow += rows;
    }

    public void Reset()
    {
        rotationState = 0;
        OffsetCol = StartOffsetCol;
        OffsetRow = StartOffsetRow;
    }
}