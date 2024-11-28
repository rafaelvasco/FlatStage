namespace FlatStage.Demos;

public class Tutorial02(string name) : BaseTutorial(name)
{
    private Texture? _texture;

    private Vec2 _particlePos = new(100.0f, 100.0f);
    private Vec2 _delta;

    private const float Speed = 90;
    private const float Friction = 0.98f;

    private Sound _bumpEffect = null!;

    public override void Load()
    {
        _texture = Content.Get<Texture>("particles");

        _bumpEffect = Content.Get<Sound>("blip");

        //_bumpEffect.Volume = 0.1f;
    }

    public override void Draw(Canvas canvas)
    {
        canvas.Draw(_texture!, _particlePos, new Rect(96, 64, 32, 32), Vec2.Zero, Color.Cyan);
    }

    private void Bump()
    {
        var pan = (_particlePos.X - (Canvas.Width / 2f)) / (Canvas.Width / 2f);
        var pitch = (((_delta.X * _delta.X) + (_delta.Y * _delta.Y)) * 0.0005f) + 0.2f;

        //_bumpEffect.PlayWithPanPitch(pan, pitch);
    }

    public override void Update(float dt)
    {
        if (Keyboard.KeyDown(Key.Left))
        {
            _delta.X -= Speed * dt;
        }

        if (Keyboard.KeyDown(Key.Right))
        {
            _delta.X += Speed * dt;
        }

        if (Keyboard.KeyDown(Key.Up))
        {
            _delta.Y -= Speed * dt;
        }

        if (Keyboard.KeyDown(Key.Down))
        {
            _delta.Y += Speed * dt;
        }

        _delta *= Friction;

        _particlePos += _delta;

        if (_particlePos.X > Canvas.Width - 32)
        {
            _particlePos.X = Canvas.Width - 32;
            _delta.X = -_delta.X;
            Bump();
        }
        else if (_particlePos.X < 0)
        {
            _particlePos.X = 0;
            _delta.X = -_delta.X;
            Bump();
        }

        if (_particlePos.Y > Canvas.Height - 32)
        {
            _particlePos.Y = Canvas.Height - 32;
            _delta.Y = -_delta.Y;
            Bump();
        }
        else if (_particlePos.Y < 0)
        {
            _particlePos.Y = 0;
            _delta.Y = -_delta.Y;
            Bump();
        }
    }
}
