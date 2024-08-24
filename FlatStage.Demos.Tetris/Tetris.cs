namespace FlatStage.Tetris;

public class Tetris : Game
{
    public Tetris()
    {
        Settings = new GameSettings()
        {
            AppTitle = "Vetris",
            CanvasWidth = 1024,
            CanvasHeight = 768
        };

        _tetrisController = new TetrisController();
        _tetrisView = new TetrisView(_tetrisController);
    }

    protected override void Load()
    {
        GameContent.Load();

        _tetrisView.UpdateLayout();

        _tetrisController.OnClearLines = OnClearLines;
        _tetrisController.OnPlaceBlock = OnPlaceBlock;
        _tetrisController.OnRotateBlock = OnRotateBlock;
        _tetrisController.OnGameStateChanged = OnGameStateChanged;

        _tetrisController.OnExitTriggered = () => Exit();
        _tetrisController.OnMenuHovered = () => GameContent.SfxMenuHover.Play();

        LoadSaveFile();

        GameContent.SngTitle.Play();

        Canvas.StretchMode = CanvasStretchMode.LetterBox;
    }

    protected override void Unload()
    {
        WriteSaveFile();
    }

    private void LoadSaveFile()
    {
        if (GameSaveIO.LoadSave(SaveGameId))
        {
            var maxScore = GameSaveIO.GetInteger("MaxScore");

            _tetrisController.MaxScore = maxScore;
        }
        else
        {
            _tetrisController.MaxScore = 0;
        }
    }

    private void WriteSaveFile()
    {
        GameSaveIO.BeginWrite(SaveGameId);

        GameSaveIO.Set(MaxScoreSaveId, _tetrisController.MaxScore);

        GameSaveIO.EndWrite();
    }

    private void OnGameStateChanged(GameStateId id)
    {
        switch (id)
        {
            case GameStateId.Game:
                {
                    if (GameContent.SngTitle.IsPlaying)
                    {
                        GameContent.SngTitle.Stop();
                    }

                    break;
                }
            case GameStateId.Menu:
                {
                    GameContent.SngTitle.Play();
                    break;
                }
            case GameStateId.GameOver:
                {
                    GameContent.SfxGameOver.Play();
                    break;
                }
        }
    }

    private void OnRotateBlock()
    {
        GameContent.SfxRotate.Play();
    }

    private void OnClearLines(int lineCount, bool doubleScore)
    {
        if (!doubleScore)
        {
            GameContent.SfxLineClear.Play();
        }
        else
        {
            GameContent.SfxFullLinesClear.Play();
        }
    }

    private void OnPlaceBlock(int blockId)
    {
        GameContent.SfxPlaceBlock.Play();
    }

    protected override void FixedUpdate(float dt)
    {
        _tetrisController.TickFixed();
        _tetrisView.Update();
    }

    protected override void Update(float dt)
    {
        _tetrisController.Tick();
    }

    protected override void Draw(Canvas canvas, float dt)
    {
        _tetrisView.Draw(canvas, dt);
    }

    private const string SaveGameId = "VetrisSave";
    private const string MaxScoreSaveId = "MaxScore";

    private readonly TetrisView _tetrisView;
    private readonly TetrisController _tetrisController;
}
