namespace FlatStage.Tetris;

public class BlockQueue
{
    private readonly Block[] blocks =
    {
        new IBlock(),
        new JBlock(),
        new LBlock(),
        new OBlock(),
        new SBlock(),
        new TBlock(),
        new ZBlock()
    };

    public Block NextBlock { get; private set; }

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    private Block RandomBlock()
    {
        var index = PRNG.Next(0, blocks.Length);

        return blocks[index];
    }

    public Block GetAndUpdate()
    {
        Block block = NextBlock;

        do
        {
            NextBlock = RandomBlock();
        } while (block.Id == NextBlock.Id);

        return block;
    }
}
