namespace FlatStage.Tetris;

public sealed class TBlock : Block
{
    private readonly GridPos[][] _tiles =
    {
        new GridPos[] { new(0, 1), new(1, 0), new(1, 1), new(1, 2) },
        new GridPos[] { new(0, 1), new(1, 1), new(1, 2), new(2, 1) },
        new GridPos[] { new(1, 0), new(1, 1), new(1, 2), new(2, 1) },
        new GridPos[] { new(0, 1), new(1, 0), new(1, 1), new(2, 1) }
    };

    protected override GridPos[][] Tiles => _tiles;

    public TBlock() : base(id: 6, startOffsetColumn: 3, startOffsetRow: 0)
    {
    }
}