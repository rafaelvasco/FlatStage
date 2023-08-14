namespace FlatStage.Tetris;

public sealed class OBlock : Block
{
    private readonly GridPos[][] tiles =
    {
        new GridPos[] { new(0, 0), new(0, 1), new(1, 0), new(1, 1) }
    };

    protected override GridPos[][] Tiles => tiles;

    public OBlock() : base(id: 4, startOffsetColumn: 4, startOffsetRow: 0)
    {
    }
}