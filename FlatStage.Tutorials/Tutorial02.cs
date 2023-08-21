using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Sound;

namespace FlatStage.Tutorials;

public class Tutorial02 : Scene
{
    private Texture? _texture;

    private Vec2 _particlePos = new Vec2(100.0f, 100.0f);
    private Vec2 _delta;

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

        canvas.Draw(_texture!, _particlePos, new Rect(96, 64, 32, 32), Vec2.Zero, Color.Cyan);

        canvas.End();
    }

    protected override void FixedUpdate(float dt)
    {
    }

    private void Bump()
    {
        var pan = (_particlePos.X - Stage.WindowSize.Width / 2f) / (Stage.WindowSize.Width / 2f);
        var pitch = (_delta.X * _delta.X + _delta.Y * _delta.Y) * 0.0005f + 0.2f;

        _bumpEffect.PlayEx(pan, pitch);
    }

    protected override void Update(float dt)
    {
        if (Control.Keyboard.KeyDown(Key.Left))
        {
            _delta.X -= Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Right))
        {
            _delta.X += Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Up))
        {
            _delta.Y -= Speed * dt;
        }

        if (Control.Keyboard.KeyDown(Key.Down))
        {
            _delta.Y += Speed * dt;
        }

        _delta *= Friction;

        _particlePos += _delta;

        if (_particlePos.X > GraphicsContext.BackbufferWidth - 32)
        {
            _particlePos.X = GraphicsContext.BackbufferWidth - 32;
            _delta.X = -_delta.X;
            Bump();
        }
        else if (_particlePos.X < 0)
        {
            _particlePos.X = 0;
            _delta.X = -_delta.X;
            Bump();
        }

        if (_particlePos.Y > GraphicsContext.BackbufferHeight - 32)
        {
            _particlePos.Y = GraphicsContext.BackbufferHeight - 32;
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