using System;
using System.Runtime;
using FlatStage.Sound;

namespace FlatStage;

public partial class Stage : Disposable
{
    public const string Version = "0.1";

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

        Platform.Init(settings);

        Input.Init(settings);

        Graphics.Init(settings);

        AudioManager.Init();

        InitLoop();

        Platform.OnQuit = Exit;
        Platform.WindowMinimized = () => { IsActive = false; };
        Platform.WindowRestored = () => { IsActive = true; };

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

        Platform.ShowWindow(true);

        _prevFrameTime = Platform.GetPerfCounter();
        _frameAccum = 0;

        while (_running)
        {
            Tick(_runningGame);
        }
    }

    public static void Exit()
    {
        AudioManager.StopAll();

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
        AudioManager.Shutdown();
        Graphics.Shutdown();
        Platform.Shutdown();
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowExceptionMessage((Exception)e.ExceptionObject);
    }

    private static void ShowExceptionMessage(Exception ex)
    {
        Platform.ShowRuntimeError("FlatStage", $"An Error Occurred: {ex.Message}");
    }

    private static Stage _instance = null!;
    private static Game? _runningGame;
}