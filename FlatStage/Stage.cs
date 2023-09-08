using System;
using System.Runtime;
using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Platform;
using FlatStage.Sound;
using System.IO;

namespace FlatStage;

public partial class Stage : Disposable
{
    public static event FileDropEvent? OnFileDrop;

    public const string Version = "0.1";

    public static StageSettings Settings { get; private set; } = null!;

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

    public Stage()
    {
        _instance = this;

        Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::");
        Console.WriteLine($"::::::: FlatStage {Version} Start :::::::::::::");
        Console.WriteLine(":::::::::::::::::::::::::::::::::::::::::");

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        Content.Init();

        Settings = Content.LoadSettingsOrDefault();

        Console.WriteLine(Settings);

        PlatformContext.Init(Settings);

        InitInput();

        GraphicsContext.Init();

        _canvas = new Canvas();

        AudioContext.Init();

        BuiltinContent.Load();

        InitLoop();

        PlatformContext.OnQuit = Exit;
        PlatformContext.WindowMinimized = () => { IsActive = false; };
        PlatformContext.WindowRestored = () => { IsActive = true; };
        PlatformContext.OnFileDrop = ProcessFileDrop;

        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

    }

    public void Run(Game game)
    {
        if (_running)
        {
            return;
        }

        GameSaveIO.SetRootPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), game.Name));

        _runningGame = game;

        _running = true;

        _runningGame.InternalPreload();

        Tick(_runningGame);

        PlatformContext.ShowWindow(true);

        _prevFrameTime = PlatformContext.GetPerfCounter();
        _frameAccum = 0;

        while (_running)
        {
            Tick(_runningGame);
        }
    }

    public static void Exit()
    {
        AudioContext.StopAll();

        if (!_instance._running)
        {
            return;
        }

        _instance._suppressDraw = true;

        _instance._running = false;
    }

    protected override void Free()
    {
        Console.WriteLine("FlatStage Shutting Down");

        _runningGame?.InternalUnload();

        Content.Shutdown();
        AudioContext.Shutdown();
        GraphicsContext.Shutdown();
        PlatformContext.Shutdown();

        Console.WriteLine($"GEN0 GC: {GC.CollectionCount(0)}");
        Console.WriteLine($"GEN1 GC: {GC.CollectionCount(1)}");
        Console.WriteLine($"GEN3 GC: {GC.CollectionCount(3)}");
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage((Exception)e.ExceptionObject);
    }

    private static void ShowExceptionMessage(Exception ex)
    {
        PlatformContext.ShowRuntimeError("FlatStage", $"An Error Occurred: {ex.Message}");
    }

    private static void InitInput()
    {
        Keyboard.Init();
        Mouse.Init();
        Gamepad.Init();
    }

    private static void UpdateInput()
    {
        Keyboard.UpdateState();
        Mouse.UpdateState();
        Gamepad.UpdateState();
    }

    private static void ProcessFileDrop(FileDropEventArgs fileDropArgs) => OnFileDrop?.Invoke(fileDropArgs);

    private static Stage _instance = null!;
    private static Canvas _canvas = null!;
    private static Game? _runningGame;
}
