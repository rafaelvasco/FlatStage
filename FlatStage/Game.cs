using FlatStage.Content;
using FlatStage.Graphics;
using FlatStage.Platform;
using FlatStage.Sound;
using System.Runtime;
using System;
using FlatStage.Input;
using System.IO;
using FlatStage.Utils;

namespace FlatStage;
public abstract class Game : Disposable
{
    public static event FileDropEvent? OnFileDrop;

    public static Game Instance => _instance;

    public const string Version = "1.0";

    public GameSettings Settings { get; init; } = null!;

    public static Size WindowSize
    {
        get => PlatformContext.GetWindowSize();
        set
        {
            var size = PlatformContext.GetWindowSize();
            if (value.Width == size.Width && value.Height == size.Height)
            {
                return;
            }

            PlatformContext.SetWindowSize(value.Width, value.Height);
        }
    }

    public static string Title
    {
        get => PlatformContext.GetWindowTitle();
        set => PlatformContext.SetWindowTitle(value);
    }

    public static bool Resizable
    {
        get => (PlatformContext.GetWindowFlags() & WindowFlags.Resizable) != 0;
        set => PlatformContext.SetWindowResizable(value);
    }

    public static bool Borderless
    {
        get => (PlatformContext.GetWindowFlags() & WindowFlags.Borderless) != 0;
        set => PlatformContext.SetWindowBorderless(value);
    }

    public static GameLoop GameLoop => _instance._loop;

    public static bool Fullscreen
    {
        get => PlatformContext.IsFullscreen();
        set
        {
            if (PlatformContext.IsFullscreen() == value)
            {
                return;
            }

            PlatformContext.SetWindowFullscreen(value);
        }
    }

    public static bool ShowCursor
    {
        get => PlatformContext.CursorVisible();
        set => PlatformContext.ShowCursor(value);
    }

    public static void ShowMessageBox(string title, string message)
    {
        PlatformContext.ShowMessageBox(title, message);
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

        Assets.Shutdown();
        AudioContext.Shutdown();
        GraphicsContext.Shutdown();
        PlatformContext.Shutdown();

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

        PlatformContext.ShowWindow(true);

        while (_loop.Running)
        {
            _loop.Tick(this);
        }
    }

    public static void Exit()
    {
        AudioContext.StopAll();

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
        Assets.Init();

        Console.WriteLine(Settings);

        PerfTimer.Begin();

        PlatformContext.Init(Settings);

        Console.WriteLine($"Platform Init Took: {PerfTimer.End()}");

        InitInput();

        PerfTimer.Begin();

        GraphicsContext.Init();

        Console.WriteLine($"Graphics Init Took: {PerfTimer.End()}");

        _canvas = new Canvas();

        PerfTimer.Begin();

        AudioContext.Init();

        Console.WriteLine($"Audio Init Took: {PerfTimer.End()}");

        PerfTimer.Begin();

        BuiltinContent.Load();

        Console.WriteLine($"Builtin Content Load Took: {PerfTimer.End()}");

        PlatformContext.OnQuit = Exit;
        PlatformContext.WindowMinimized = () => { _loop.IsActive = false; };
        PlatformContext.WindowRestored = () => { _loop.IsActive = true; };
        PlatformContext.OnFileDrop = ProcessFileDrop;

        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage((Exception)e.ExceptionObject);
    }

    private static void ShowExceptionMessage(Exception ex)
    {
        PlatformContext.ShowRuntimeError("FlatStage", $"An Error Occurred: {ex.Message}");
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
    private GameLoop _loop = null!;

}
