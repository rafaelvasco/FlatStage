using System;

namespace FlatStage.Tetris;

public enum GameStateId
{
    Menu,
    Game,
    GameOver
}

public class TetrisController
{
    private const int MaxDelay = 200;
    private const int MinDelay = 25;
    private const int DelayDecrease = 5;
    private const int InputHeldDownMoveDelay = 10;
    private const int StartHeldDownInputDelay = 20;
    private const int FullClearLineCount = 4;
    private const int DefaultScoreGain = 1;
    private const int FullClearScoreGain = 2;
    private const int TickIncrease = 1;

    public Action<int, bool> OnClearLines = null!;
    public Action<int> OnPlaceBlock = null!;
    public Action OnRotateBlock = null!;
    public Action<GameStateId> OnGameStateChanged = null!;
    public Action OnExitTriggered = null!;
    public Action OnMenuHovered = null!;

    public Block CurrentBlock
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

    public MenuAction[] MenuItems => _mainMenuItems;

    public int CurrentHoveredMenuIndex => _currentHoveredMenuIndex;

    public int CurrentActiveMenuIndex => _currentActiveMenuIndex;

    public GameGrid GameGrid { get; }
    public BlockQueue BlockQueue { get; }

    public GameStateId GameStateId { get; private set; } = GameStateId.Menu;

    public int Score { get; internal set; }

    public int MaxScore { get; internal set; }

    public Block? HeldBlock { get; private set; }
    public bool CanHold { get; private set; }

    public TetrisController()
    {
        GameGrid = new GameGrid(22, 10);
        BlockQueue = new BlockQueue();
        CurrentBlock = BlockQueue.GetAndUpdate();

        Keyboard.OnKeyDown += Keyboard_OnKeyDown;
        Keyboard.OnKeyUp += Keyboard_OnKeyUp;
    }

    public void SetState(GameStateId state)
    {
        if (GameStateId != state)
        {
            GameStateId = state;

            switch (state)
            {
                case GameStateId.Game: Restart(); break;
            }

            OnGameStateChanged(state);
        }
    }

    public void TickFixed()
    {
        if (GameStateId == GameStateId.Game)
        {
            _ticks += TickIncrease;

            float delay = MathUtils.Max(MinDelay, MaxDelay - (Score * DelayDecrease));

            if (!(_ticks > delay)) return;

            _ticks = 0;

            if (!_pressingMoveDown)
            {
                MoveBlockDown();
            }
        }
    }

    public void Tick()
    {
        if (_pressingMoveHorizontal)
        {
            if (_currentHorizontalDirection < 0)
            {
                HandleMoveInput(MoveBlockLeft);
            }
            else
            {
                HandleMoveInput(MoveBlockRight);
            }
        }

        if (_pressingMoveDown)
        {
            HandleMoveInput(MoveBlockDown);
        }

        if ((!Keyboard.KeyDown(Key.A) && !Keyboard.KeyDown(Key.D)))
        {
            _pressingMoveHorizontal = false;
            _currentHorizontalDirection = 0;
        }
    }

    private void ProcessInputMenu(Key key, bool down)
    {
        var index = _currentHoveredMenuIndex;

        switch (key)
        {
            case Key.W when down:
                {
                    index -= 1;
                    break;
                }
            case Key.S when down:
                {
                    index += 1;
                    break;
                }
            case Key.Space or Key.Enter or Key.J or Key.K when down:
                {
                    _currentActiveMenuIndex = _currentHoveredMenuIndex;
                    break;
                }
            case Key.Space or Key.Enter or Key.J or Key.K when !down:
                {
                    _currentActiveMenuIndex = -1;
                    TriggerMenuItem(_currentHoveredMenuIndex);
                    break;
                }
        }

        index = MathUtils.Clamp(index, 0, _mainMenuItems.Length - 1);

        _lastHoveredMenuIndex = _currentHoveredMenuIndex;

        _currentHoveredMenuIndex = index;

        if (_currentHoveredMenuIndex != _lastHoveredMenuIndex)
        {
            OnMenuHovered();
        }
    }

    private void ProcessInputGame(Key key, bool down)
    {
        switch (key)
        {
            case Key.A when down:
                {
                    ClearMoveInputTicks();
                    _currentHorizontalDirection = -1;
                    _pressingMoveHorizontal = true;
                    break;
                }

            case Key.D when down:
                {
                    ClearMoveInputTicks();
                    _currentHorizontalDirection = 1;
                    _pressingMoveHorizontal = true;
                    break;
                }
            case Key.S when down:
                {
                    ClearMoveInputTicks();
                    _pressingMoveDown = true;
                    break;
                }
            case Key.S when !down:
                {
                    _pressingMoveDown = false;
                    break;
                }
            case Key.J when down:
                {
                    RotateBlockCCW();
                    OnRotateBlock();
                    break;
                }
            case Key.K when down:
                {
                    RotateBlockCW();
                    OnRotateBlock();
                    break;
                }
            case Key.Space when down:
                {
                    DropBlock();
                    break;
                }
            case Key.Enter when down:
                {
                    HoldBlock();
                    break;
                }
            case Key.Escape when down:
                {
                    SetState(GameStateId.Menu);
                    break;
                }
        }
    }

    private void ProcessInputGameOver(Key key, bool down)
    {
        switch (key)
        {
            case Key.Space or Key.J or Key.K or Key.Enter when down:
                {
                    SetState(GameStateId.Game);
                    break;
                }
            case Key.Escape when down:
                {
                    SetState(GameStateId.Menu);
                    break;
                }
        }
    }

    private void TriggerMenuItem(int index)
    {
        switch (_mainMenuItems[index].ActionId)
        {
            case MenuActionId.Start: SetState(GameStateId.Game); break;
            case MenuActionId.Options: break;
            case MenuActionId.Exit: OnExitTriggered(); break;
        }
    }

    /* =============================== MOVEMENT ================================= */
    public void MoveBlockLeft()
    {
        CurrentBlock.Move(0, -1);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(0, 1);
        }
    }

    public void MoveBlockRight()
    {
        CurrentBlock.Move(0, 1);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(0, -1);
        }
    }

    public void MoveBlockDown()
    {
        CurrentBlock.Move(1, 0);

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }

    public int BlockDropDistance()
    {
        int drop = GameGrid.Rows;

        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            drop = MathUtils.Min(drop, TileDropDistance(CurrentBlock, p));
        }

        return drop;
    }

    public void DropBlock()
    {
        CurrentBlock.Move(BlockDropDistance(), 0);
        PlaceBlock();
    }

    public void RotateBlockCW()
    {
        CurrentBlock.RotateCW();
        HandleRotateCollision();
    }

    public void RotateBlockCCW()
    {
        CurrentBlock.RotateCCW();
        HandleRotateCollision();
    }

    private void ClearMoveInputTicks()
    {
        _startInputTicks = 0;
        _inputTicks = 0;
    }

    private void HandleMoveInput(Action action, bool delayStartHold = true)
    {
        if (_startInputTicks == 0 && _inputTicks == 0)
        {
            action();
        }

        _startInputTicks += 1;

        if (delayStartHold)
        {
            if (_startInputTicks > StartHeldDownInputDelay)
            {
                DelayedMove(action, InputHeldDownMoveDelay);
            }
        }
        else
        {
            DelayedMove(action, InputHeldDownMoveDelay);
        }

    }

    private void DelayedMove(Action action, int delay)
    {

        _inputTicks += 1;

        if (_inputTicks > delay)
        {
            _inputTicks = 0;
            action();
        }
    }

    private int TileDropDistance(Block block, GridPos p)
    {
        int drop = 0;

        while (GameGrid.IsEmpty(p.Row + block.OffsetRow + drop + 1, p.Column + block.OffsetCol))
        {
            drop++;
        }

        return drop;
    }

    private void Keyboard_OnKeyUp(Key key)
    {
        switch (GameStateId)
        {
            case GameStateId.Game:
                ProcessInputGame(key, false);
                break;
            case GameStateId.Menu:
                ProcessInputMenu(key, false);
                break;
            case GameStateId.GameOver:
                ProcessInputGameOver(key, false);
                break;
        }
    }

    private void Keyboard_OnKeyDown(Key key)
    {
        switch (GameStateId)
        {
            case GameStateId.Game:
                ProcessInputGame(key, true);
                break;
            case GameStateId.Menu:
                ProcessInputMenu(key, true);
                break;
            case GameStateId.GameOver:
                ProcessInputGameOver(key, true);
                break;
        }
    }

    /* ========================= BLOCK CONTACT ================================= */
    private void PlaceBlock()
    {
        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            GameGrid[p.Row + CurrentBlock.OffsetRow, p.Column + CurrentBlock.OffsetCol] = CurrentBlock.Id;
        }

        PostPlaceBlock();
    }

    private void HandleRotateCollision()
    {
        _tryFitCount = 0;

        while (!CurrentBlockFitsInside())
        {
            _tryFitCount += 1;

            if (_tryFitCount > 3)
            {
                break;
            }

            if (CurrentBlock.OffsetCol < GameGrid.Columns / 2)
            {
                CurrentBlock.Move(0, 1);
            }
            else
            {
                CurrentBlock.Move(0, -1);
            }
        }

        if (!CurrentBlockFits())
        {
            CurrentBlock.Move(-1, 0);
            PlaceBlock();
        }
    }

    private void PostPlaceBlock()
    {
        var cleared = GameGrid.ClearFullRows();

        if (CheckGameOver())
        {
            SetScoringState();
            SetState(GameStateId.GameOver);
        }
        else
        {
            if (cleared > 0)
            {
                HandleClearedLines(cleared);
            }
            else
            {
                OnPlaceBlock(CurrentBlock.Id);
            }

            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }
    }

    private void SetScoringState()
    {
        if (Score > MaxScore)
        {
            MaxScore = Score;
        }
    }

    private void HandleClearedLines(int lineCount)
    {
        var doubleScore = lineCount == FullClearLineCount;

        if (!doubleScore)
        {
            Score += DefaultScoreGain;
        }
        else
        {
            Score += FullClearScoreGain;
        }

        OnClearLines(lineCount, doubleScore);
    }

    private bool CurrentBlockFitsInside()
    {
        foreach (GridPos p in CurrentBlock.TilePositions())
        {
            if (!GameGrid.IsInside(p.Row + currentBlock.OffsetRow, p.Column + currentBlock.OffsetCol))
            {
                return false;
            }
        }

        return true;
    }

    private bool CurrentBlockFits()
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

    /* =============== BLOCK HOLDING ============================================ */
    public void HoldBlock()
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

    /* =========================================================================== */

    private void Restart()
    {
        GameGrid.ClearAll();
        CurrentBlock = BlockQueue.GetAndUpdate();
        CanHold = true;
        HeldBlock = null;
        Score = 0;

        ClearMoveInputTicks();
        _pressingMoveDown = false;
        _pressingMoveHorizontal = false;
        _currentHorizontalDirection = 0;

    }

    private bool CheckGameOver()
    {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }

    private Block currentBlock = null!;
    private int _ticks;
    private int _startInputTicks;
    private int _inputTicks;
    private bool _pressingMoveDown;
    private bool _pressingMoveHorizontal;
    private int _currentHorizontalDirection;

    private int _tryFitCount;

    private int _lastHoveredMenuIndex;
    private int _currentHoveredMenuIndex;
    private int _currentActiveMenuIndex = -1;

    private readonly MenuAction[] _mainMenuItems =
    [
        new MenuAction { ActionId = MenuActionId.Start, Label = "Start", Rect = new Rect() },
        new MenuAction { ActionId = MenuActionId.Options, Label = "Options", Rect = new Rect() },
        new MenuAction { ActionId = MenuActionId.Exit, Label = "Exit", Rect = new Rect() }
    ];

    // private readonly MenuAction[] _optionsMenuItems =
    // [
    //     new MenuAction { ActionId = MenuActionId.Start, Label = "Start", Rect = new Rect() },
    //     new MenuAction { ActionId = MenuActionId.Options, Label = "Options", Rect = new Rect() },
    //     new MenuAction { ActionId = MenuActionId.Exit, Label = "Exit", Rect = new Rect() }
    // ];
}
