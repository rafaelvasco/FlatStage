using FlatStage.ContentPipeline;
using FlatStage.Graphics;
using System.Text;

namespace FlatStage.Tetris;
public class TetrisView
{
    private const string HeldIndicatorLabel = "HOLD";
    private const string NextIndicatorLabel = "NEXT";
    private const float BlockIndicatorsDistanceFromGrid = 50;
    private const int TetraminoDisplaySize = 128;
    private const int GameOverRegionIndex = 8;
    private const int CellSize = 32;
    private float FontScale = 2.0f;

    private Texture _bg = null!;
    private Texture _objects = null!;
    private TextureFont _font = null!;

    private StringBuilder _scoreString = new("SCORE: ");

    private float _heldIndicatorOffsetX;
    private float _heldIndicatorOffsetY;
    private float _nextIndicatorOffsetX;
    private float _nextIndicatorOffsetY;
    private float _gridOffsetX;
    private float _gridOffsetY;
    private float _heldIndicatorLabelOffsetDelta;
    private float _nextIndicatorLabelOffsetDelta;

    private int _gridWidth;
    private int _gridHeight;

    private Vec2 _bgOffset;

    private readonly Rect[] _objectsRegions = new Rect[9];
    private readonly Rect[] _tetraminosImages = new Rect[8];

    public TetrisView()
    {
        _tetraminosImages[0] = new(768, 768, 256, 256); // Empty
        _tetraminosImages[1] = new(512, 768, 256, 256); // I
        _tetraminosImages[2] = new(256, 768, 256, 256); // J
        _tetraminosImages[3] = new(0, 768, 256, 256);   // L
        _tetraminosImages[4] = new(768, 512, 256, 256); // O
        _tetraminosImages[5] = new(512, 512, 256, 256); // S
        _tetraminosImages[6] = new(768, 256, 256, 256); // T
        _tetraminosImages[7] = new(768, 0, 256, 256);   // Z

        _objectsRegions[0] = new(512, 256, 256, 256);   // Empty
        _objectsRegions[1] = new(0, 512, 256, 256);     // Cyan
        _objectsRegions[2] = new(256, 512, 256, 256);   // Blue
        _objectsRegions[3] = new(0, 0, 256, 256);       // Orange
        _objectsRegions[4] = new(256, 0, 256, 256);     // Yellow
        _objectsRegions[5] = new(256, 256, 256, 256);   // Green
        _objectsRegions[6] = new(512, 0, 256, 256);     // Purple
        _objectsRegions[7] = new(0, 256, 256, 256);     // Red,
        _objectsRegions[8] = new(0, 1038, 937, 550);    // GameOver
    }

    public void Load()
    {
        _bg = Content.Get<Texture>("bg");
        _objects = Content.Get<Texture>("tetris_sheet");
        _font = Content.Get<TextureFont>("monogram");
    }

    public void UpdateLayout()
    {
        _gridWidth = GameState.GameGrid.Columns * CellSize;
        _gridHeight = GameState.GameGrid.Rows * CellSize;

        _gridOffsetX = (Stage.WindowSize.Width / 2f) - (_gridWidth / 2f);
        _gridOffsetY = (Stage.WindowSize.Height / 2f) - (_gridHeight / 2f);

        _heldIndicatorOffsetX = _gridOffsetX - TetraminoDisplaySize - BlockIndicatorsDistanceFromGrid;
        _heldIndicatorOffsetY = (Stage.WindowSize.Height / 2f) - (TetraminoDisplaySize / 2f);

        _nextIndicatorOffsetX = _gridOffsetX + _gridWidth + BlockIndicatorsDistanceFromGrid;
        _nextIndicatorOffsetY = _heldIndicatorOffsetY;

        _heldIndicatorLabelOffsetDelta = (TetraminoDisplaySize / 2.0f) - (_font.MeasureString(HeldIndicatorLabel, FontScale, FontScale).X / 2.0f);
        _nextIndicatorLabelOffsetDelta = (TetraminoDisplaySize / 2.0f) - (_font.MeasureString(NextIndicatorLabel, FontScale, FontScale).X / 2.0f);
    }

    public void Process()
    {
        _bgOffset.X += 1.0f;
        _bgOffset.Y += 1.0f;

        if (_bgOffset.X > _bg.Width)
        {
            _bgOffset.X = 0;
        }

        if (_bgOffset.Y > _bg.Height)
        {
            _bgOffset.Y = 0;
        }
    }

    public void Draw(Canvas2D canvas, float dt)
    {
        DrawBackground(canvas, dt);
        DrawGrid(canvas, GameState.GameGrid, _gridOffsetX, _gridOffsetY);
        DrawGhostBlock(canvas, GameState.CurrentBlock, _gridOffsetX, _gridOffsetY);
        DrawBlock(canvas, GameState.CurrentBlock, _gridOffsetX, _gridOffsetY);
        DrawNextBlock(canvas, GameState.BlockQueue, _nextIndicatorOffsetX, _nextIndicatorOffsetY);
        DrawHeldBlock(canvas, GameState.HeldBlock, _heldIndicatorOffsetX, _heldIndicatorOffsetY);
        DrawTexts(canvas);

        if (GameState.GameOver)
        {
            DrawGameOver(canvas);
        }
    }

    private void DrawBackground(Canvas2D canvas, float dt)
    {
        canvas.Draw(_bg,
            new RectF(0, 0,
                Stage.WindowSize.Width,
                Stage.WindowSize.Height),
            new
            ((int)(dt * _bgOffset.X * 0.25f),
            (int)(dt * _bgOffset.Y * 0.25f), Stage.WindowSize.Width, Stage.WindowSize.Height),
            Color.White);
    }

    private void DrawGrid(Canvas2D canvas, GameGrid grid, float offsetX, float offsetY)
    {
        for (var r = 0; r < grid.Rows; ++r)
        {
            for (var c = 0; c < grid.Columns; ++c)
            {
                var id = grid[r, c];
                var x = (c * CellSize) + offsetX;
                var y = (r * CellSize) + offsetY;
                var src = _objectsRegions[id];

                var destination = new RectF(x, y, CellSize, CellSize);

                var alpha = id > 0 ? 1 : 0.85f;

                canvas.Draw(_objects, destination, src, Color.White * alpha);
            }
        }
    }

    private void DrawBlock(Canvas2D canvas, Block block, float offsetX, float offsetY)
    {
        foreach (var position in block.TilePositions())
        {
            var src = _objectsRegions[block.Id];

            var x = ((position.Column + block.OffsetCol) * CellSize) + offsetX;
            var y = ((position.Row + block.OffsetRow) * CellSize) + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);
            canvas.Draw(_objects, destination, src, Color.White);
        }
    }

    private void DrawGhostBlock(Canvas2D canvas, Block block, float offsetX, float offsetY)
    {
        var dropDistance = GameState.BlockDropDistance();

        foreach (var position in block.TilePositions())
        {
            var x = ((position.Column + block.OffsetCol) * CellSize) + offsetX;
            var y = (((position.Row + block.OffsetRow) + dropDistance) * CellSize) + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);

            var src = _objectsRegions[block.Id];

            canvas.Draw(_objects, destination, src, Color.White * 0.45f);
        }
    }

    private void DrawGameOver(Canvas2D canvas)
    {
        canvas.Draw(_objects, new Vec2(Stage.WindowSize.Width / 2f, Stage.WindowSize.Height / 2f), _objectsRegions[GameOverRegionIndex], Color.White, 0f, new Vec2(0.5f, 0.5f), new Vec2(0.5f, 0.5f), FlipMode.None, 0f);
    }

    private void DrawNextBlock(Canvas2D canvas, BlockQueue blockQueue, float offsetX, float offsetY)
    {
        var next = blockQueue.NextBlock;
        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);
        canvas.Draw(_objects, destination, _tetraminosImages[next.Id], Color.Wheat);
    }

    private void DrawHeldBlock(Canvas2D canvas, Block? heldBlock, float offsetX, float offsetY)
    {
        Rect src;

        src = heldBlock == null ? _tetraminosImages[0] : _tetraminosImages[heldBlock.Id];

        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);

        canvas.Draw(_objects, destination, src, Color.Wheat);
    }

    private void DrawTexts(Canvas2D canvas)
    {
        _scoreString.Clear();
        _scoreString.Append("SCORE: ");
        _scoreString.Append(GameState.Score);

        var textSize = _font.MeasureString(_scoreString);

        canvas.DrawText(_font, _scoreString, new Vec2((Stage.WindowSize.Width / 8f) - (textSize.X / 2f), 50f), new Vec2(2.0f, 2.0f), Color.White);

        canvas.DrawText(_font, HeldIndicatorLabel, new Vec2(_heldIndicatorOffsetX + _heldIndicatorLabelOffsetDelta, _heldIndicatorOffsetY + TetraminoDisplaySize + 10f), new Vec2(FontScale, FontScale), Color.White);
        canvas.DrawText(_font, NextIndicatorLabel, new Vec2(_nextIndicatorOffsetX + _nextIndicatorLabelOffsetDelta, _nextIndicatorOffsetY + TetraminoDisplaySize + 10f), new Vec2(FontScale, FontScale), Color.White);
    }
}
