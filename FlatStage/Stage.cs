using System;
using System.Runtime;
using FlatStage.ContentPipeline;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Platform;
using FlatStage.Sound;

namespace FlatStage;

public partial class Stage : Disposable
{
    public const string Version = "0.1";

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

        var settings = Content.LoadSettingsOrDefault();

        Console.WriteLine(settings);

        PlatformContext.Init(settings);

        Control.Init(settings);

        GraphicsContext.Init(settings);

        AudioContext.Init();

        InitLoop();

        PlatformContext.OnQuit = Exit;
        PlatformContext.WindowMinimized = () => { IsActive = false; };
        PlatformContext.WindowRestored = () => { IsActive = true; };

        GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
    }

    public void Run(Game game)
    {
        if (_running)
        {
            return;
        }

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
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage((Exception)e.ExceptionObject);
    }

    private static void ShowExceptionMessage(Exception ex)
    {
        PlatformContext.ShowRuntimeError("FlatStage", $"An Error Occurred: {ex.Message}");
    }

    private static Stage _instance = null!;
    private static Game? _runningGame;
}