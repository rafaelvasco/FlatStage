namespace FlatStage.Tetris;

public class TetrisView
{
    public TetrisView(TetrisController controller)
    {
        _tetraminosImages[0] = new Rect(768, 768, 256, 256); // Empty
        _tetraminosImages[1] = new Rect(512, 768, 256, 256); // I
        _tetraminosImages[2] = new Rect(256, 768, 256, 256); // J
        _tetraminosImages[3] = new Rect(0, 768, 256, 256);   // L
        _tetraminosImages[4] = new Rect(768, 512, 256, 256); // O
        _tetraminosImages[5] = new Rect(512, 512, 256, 256); // S
        _tetraminosImages[6] = new Rect(768, 256, 256, 256); // T
        _tetraminosImages[7] = new Rect(768, 0, 256, 256);   // Z

        _objectsRegions[0] = new Rect(512, 256, 256, 256);   // Empty
        _objectsRegions[1] = new Rect(0, 512, 256, 256);     // Cyan
        _objectsRegions[2] = new Rect(256, 512, 256, 256);   // Blue
        _objectsRegions[3] = new Rect(0, 0, 256, 256);       // Orange
        _objectsRegions[4] = new Rect(256, 0, 256, 256);     // Yellow
        _objectsRegions[5] = new Rect(256, 256, 256, 256);   // Green
        _objectsRegions[6] = new Rect(512, 0, 256, 256);     // Purple
        _objectsRegions[7] = new Rect(0, 256, 256, 256);     // Red,
        _objectsRegions[8] = new Rect(0, 1038, 937, 550);    // GameOver

        _controller = controller;

    }

    /* ========================== INPUT AND PROCESS ================================== */

    public void UpdateLayout()
    {
        _gridWidth = _controller.GameGrid.Columns * CellSize;
        _gridHeight = _controller.GameGrid.Rows * CellSize;

        _gridOffsetX = (Canvas.Width / 2f) - (_gridWidth / 2f);
        _gridOffsetY = (Canvas.Height / 2f) - (_gridHeight / 2f);

        _heldIndicatorOffsetX = _gridOffsetX - TetraminoDisplaySize - BlockIndicatorsDistanceFromGrid;
        _heldIndicatorOffsetY = (Canvas.Height / 2f) - (TetraminoDisplaySize / 2f);

        _nextIndicatorOffsetX = _gridOffsetX + _gridWidth + BlockIndicatorsDistanceFromGrid;
        _nextIndicatorOffsetY = _heldIndicatorOffsetY;

        _heldIndicatorLabelOffsetDelta = (TetraminoDisplaySize / 2.0f) - (GameContent.FntDefault.MeasureString(HeldIndicatorLabel).X * TextScale / 2.0f);
        _nextIndicatorLabelOffsetDelta = (TetraminoDisplaySize / 2.0f) - (GameContent.FntDefault.MeasureString(NextIndicatorLabel).X * TextScale / 2.0f);

        _menuSpacing = 80;

        _menuYOffset = Canvas.Height - (_controller.MenuItems.Length * _menuSpacing) - 100;

        for (int i = 0; i < _controller.MenuItems.Length; ++i)
        {
            var label = _controller.MenuItems[i].Label;
            float textW = GameContent.FntMenu.MeasureString(label).X * TextScale;

            _controller.MenuItems[i].Rect = new Rect((int)((Canvas.Width / 2f) - (textW / 2f)), (int)(_menuYOffset + (i * _menuSpacing)), (int)textW, (int)_menuSpacing);
        }

        var gameTitleTextW = GameContent.FntGameTitle.MeasureString(GameTitle).X * GameTitleScale;

        _gameTitleXOffset = (Canvas.Width / 2f) - (gameTitleTextW / 2f);
        _gameTitleYOffset = 100.0f;

        _scoreTextsOffsetX = 20;
        _scoreTextsOffsetY = 20.0f;

    }

    public void Update()
    {
        _bgOffset.X += 1.0f;
        _bgOffset.Y += 1.0f;

        if (_bgOffset.X > GameContent.TexBackground.Width)
        {
            _bgOffset.X = 0;
        }

        if (_bgOffset.Y > GameContent.TexBackground.Height)
        {
            _bgOffset.Y = 0;
        }
    }

    public void Draw(Canvas canvas, float dt)
    {
        DrawBackground(canvas, dt);

        switch (_controller.GameStateId)
        {
            case GameStateId.Game:
                {

                    DrawGrid(canvas, _gridOffsetX, _gridOffsetY);
                    DrawGhostBlock(canvas, _controller.CurrentBlock, _gridOffsetX, _gridOffsetY);
                    DrawBlock(canvas, _controller.CurrentBlock, _gridOffsetX, _gridOffsetY);
                    DrawNextBlock(canvas, _controller.BlockQueue, _nextIndicatorOffsetX, _nextIndicatorOffsetY);
                    DrawHeldBlock(canvas, _controller.HeldBlock, _heldIndicatorOffsetX, _heldIndicatorOffsetY);
                    DrawTexts(canvas);

                    break;
                }
            case GameStateId.GameOver:
                {
                    DrawGameOver(canvas);

                    break;
                }

            case GameStateId.Menu:
                {
                    DrawGameTitle(canvas);
                    DrawMenu(canvas);
                    break;
                }
        }
    }

    private void DrawGameTitle(Canvas canvas)
    {
        canvas.DrawText(GameContent.FntGameTitle, GameTitle, new Vec2(_gameTitleXOffset, _gameTitleYOffset + 16), new Vec2(GameTitleScale, GameTitleScale), Color.Red);
        canvas.DrawText(GameContent.FntGameTitle, GameTitle, new Vec2(_gameTitleXOffset, _gameTitleYOffset + 8), new Vec2(GameTitleScale, GameTitleScale), Color.Cyan);
        canvas.DrawText(GameContent.FntGameTitle, GameTitle, new Vec2(_gameTitleXOffset, _gameTitleYOffset), new Vec2(GameTitleScale, GameTitleScale), Color.White);
    }

    private void DrawMenu(Canvas canvas)
    {
        var menuItems = _controller.MenuItems;

        for (int i = 0; i < menuItems.Length; i++)
        {
            string label = menuItems[i].Label;

            var rect = menuItems[i].Rect;

            var color = Color.White;
            var shadowColor = Color.Black;

            var offsetX = rect.X;
            var offsetY = rect.Y;

            if (_controller.CurrentHoveredMenuIndex == i)
            {
                color = Color.Cyan;
                offsetY = rect.Y - (MenuActiveYOffset / 2);
            }

            if (_controller.CurrentActiveMenuIndex == i)
            {
                offsetY = rect.Y + (MenuActiveYOffset / 2);
            }

            canvas.DrawText(GameContent.FntMenu, label, new Vec2(offsetX, rect.Y), new Vec2(TextScale, TextScale), shadowColor);
            canvas.DrawText(GameContent.FntMenu, label, new Vec2(offsetX, offsetY - MenuActiveYOffset), new Vec2(TextScale, TextScale), color);
        }
    }

    private void DrawBackground(Canvas canvas, float dt)
    {
        canvas.Draw(GameContent.TexBackground,
            new RectF(0, 0,
                Canvas.Width,
                Canvas.Height),
            new Rect((int)(dt * _bgOffset.X * 0.25f),
            (int)(dt * _bgOffset.Y * 0.25f), Canvas.Width, Canvas.Height),
            Color.White);
    }

    private void DrawGrid(Canvas canvas, float offsetX, float offsetY)
    {
        var grid = _controller.GameGrid;

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

                canvas.Draw(GameContent.TexObjects, destination, src, Color.White * alpha);
            }
        }
    }

    private void DrawBlock(Canvas canvas, Block block, float offsetX, float offsetY)
    {
        foreach (var position in block.TilePositions())
        {
            var src = _objectsRegions[block.Id];

            var x = ((position.Column + block.OffsetCol) * CellSize) + offsetX;
            var y = ((position.Row + block.OffsetRow) * CellSize) + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);
            canvas.Draw(GameContent.TexObjects, destination, src, Color.White);
        }
    }

    private void DrawGhostBlock(Canvas canvas, Block block, float offsetX, float offsetY)
    {
        var dropDistance = _controller.BlockDropDistance();

        foreach (var position in block.TilePositions())
        {
            var x = ((position.Column + block.OffsetCol) * CellSize) + offsetX;
            var y = (((position.Row + block.OffsetRow) + dropDistance) * CellSize) + offsetY;

            var destination = new RectF(x, y, CellSize, CellSize);

            var src = _objectsRegions[block.Id];

            canvas.Draw(GameContent.TexObjects, destination, src, Color.White * 0.45f);
        }
    }

    private void DrawGameOver(Canvas canvas)
    {
        canvas.Draw(GameContent.TexObjects, new Vec2(Canvas.Width / 2f, Canvas.Height / 2f), _objectsRegions[GameOverRegionIndex], Color.White, 0f, new Vec2(0.5f, 0.5f), new Vec2(0.5f, 0.5f), FlipMode.None, 0f);
    }

    private void DrawNextBlock(Canvas canvas, BlockQueue blockQueue, float offsetX, float offsetY)
    {
        var next = blockQueue.NextBlock;
        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);
        canvas.Draw(GameContent.TexObjects, destination, _tetraminosImages[next.Id], Color.Wheat);
    }

    private void DrawHeldBlock(Canvas canvas, Block? heldBlock, float offsetX, float offsetY)
    {
        Rect src;

        src = heldBlock == null ? _tetraminosImages[0] : _tetraminosImages[heldBlock.Id];

        var destination = new RectF(offsetX, offsetY, TetraminoDisplaySize, TetraminoDisplaySize);

        canvas.Draw(GameContent.TexObjects, destination, src, Color.Wheat);
    }

    private void DrawTexts(Canvas canvas)
    {
        _scoreString.Clear();
        _scoreString.Append("SCORE: ");
        _scoreString.Append(_controller.Score.ToString("D4"));

        _maxScoreString.Clear();
        _maxScoreString.Append("MAX: ");
        _maxScoreString.Append(_controller.MaxScore.ToString("D4"));

        canvas.DrawText(GameContent.FntDefault, _scoreString.ReadOnlySpan, new Vec2(_scoreTextsOffsetX, _scoreTextsOffsetY), new Vec2(TextScale, TextScale), Color.Cyan);
        canvas.DrawText(GameContent.FntDefault, _maxScoreString.ReadOnlySpan, new Vec2(_scoreTextsOffsetX, _scoreTextsOffsetY + 30), new Vec2(TextScale, TextScale), Color.Cyan);

        canvas.DrawText(GameContent.FntDefault, HeldIndicatorLabel, new Vec2(_heldIndicatorOffsetX + _heldIndicatorLabelOffsetDelta, _heldIndicatorOffsetY + TetraminoDisplaySize + 10f), new Vec2(TextScale, TextScale), Color.Cyan);
        canvas.DrawText(GameContent.FntDefault, NextIndicatorLabel, new Vec2(_nextIndicatorOffsetX + _nextIndicatorLabelOffsetDelta, _nextIndicatorOffsetY + TetraminoDisplaySize + 10f), new Vec2(TextScale, TextScale), Color.Cyan);
    }

    private const string GameTitle = "VETRIS";
    private const string HeldIndicatorLabel = "HOLD";
    private const string NextIndicatorLabel = "NEXT";

    private const float TextScale = 2.0f;
    private const float GameTitleScale = 10f;
    private const float BlockIndicatorsDistanceFromGrid = 50;

    private const int TetraminoDisplaySize = 128;
    private const int GameOverRegionIndex = 8;
    private const int MenuActiveYOffset = 4;
    private const int CellSize = 32;

    private readonly StringBuffer _scoreString = new();
    private readonly StringBuffer _maxScoreString = new();
    private readonly Rect[] _objectsRegions = new Rect[9];
    private readonly Rect[] _tetraminosImages = new Rect[8];
    private readonly TetrisController _controller;

    private float _heldIndicatorOffsetX;
    private float _heldIndicatorOffsetY;
    private float _nextIndicatorOffsetX;
    private float _nextIndicatorOffsetY;
    private float _gridOffsetX;
    private float _gridOffsetY;
    private float _heldIndicatorLabelOffsetDelta;
    private float _nextIndicatorLabelOffsetDelta;
    private float _menuYOffset;
    private float _menuSpacing;
    private float _gameTitleXOffset;
    private float _gameTitleYOffset;
    private float _scoreTextsOffsetX;
    private float _scoreTextsOffsetY;

    private int _gridWidth;
    private int _gridHeight;

    private Vec2 _bgOffset;

}
