using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;

namespace FlatStage.Tetris;

public class Tetris : Game
{
    private Texture _sheet = null!;

    private const int MaxDelay = 200;
    private const int MinDelay = 25;
    private const int DelayDecrease = 5;
    private const int CellSize = 32;
    private const int TetraminoDisplaySize = 128;

    private const float BlockIndicatorsDistanceFromGrid = 50;

    private const int SheetCellSize = 256;

    private int _ticks;
    private readonly GameState _gameState = new();

    private int GridPxWidth;
    private int GridPxHeight;

    private float _gridOffsetX;
    private float _gridOffsetY;

    private float _currentIndicatorOffsetX;
    private float _currentIndicatorOffsetY;

    private float _nextIndicatorOffsetX;
    private float _nextIndicatorOffsetY;

    public Tetris()
    {
        UpdateLayout();
    }

    protected override void Preload()
    {
        _sheet = Content.Get<Texture>("tetris_sheet");

        GraphicsContext.SetViewClear(0, Color.Black);
    }

    private void UpdateLayout()
    {
        GridPxWidth = _gameState.GameGrid.Columns * CellSize;
        GridPxHeight = _gameState.GameGrid.Rows * CellSize;

        _gridOffsetX = Stage.WindowSize.Width / 2f - GridPxWidth / 2f;
        _gridOffsetY = Stage.WindowSize.Height / 2f - GridPxHeight / 2f;

        _currentIndicatorOffsetX = _gridOffsetX - TetraminoDisplaySize - BlockIndicatorsDistanceFromGrid;
        _currentIndicatorOffsetY = Stage.WindowSize.Height / 2f - TetraminoDisplaySize / 2f;

        _nextIndicatorOffsetX = _gridOffsetX + GridPxWidth + BlockIndicatorsDistanceFromGrid;
        _nextIndicatorOffsetY = _currentIndicatorOffsetY;
    }

    private readonly Rect[] _tiles =
    {
        new(SheetCellSize * 2, SheetCellSize, SheetCellSize, SheetCellSize), // Empty
        new(0, SheetCellSize * 2, SheetCellSize, SheetCellSize), // Cyan
        new(SheetCellSize, SheetCellSize * 2, SheetCellSize, SheetCellSize), // Blue
        new(0, 0, SheetCellSize, SheetCellSize), // Orange
        new(SheetCellSize, 0, SheetCellSize, SheetCellSize), // Pink
        new(SheetCellSize, SheetCellSize, SheetCellSize, SheetCellSize), // Green
        new(SheetCellSize * 2, 0, SheetCellSize, SheetCellSize), // Purple
        new(0, SheetCellSize, SheetCellSize, SheetCellSize) // Red
    };

    private readonly Rect[] _tetraminosImages =
    {
        new(SheetCellSize * 3, SheetCellSize * 3, SheetCellSize, SheetCellSize), // Empty
        new(SheetCellSize * 2, SheetCellSize * 3, SheetCellSize, SheetCellSize), // I
        new(SheetCellSize, SheetCellSize * 3, SheetCellSize, SheetCellSize), // J
        new(0, SheetCellSize * 3, SheetCellSize, SheetCellSize), // L
        new(SheetCellSize * 3, SheetCellSize * 2, SheetCellSize, SheetCellSize), // O
        new(SheetCellSize * 2, SheetCellSize * 2, SheetCellSize, SheetCellSize), // S
        new(SheetCellSize * 3, SheetCellSize, SheetCellSize, SheetCellSize), // T
        new(SheetCellSize * 3, 0, SheetCellSize, SheetCellSize) // Z
    };

    private void DrawGrid(Canvas2D canvas, GameGrid grid, float offsetX, float offsetY)
    {
        for (var r = 0; r < grid.Rows; ++r)
        {
            for (var c = 0; c < grid.Columns; ++c)
            {
                var id = grid[r, c];
                var x = c * CellSize + offsetX;
                var y = r * CellSize + offsetY;
                var src = _tiles[id];

                var destination = new RectF(x, y, CellSize, CellSize);
                canvas.Draw(_sheet, destination, src, Color.White);
            }
        }
    }

    private void DrawBlock(Canvas2D canvas, Block block, float offsetX, float offsetY)
    {
        foreach (var position in block.TilePositions())
        {
            var src = _tiles[block.Id];

            var x = (position.Column + block.OffsetCol) * CellSize + offsetX;
            var y = (position.Row + block.OffsetRow) * CellSize + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);
            canvas.Draw(_sheet, destination, src, Color.White);
        }
    }

    private void DrawNextBlock(Canvas2D canvas, BlockQueue blockQueue, float offsetX, float offsetY)
    {
        var next = blockQueue.NextBlock;
        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);
        canvas.Draw(_sheet, destination, _tetraminosImages[next.Id], Color.Wheat);
    }

    private void DrawHeldBlock(Canvas2D canvas, Block? heldBlock, float offsetX, float offsetY)
    {
        Rect src;

        src = heldBlock == null ? _tetraminosImages[0] : _tetraminosImages[heldBlock.Id];

        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);

        canvas.Draw(_sheet, destination, src, Color.Wheat);
    }

    private void DrawGhostBlock(Canvas2D canvas, Block block, float offsetX, float offsetY)
    {
        var dropDistance = _gameState.BlockDropDistance();

        foreach (var position in block.TilePositions())
        {
            var x = (position.Column + block.OffsetCol) * CellSize + offsetX;
            var y = ((position.Row + block.OffsetRow) + dropDistance) * CellSize + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);

            var src = _tiles[block.Id];

            canvas.Draw(_sheet, destination, src, Color.White.WithAlpha(0.25f));
        }
    }

    private void DrawGame(Canvas2D canvas, GameState gameState)
    {
        DrawGrid(canvas, gameState.GameGrid, _gridOffsetX, _gridOffsetY);
        DrawGhostBlock(canvas, gameState.CurrentBlock, _gridOffsetX, _gridOffsetY);
        DrawBlock(canvas, gameState.CurrentBlock, _gridOffsetX, _gridOffsetY);
        DrawNextBlock(canvas, gameState.BlockQueue, _nextIndicatorOffsetX, _nextIndicatorOffsetY);
        DrawHeldBlock(canvas, gameState.HeldBlock, _currentIndicatorOffsetX, _currentIndicatorOffsetY);

        if (gameState.GameOver)
        {

        }
    }

    protected override void FixedUpdate(float dt)
    {
        if (_gameState.GameOver) return;

        _ticks += 1;

        float delay = Calc.Max(MinDelay, MaxDelay - _gameState.Score * DelayDecrease);

        if (!(_ticks > delay)) return;

        _ticks = 0;
        _gameState.MoveBlockDown();
    }

    protected override void Update(float dt)
    {
        if (!_gameState.GameOver)
        {
            if (Control.Keyboard.KeyPressed(Key.A))
            {
                _gameState.MoveBlockLeft();
            }
            else if (Control.Keyboard.KeyPressed(Key.D))
            {
                _gameState.MoveBlockRight();
            }
            else if (Control.Keyboard.KeyPressed(Key.J))
            {
                _gameState.RotateBlockCCW();
            }
            else if (Control.Keyboard.KeyPressed(Key.K))
            {
                _gameState.RotateBlockCW();
            }
            else if (Control.Keyboard.KeyPressed(Key.S))
            {
                _gameState.MoveBlockDown();
            }

            if (Control.Keyboard.KeyPressed(Key.Space))
            {
                _gameState.DropBlock();
            }

            if (Control.Keyboard.KeyPressed(Key.Enter))
            {
                _gameState.HoldBlock();
            }
        }
        else
        {

        }

    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        DrawGame(canvas, _gameState);

        canvas.End();
    }
}