using FlatStage.Sound;

namespace FlatStage.Tutorials;

public class Tutorial03 : Game
{
    private Audio _song1 = null!;
    private Audio _effect = null!;

    protected override void Preload()
    {
        _song1 = Content.Get<Audio>("delphi_loop_wav");
        _effect = Content.Get<Audio>("blip");

        _effect.Volume = 0.1f;

        Graphics.SetViewClear(0, Color.Black);
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin();

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    protected override void Update(float dt)
    {
        if (Input.Keyboard.KeyPressed(Key.Space))
        {
            if (!_song1.IsPlaying)
            {
                _song1.Play();
            }
            else
            {
                _song1.Stop();
            }
        }

        if (Input.Mouse.ButtonPressed(MouseButton.Left))
        {
            _effect.PlayEx(0f, (float)RandomEx.Instance.Gen(0.1, 0.5));
            _effect.Play();
        }
    }
}