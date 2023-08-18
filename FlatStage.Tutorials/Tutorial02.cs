using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Sound;

namespace FlatStage.Tutorials;

public class Tutorial02 : Game
{
    private Texture? _texture;

    private float _particleX = 100.0f;
    private float _particleY = 100.0f;
    private float _dx;
    private float _dy;

    private const float Speed = 90;
    private const float Friction = 0.98f;

    private Audio _bumpEffect = null!;

    protected override void Preload()
    {
        _texture = Content.Get<Texture>("particles");

        _bumpEffect = Content.Get<Audio>("blip");

        _bumpEffect.Volume = 0.1f;

        GraphicsContext.SetViewClear(0, Color.DarkBlue);
    }

    protected override void Draw(Canvas2D canvas, float dt)
    {
        canvas.Begin(
            BlendState.Additive,
            SamplerState.PointClamp,
            RasterizerState.CullCounterClockWise,
            Matrix.Identity,
            null
        );

        canvas.Draw(_texture!, _particleX, _particleY, new Rect(96, 64, 32, 32), Color.Cyan);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    private void Bump()
    {
        var pan = (_particleX - Stage.WindowSize.Width / 2f) / (Stage.WindowSize.Width / 2f);
        var pitch = (_dx * _dx + _dy * _dy) * 0.0005f + 0.2f;

        _bumpEffect.PlayEx(pan, pitch);
    }

    protected override void Update(float dt)
    {
        if (Control.Keyboard.KeyDown(Key.Left))
        {
            _dx -= Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Right))
        {
            _dx += Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Up))
        {
            _dy -= Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Down))
        {
            _dy += Speed * dt;
        }

        _dx *= Friction;
        _dy *= Friction;

        _particleX += _dx;
        _particleY += _dy;

        if (_particleX > GraphicsContext.BackbufferWidth - 32)
        {
            _particleX = GraphicsContext.BackbufferWidth - 32;
            _dx = -_dx;
            Bump();
        }
        else if (_particleX < 0)
        {
            _particleX = 0;
            _dx = -_dx;
            Bump();
        }

        if (_particleY > GraphicsContext.BackbufferHeight - 32)
        {
            _particleY = GraphicsContext.BackbufferHeight - 32;
            _dy = -_dy;
            Bump();
        }
        else if (_particleY < 0)
        {
            _particleY = 0;
            _dy = -_dy;
            Bump();
        }
    }
}