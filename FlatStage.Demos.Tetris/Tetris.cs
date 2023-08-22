using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Sound;

namespace FlatStage.Tetris;

public class Tetris : Game
{
    private Audio _sfxRotate = null!;
    private Audio _sfxGameOver = null!;
    private Audio _sfxLineClear = null!;
    private Audio _sfxPlaceBlock = null!;

    private TetrisView _view = null!;

    private const int MaxDelay = 200;
    private const int MinDelay = 25;
    private const int DelayDecrease = 5;
    private const int MoveDownDelay = 5;

    private int _ticks;
    private int _moveDownTicks;

    public Tetris()
    {
        _drawDebugInfo = false;
        GameState.Init();
    }

    protected override void Preload()
    {

        _sfxRotate = Content.Get<Audio>("rotate_sfx");
        _sfxLineClear = Content.Get<Audio>("lineclear_sfx");
        _sfxGameOver = Content.Get<Audio>("gameover_sfx");
        _sfxPlaceBlock = Content.Get<Audio>("placeblock_sfx");

        _view = new TetrisView();
        _view.Load();
        _view.UpdateLayout();

        GraphicsContext.SetViewClear(0, Color.Black);

        GameState.OnClearLines = OnClearLines;
        GameState.OnPlaceBlock = OnPlaceBlock;
        GameState.OnGameOver = OnGameOver;

    }

    private void OnClearLines(int lineCount)
    {
        _sfxLineClear.Play();
    }

    private void OnPlaceBlock(int blockId)
    {
        _sfxPlaceBlock.Play();
    }

    private void OnGameOver()
    {
        _sfxGameOver.Play();
    }

    protected override void FixedUpdate(float dt)
    {
        _view.Process();

        if (GameState.GameOver) return;

        _ticks += 1;

        float delay = Calc.Max(MinDelay, MaxDelay - (GameState.Score * DelayDecrease));

        if (!(_ticks > delay)) return;

        _ticks = 0;
        GameState.MoveBlockDown();
    }

    protected override void Update(float dt)
    {
        if (!GameState.GameOver)
        {
            if (Control.Keyboard.KeyPressed(Key.A))
            {
                GameState.MoveBlockLeft();
            }
            else if (Control.Keyboard.KeyPressed(Key.D))
            {
                GameState.MoveBlockRight();
            }
            else if (Control.Keyboard.KeyPressed(Key.J))
            {
                GameState.RotateBlockCCW();
                _sfxRotate.Play();
            }
            else if (Control.Keyboard.KeyPressed(Key.K))
            {
                GameState.RotateBlockCW();
                _sfxRotate.Play();
            }
            else if (Control.Keyboard.KeyDown(Key.S))
            {
                _moveDownTicks += 1;

                if (_moveDownTicks > MoveDownDelay)
                {
                    _moveDownTicks = 0;
                    GameState.MoveBlockDown();
                }
            }

            if (Control.Keyboard.KeyPressed(Key.Space))
            {
                GameState.DropBlock();
            }

            if (Control.Keyboard.KeyPressed(Key.Enter))
            {
                GameState.HoldBlock();
            }
        }
        else
        {
            if (Control.Keyboard.KeyPressed(Key.Space))
            {
                GameState.Restart();
            }
        }

    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        _view.Draw(canvas, dt);

        canvas.End();
    }
}