namespace FlatStage;

public abstract class Game
{
    public bool EscapeQuits { get; set; } = false;

    public Game()
    {
        _canvas = new Canvas2D();

#if DEBUG
        _drawDebugInfo = true;
#endif
    }

    internal void InternalFixedUpdate(float dt)
    {
#if DEBUG
        _timeDeltaFixed = dt;
#endif

        FixedUpdate(dt);
    }

    internal void InternalUpdate(float dt)
    {
#if DEBUG

        _timeDelta = dt;

        if (Input.Keyboard.KeyPressed(Key.F11))
        {
            Stage.Fullscreen = !Stage.Fullscreen;
        }
        else if (Input.Keyboard.KeyPressed(Key.F2))
        {
            _drawDebugInfo = !_drawDebugInfo;
        }


#endif

        if (EscapeQuits && Input.Keyboard.KeyPressed(Key.Escape))
        {
            Stage.Exit();
        }

        Update(dt);
    }

    internal void InternalDraw(float dt)
    {
#if DEBUG
        Graphics.ClearDebugText();
        if (_drawDebugInfo)
        {
            Graphics.DebugTextWrite(2, 2, DebugColor.White, DebugColor.Black, $"FLATSTAGE ENGINE {Stage.Version}");
            Graphics.DebugTextWrite(2, 3, DebugColor.White, DebugColor.Black,
                $"Variable Dt: {_timeDelta}, Fixed Dt: {_timeDeltaFixed}, Draw Dt: {dt}");
        }

#endif

        Draw(_canvas, dt);
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

    protected abstract void Draw(Canvas2D canvas, float dt);

    private readonly Canvas2D _canvas;

#if DEBUG

    private bool _drawDebugInfo;
    private float _timeDelta;
    private float _timeDeltaFixed;

#endif
}