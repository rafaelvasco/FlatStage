namespace FlatStage.Tetris;

public sealed class JBlock : Block
{
    private readonly GridPos[][] _tiles =
    {
        new GridPos[] { new(0, 0), new(1, 0), new(1, 1), new(1, 2) },
        new GridPos[] { new(0, 1), new(0, 2), new(1, 1), new(2, 1) },
        new GridPos[] { new(1, 0), new(1, 1), new(1, 2), new(2, 2) },
        new GridPos[] { new(0, 1), new(1, 1), new(2, 1), new(2, 0) }
    };

    protected override GridPos[][] Tiles => _tiles;

    public JBlock() : base(id: 2, startOffsetColumn: 3, startOffsetRow: 0)
    {
    }
}