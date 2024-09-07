﻿namespace FlatStage.Demos;

public class Tutorial03(string name) : BaseTutorial(name)
{
    private Audio _song1 = null!;
    private Audio _effect = null!;

    public override void Load()
    {
        _song1 = Content.Get<Audio>("delphi_loop");
        _effect = Content.Get<Audio>("blip");

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