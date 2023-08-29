using FlatStage.Graphics;

namespace FlatStage.Tetris;

public class Tetris : Game
{

    private TetrisView _tetrisView = null!;
    private TetrisController _tetrisController = null!;

    public Tetris()
    {
        _tetrisController = new TetrisController();
        _tetrisView = new TetrisView(_tetrisController);
    }

    protected override void Preload()
    {
        GameContent.Load();

        _tetrisView.UpdateLayout();

        GraphicsContext.SetViewClear(0, Color.Black);

        _tetrisController.OnClearLines = OnClearLines;
        _tetrisController.OnPlaceBlock = OnPlaceBlock;
        _tetrisController.OnRotateBlock = OnRotateBlock;
        _tetrisController.OnGameStateChanged = OnGameStateChanged;

        _tetrisController.OnExitTriggered = () => Stage.Exit();
        _tetrisController.OnMenuHovered = () => GameContent.SfxMenuHover.Play();

        GameContent.SngTitle.Play();
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

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        _tetrisView.Draw(canvas, dt);

        canvas.End();
    }
}