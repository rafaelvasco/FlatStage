using System.Diagnostics;
using System.Runtime;

namespace FlatStage;
public abstract class Game : Disposable
{
    public static event FileDropEvent? OnFileDrop;

    public static Game Instance => _instance;

    public const string Version = "1.0";

    public GameSettings Settings { get; init; }

    public static Size WindowSize
    {
        get => Platform.GetWindowSize();
        set
        {
            var size = Platform.GetWindowSize();
            if (value.Width == size.Width && value.Height == size.Height)
            {
                return;
            }

            Platform.SetWindowSize(value.Width, value.Height);
        }
    }

    public static string Title
    {
        get => Platform.GetWindowTitle();
        set => Platform.SetWindowTitle(value);
    }

    public static bool Resizable
    {
        get => (Platform.GetWindowFlags() & WindowFlags.Resizable) != 0;
        set => Platform.SetWindowResizable(value);
    }

    public static bool Borderless
    {
        get => (Platform.GetWindowFlags() & WindowFlags.Borderless) != 0;
        set => Platform.SetWindowBorderless(value);
    }

    public static GameLoop GameLoop => _instance._loop;

    public static bool Fullscreen
    {
        get => Platform.IsFullscreen();
        set
        {
            if (Platform.IsFullscreen() == value)
            {
                return;
            }

            Platform.SetWindowFullscreen(value);
        }
    }

    public static bool ShowCursor
    {
        get => Platform.CursorVisible();
        set => Platform.ShowCursor(value);
    }

    public static void ShowMessageBox(string title, string message)
    {
        Platform.ShowMessageBox(title, message);
    }

    protected Game()
    {
        _instance = this;

        Settings = new GameSettings()
        {
            AppTitle = "FlatStage Game",
            CanvasWidth = 800,
            CanvasHeight = 600
        };

        _loop = new GameLoop();
    }

    protected override void Free()
    {
        Console.WriteLine("FlatStage Shutting Down");

        InternalUnload();

        Content.Shutdown();
        AudioContext.Shutdown();
        Graphics.Shutdown();
        Platform.Shutdown();

        Console.WriteLine($"GEN0 GC: {GC.CollectionCount(0)}");
        Console.WriteLine($"GEN1 GC: {GC.CollectionCount(1)}");
        Console.WriteLine($"GEN3 GC: {GC.CollectionCount(3)}");
    }

    public void Run()
    {
        if (_loop.Running)
        {
            return;
        }

        Initialize();

        GameSaveIO.SetRootPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Settings.AppTitle));

        PerfTimer.Begin();

        InternalLoad();

        Console.WriteLine($"Game Content Load Took: {PerfTimer.End()}");

        _loop.Start(this);

        Platform.ShowWindow(true);

        while (_loop.Running)
        {
            _loop.Tick(this);
        }
    }

    public static void Exit()
    {
        AudioContext.StopAll();

        Debug.Assert(_instance != null, nameof(_instance) + " != null");
        _instance._loop.Terminate();
    }

    internal void InternalFixedUpdate(float dt)
    {
        FixedUpdate(dt);
    }

    internal void UpdateInput()
    {
        Keyboard.UpdateState();
        Mouse.UpdateState();
        Gamepad.UpdateState();
    }

    internal void InternalUpdate(float dt)
    {
#if DEBUG

        if (Keyboard.KeyPressed(Key.F11))
        {
            Fullscreen = !Fullscreen;
        }

#endif

        Update(dt);
    }

    internal void InternalDraw(float dt)
    {
        _canvas.BeginRendering();

        Draw(_canvas, dt);

        _canvas.EndRendering();
    }

    internal void InternalLoad()
    {
        Load();
    }

    internal void InternalUnload()
    {
        Unload();
    }

    protected virtual void Load()
    {
    }

    protected virtual void Unload()
    {
    }

    protected abstract void Update(float dt);

    protected virtual void FixedUpdate(float dt) { }

    protected abstract void Draw(Canvas canvas, float dt);

    private void Initialize()
    {
        Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::");
        Console.WriteLine($"::::::: FlatStage {Version} Start :::::::::::::");
        Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::");

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        Content.Init();

        Console.WriteLine(Settings);

        PerfTimer.Begin();

        Platform.Init(Settings);

        Console.WriteLine($"Platform Init Took: {PerfTimer.End()}");

        InitInput();

        PerfTimer.Begin();

        Graphics.Init();

        Console.WriteLine($"Graphics Init Took: {PerfTimer.End()}");

        _canvas = new Canvas();

        PerfTimer.Begin();

        AudioContext.Init();

        Console.WriteLine($"Audio Init Took: {PerfTimer.End()}");

        PerfTimer.Begin();

        BuiltinContent.Load();

        Console.WriteLine($"Builtin Content Load Took: {PerfTimer.End()}");

        Platform.OnQuit = Exit;
        Platform.WindowMinimized = () => { _loop.IsActive = false; };
        Platform.WindowRestored = () => { _loop.IsActive = true; };
        Platform.OnFileDrop = ProcessFileDrop;

        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Platform.ShowRuntimeError("FlatStage", "We crashed. Check log.txt for more.");
        Logger.Write(((Exception)e.ExceptionObject).Message, LogLevel.Error);
        Logger.Flush();
    }

    private void InitInput()
    {
        Keyboard.Init();
        Mouse.Init();
        Gamepad.Init();
    }

    private void ProcessFileDrop(FileDropEventArgs fileDropArgs) => OnFileDrop?.Invoke(fileDropArgs);

    private static Game _instance = null!;
    private Canvas _canvas = null!;
    private readonly GameLoop _loop;

}
