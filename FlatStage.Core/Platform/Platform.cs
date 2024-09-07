
using System.Runtime.InteropServices;
using SDL;
using static SDL.SDL3;

namespace FlatStage;

public enum PlatformId
{
    Windows,
    Mac,
    LinuxX11,
    LinuxWayland
}

public delegate void FileDropEvent(FileDropEventArgs args);

public readonly struct FileDropEventArgs(string[] files)
{
    /// <summary>
    /// The paths of dropped files
    /// </summary>
    public string[] Files { get; } = files;
}

internal static unsafe partial class Platform
{
    public static Action? OnQuit;
    public static Action<FileDropEventArgs>? OnFileDrop;

    public static PlatformId PlatformId { get; private set; }

    public static void Init(GameSettings settings)
    {
        Ensure64BitArch();

        DetectRunningPlatform();

        SDL_SetMainReady();

        SDL_SetHint("SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS", "0");
        SDL_SetHint("SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS", "1");

        if (SDL_Init((SDL_InitFlags)(SDL_INIT_VIDEO | SDL_INIT_GAMEPAD)) < 0)
        {
            SDL_Quit();
            throw new ApplicationException("Failed to initialize SDL");
        }

        CreateWindow(settings);

        InitGamePad();
    }

    public static void Shutdown()
    {
        Console.WriteLine("Platform: Shutdown");

        DestroyWindow();
        SDL_Quit();
    }

    public static void ProcessEvents()
    {
        SDL_Event evt;
        while (SDL_PollEvent(&evt) != 0)
        {
            switch (evt.type)
            {
                case (uint)SDL_EventType.SDL_EVENT_KEY_DOWN or (uint)SDL_EventType.SDL_EVENT_KEY_UP:
                    ProcessKeyEvent(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_MOUSE_MOTION
                    or (uint)SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN
                    or (uint)SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP
                    or (uint)SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                    ProcessMouseEvent(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_WINDOW_RESIZED or
                    (uint)SDL_EventType.SDL_EVENT_WINDOW_MINIMIZED or
                    (uint)SDL_EventType.SDL_EVENT_WINDOW_RESTORED or
                    (uint)SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER or
                    (uint)SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:

                    ProcessWindowEvent(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_GAMEPAD_ADDED
                    or (uint)SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
                    ProcessGamePadEvent(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_TEXT_INPUT:
                    ProcessTextInputEvent(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_DROP_FILE:
                    ProcessDropFile(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_DROP_COMPLETE:
                    CompleteDropFile(evt);
                    break;
                case (uint)SDL_EventType.SDL_EVENT_QUIT:
                    OnQuit?.Invoke();
                    break;
            }
        }
    }

    private static void ProcessDropFile(SDL_Event evt)
    {
        if (evt.drop.windowID != _windowId)
        {
            return;
        }

        string? path = evt.drop.GetData();

        if (string.IsNullOrEmpty(path)) return;

        _dropList ??= [];
        _dropList.Add(path);
    }

    private static void CompleteDropFile(SDL_Event evt)
    {
        if (evt.drop.windowID != _windowId || _dropList == null)
        {
            return;
        }

        if (_dropList.Count <= 0) return;

        OnFileDrop?.Invoke(new FileDropEventArgs(_dropList.ToArray()));

        _dropList.Clear();
    }

    public static void ShowRuntimeError(string title, string message)
    {
        SDL_ShowSimpleMessageBox(
            SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            title,
            message,
            _windowHandle
        );
    }

    public static void ShowMessageBox(string title, string message)
    {
        SDL_ShowSimpleMessageBox(
            SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
            title,
            message,
            _windowHandle
        );
    }

    public static double GetPerfFreq()
    {
        return SDL_GetPerformanceFrequency();
    }

    public static double GetPerfCounter()
    {
        return SDL_GetPerformanceCounter();
    }

    private static void DetectRunningPlatform()
    {
        if (OperatingSystem.IsWindows())
        {
            PlatformId = PlatformId.Windows;
        }
        else if (OperatingSystem.IsMacOS())
        {
            PlatformId = PlatformId.Mac;
        }
        else if (OperatingSystem.IsLinux())
        {
            var currentVideoDriver = SDL_GetCurrentVideoDriver();

            if (!string.IsNullOrEmpty(currentVideoDriver))
            {
                if (currentVideoDriver.Equals("x11"))
                {
                    PlatformId = PlatformId.LinuxX11;
                }
                else if (currentVideoDriver.Equals("wayland"))
                {
                    PlatformId = PlatformId.LinuxWayland;
                }

                return;
            }

            throw new Exception($"Unsupported Linux Platform: {currentVideoDriver}");

        }
        else
        {
            throw new InvalidOperationException("Unsupported Platform!");
        }
    }

    private static void Ensure64BitArch()
    {
        var runtimeArch = RuntimeInformation.OSArchitecture;
        if (runtimeArch is Architecture.Arm or Architecture.X86)
        {
            throw new ApplicationException("Only X64 Archicture is supported");
        }
    }

    private static List<string>? _dropList;
}
