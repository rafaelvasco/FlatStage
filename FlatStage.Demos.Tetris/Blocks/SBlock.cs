namespace FlatStage.Tetris;

public sealed class SBlock : Block
{
    private readonly GridPos[][] _tiles =
    {
        new GridPos[] { new(0, 1), new(0, 2), new(1, 0), new(1, 1) },
        new GridPos[] { new(0, 1), new(1, 1), new(1, 2), new(2, 2) },
        new GridPos[] { new(1, 1), new(1, 2), new(2, 0), new(2, 1) },
        new GridPos[] { new(0, 0), new(1, 0), new(1, 1), new(2, 1) }
    };

    protected override GridPos[][] Tiles => _tiles;

    public SBlock() : base(id: 5, startOffsetColumn: 3, startOffsetRow: 0)
    {
    }
}