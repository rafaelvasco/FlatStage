using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using static FlatStage.Foundation.SDL2.SDL;

namespace FlatStage.Platform;

public enum PlatformId
{
    Windows,
    Mac,
    Linux
}

public delegate void FileDropEvent(FileDropEventArgs args);

public readonly struct FileDropEventArgs
{
    public FileDropEventArgs(string[] files)
    {
        Files = files;
    }

    /// <summary>
    /// The paths of dropped files
    /// </summary>
    public string[] Files { get; }
}

internal static partial class PlatformContext
{
    public static Action OnQuit = null!;
    public static Action<FileDropEventArgs> OnFileDrop = null!;

    public static PlatformId PlatformId { get; private set; }

    public static void Init(StageSettings settings)
    {
        Ensure64BitArch();

        DetectRunningPlatform();

        SDL_SetMainReady();

#if DEBUG

        if (PlatformId == PlatformId.Windows)
        {
            SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
        }

#endif

        SDL_SetHint("SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS", "0");
        SDL_SetHint("SDL_JOYSTICK_ALLOW_BACKGROUND_EVENTS", "1");

        if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_JOYSTICK | SDL_INIT_GAMECONTROLLER | SDL_INIT_HAPTIC) < 0)
        {
            SDL_Quit();
            throw new ApplicationException("Failed to initialize SDL");
        }

        CreateWindow(settings);

        InitMouse();
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
        while (SDL_PollEvent(out SDL_Event evt) != 0)
        {
            switch (evt.type)
            {
                case SDL_EventType.SDL_KEYDOWN or SDL_EventType.SDL_KEYUP:
                    ProcessKeyEvent(evt);
                    break;
                case SDL_EventType.SDL_MOUSEMOTION
                    or SDL_EventType.SDL_MOUSEBUTTONDOWN
                    or SDL_EventType.SDL_MOUSEBUTTONUP
                    or SDL_EventType.SDL_MOUSEWHEEL:
                    ProcessMouseEvent(evt);
                    break;
                case SDL_EventType.SDL_WINDOWEVENT:
                    ProcessWindowEvent(evt);
                    break;
                case SDL_EventType.SDL_CONTROLLERDEVICEADDED
                    or SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                    ProcessGamePadEvent(evt);
                    break;
                case SDL_EventType.SDL_TEXTINPUT:
                    ProcessTextInputEvent(evt);
                    break;
                case SDL_EventType.SDL_DROPFILE:
                    ProcessDropFile(evt);
                    break;
                case SDL_EventType.SDL_DROPCOMPLETE:
                    CompleteDropFile(evt);
                    break;
                case SDL_EventType.SDL_QUIT:
                    OnQuit.Invoke();
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

        string path = UTF8_ToManaged(evt.drop.file, freePtr: true);

        _dropList ??= new List<string>();

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
            IntPtr.Zero
        );
    }

    public static void ShowMessageBox(string title, string message)
    {
        SDL_ShowSimpleMessageBox(
            SDL_MessageBoxFlags.SDL_MESSAGEBOX_INFORMATION,
            title,
            message,
            IntPtr.Zero
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
            PlatformId = PlatformId.Linux;
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