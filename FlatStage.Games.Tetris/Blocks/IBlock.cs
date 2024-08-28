namespace FlatStage.Tetris;

public sealed class IBlock : Block
{
    private readonly GridPos[][] _tiles =
    {
        new GridPos[] { new(1, 0), new(1, 1), new(1, 2), new(1, 3) },
        new GridPos[] { new(0, 2), new(1, 2), new(2, 2), new(3, 2) },
        new GridPos[] { new(2, 0), new(2, 1), new(2, 2), new(2, 3) },
        new GridPos[] { new(0, 1), new(1, 1), new(2, 1), new(3, 1) }
    };

    protected override GridPos[][] Tiles => _tiles;

    public IBlock() : base(id: 1, startOffsetRow: -1, startOffsetColumn: 3)
    {
    }
}