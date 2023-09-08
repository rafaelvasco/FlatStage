using FlatStage.Input;
using FlatStage.Graphics;

namespace FlatStage;

public abstract class Game
{
    public abstract string Name { get; }

    public bool EscapeQuits { get; set; } = false;

    public bool F11ToggleFullscreen { get; set; } = false;

    public Game()
    {

    }

    internal void InternalFixedUpdate(float dt)
    {
        FixedUpdate(dt);
    }

    internal void InternalUpdate(float dt)
    {
        if (F11ToggleFullscreen && Keyboard.KeyPressed(Key.F11))
        {
            Stage.Fullscreen = !Stage.Fullscreen;
        }

        if (EscapeQuits && Keyboard.KeyPressed(Key.Escape))
        {
            Stage.Exit();
        }

        Update(dt);
    }

    internal void InternalDraw(Canvas canvas, float dt)
    {
        canvas.BeginRendering();

        Draw(canvas, dt);

        canvas.EndRendering();
    }

    internal void InternalPreload()
    {
        Preload();
    }

    internal void InternalUnload()
    {
        Unload();
    }

    protected virtual void Preload()
    {
    }

    protected virtual void Unload()
    {
    }

    protected virtual void FixedUpdate(float dt)
    {
    }

    protected abstract void Update(float dt);

    protected abstract void Draw(Canvas canvas, float dt);

}
