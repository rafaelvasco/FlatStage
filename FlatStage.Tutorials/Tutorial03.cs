using FlatStage.Content;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Sound;

namespace FlatStage.Tutorials;

public class Tutorial03 : BaseTutorial
{
    private Audio _song1 = null!;
    private Audio _effect = null!;

    public Tutorial03(string name) : base(name)
    {
    }

    public override void Load()
    {
        _song1 = Assets.Get<Audio>("delphi_loop");
        _effect = Assets.Get<Audio>("blip");

        _effect.Volume = 0.1f;
    }

    public override void Draw(Canvas canvas)
    {
    }

    public override void Update(float dt)
    {
        if (Keyboard.KeyPressed(Key.Space))
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

        if (Mouse.ButtonPressed(MouseButton.Left))
        {
            _effect.PlayWithPanPitch(0f, PRNG.NextFloat(1f, 5f) / 10f);
        }
    }
}
