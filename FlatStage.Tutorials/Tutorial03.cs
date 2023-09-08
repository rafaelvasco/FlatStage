using FlatStage.ContentPipeline;
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
        _song1 = Content.Get<Audio>("delphi_loop");
        _effect = Content.Get<Audio>("blip");

        _effect.Volume = 0.1f;
    }

    public override void Draw(Canvas canvas, float dt)
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
            _effect.PlayWithPanPitch(0f, Random.Default.Next(1, 5) / 10f);
        }
    }
}
