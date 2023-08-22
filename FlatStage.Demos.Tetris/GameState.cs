using System;

namespace FlatStage.Tetris;

public static class GameState
{
    public static Action<int> OnClearLines = null!;
    public static Action<int> OnPlaceBlock = null!;
    public static Action OnGameOver = null!;

    private static Block currentBlock = null!;

    public static Block CurrentBlock
    {
        get => currentBlock;
        private set
        {
            currentBlock = value;
            currentBlock.Reset();

            for (int i = 0; i < 2; i++)
            {
                currentBlock.Move(1, 0);

                if (!CurrentBlockFits())
                {
                    currentBlock.Move(-1, 0);
                }
            }
        }
    }

    public static GameGrid GameGrid { get; private set; } = null!;
    public static BlockQueue BlockQueue { get; private set; } = null!;
    public static bool GameOver { get; private set; }
    public static int Score { get; private set; }
    public static Block? HeldBlock { get; private set; }
    public static bool CanHold { get; private set; }

    public static void Init()
    {
        GameGrid = new GameGrid(22, 10);
        BlockQueue = new BlockQueue();
        CurrentBlock = BlockQueue.GetAndUpdate();
        CanHold = true;
    }

    public static void Restart()
    {
        GameGrid.ClearAll();
        CurrentBlock = BlockQueue.GetAndUpdate();
        GameOver = false;
        CanHold = true;
        Score = 0;
    }

    public static void HoldBlock()
    {
        if (!CanHold)
        {
            return;
        }

        if (HeldBlock == null)
        {
            HeldBlock = CurrentBlock;
            CurrentBlock = BlockQueue.GetAndUpdate();
        }
        else
        {
            (CurrentBlock, HeldBlock) = (HeldBlock, CurrentBlock);
        }

        CanHold = false;
    }

    public static void RotateBlockCW()
    {
        CurrentBlock.RotateCW();

        if (!CurrentBlockFits())
        {
            CurrentBlock.RotateCCW();
        }
    }

    public static void RotateBlockCCW()
    {
        CurrentBlock.RotateCCW();

        if (!CurrentBlockFits())
        {
            CurrentBlock.RotateCW();
        }
    }

    public static void MoveBlockLeft()
    {
        CurrentBlock.Move(0, -1);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(0, 1);
        }
    }

    public static void MoveBlockRight()
    {
        CurrentBlock.Move(0, 1);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(0, -1);
        }
    }

    private static bool IsGameOver()
    {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }

    private static void PlaceBlock()
    {
        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            GameGrid[p.Row + CurrentBlock.OffsetRow, p.Column + CurrentBlock.OffsetCol] = CurrentBlock.Id;
        }

        var cleared = GameGrid.ClearFullRows();

        if (cleared > 0)
        {
            Score += cleared;
            OnClearLines(cleared);
        }

        if (IsGameOver())
        {
            OnGameOver();
            GameOver = true;
        }
        else
        {
            OnPlaceBlock(CurrentBlock.Id);
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
    }

    public static void MoveBlockDown()
    {
        CurrentBlock.Move(1, 0);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }

    private static bool CurrentBlockFits()
    {
        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            if (!GameGrid.IsEmpty(p.Row + currentBlock.OffsetRow, p.Column + currentBlock.OffsetCol))
            {
                return false;
            }
        }

        return true;
    }

    private static int TileDropDistance(Block block, GridPos p)
    {
        int drop = 0;

        while (GameGrid.IsEmpty(p.Row + block.OffsetRow + drop + 1, p.Column + block.OffsetCol))
        {
            drop++;
        }

        return drop;
    }

    public static int BlockDropDistance()
    {
        int drop = GameGrid.Rows;

        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            drop = Calc.Min(drop, TileDropDistance(CurrentBlock, p));
        }

        return drop;
    }

    public static void DropBlock()
    {
        CurrentBlock.Move(BlockDropDistance(), 0);
        PlaceBlock();
    }
}