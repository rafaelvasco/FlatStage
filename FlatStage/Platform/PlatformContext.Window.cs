using System;

using static FlatStage.Foundation.SDL2.SDL;

namespace FlatStage.Platform;

internal struct NativeDisplayHandles
{
    public IntPtr WindowHandle;
    public IntPtr? DisplayType;
}

public struct WindowMode
{
    public int Width;
    public int Height;
    public bool Fullscreen;
    public int RefreshRate;
}

[Flags]
public enum WindowFlags
{
    Fullscreen = 0x00000001,
    Shown = 0x00000004,
    Hidden = 0x00000008,
    Borderless = 0x00000010,
    Resizable = 0x00000020,
    Minimized = 0x00000040,
    Maximized = 0x00000080,
    InputFocus = 0x00000200,
    MouseFocus = 0x00000400,
    FullscreenDesktop = 0x00001001,
    AllowHighDpi = 0x00002000,
    MouseCapture = 0x00004000
}

internal static partial class PlatformContext
{
    public static Action<Size> WindowResized = null!;
    public static Action WindowMinimized = null!;
    public static Action WindowRestored = null!;

    private static void CreateWindow(GameSettings settings)
    {
        var windowFlags =
            SDL_WindowFlags.SDL_WINDOW_HIDDEN |
            SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS |
            SDL_WindowFlags.SDL_WINDOW_MOUSE_FOCUS;

        if (settings.Fullscreen)
        {
            windowFlags |= SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP;
        }

        _windowHandle = SDL_CreateWindow(
            settings.AppTitle ?? "FlatStage",
            SDL_WINDOWPOS_CENTERED,
            SDL_WINDOWPOS_CENTERED,
            settings.CanvasWidth,
            settings.CanvasHeight,
            windowFlags
        );

        if (_windowHandle == IntPtr.Zero)
        {
            throw new ApplicationException("Could not create Window");
        }

        _windowId = SDL_GetWindowID(_windowHandle);
    }

    public static void ShowWindow(bool show)
    {
        if (show)
        {
            SDL_ShowWindow(_windowHandle);
        }
        else
        {
            SDL_HideWindow(_windowHandle);
        }
    }

    public static WindowMode GetWindowMode()
    {
        _ = SDL_GetWindowDisplayMode(_windowHandle, out SDL_DisplayMode mode);

        return new WindowMode
        {
            Fullscreen = IsFullscreen(),
            Width = mode.w,
            Height = mode.h,
        };
    }

    public static bool IsFullscreen()
    {
        return (GetWindowFlags() & WindowFlags.Fullscreen) == WindowFlags.Fullscreen;
    }

    public static void SetWindowFullscreen(bool fullscreen)
    {
        if (IsFullscreen() != fullscreen)
        {
            _ = SDL_SetWindowFullscreen(_windowHandle, (uint)(fullscreen ? WindowFlags.FullscreenDesktop : 0));
        }
    }

    public static void SetWindowSize(int width, int height)
    {
        if (IsFullscreen())
        {
            return;
        }

        SDL_SetWindowSize(_windowHandle, width, height);
        SDL_SetWindowPosition(_windowHandle, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
    }

    public static Size GetWindowSize()
    {
        SDL_GetWindowSize(_windowHandle, out var w, out var h);
        return new Size(w, h);
    }

    public static void SetWindowBorderless(bool borderless)
    {
        SDL_SetWindowBordered(_windowHandle, borderless ? SDL_bool.SDL_FALSE : SDL_bool.SDL_TRUE);
    }

    public static void SetWindowResizable(bool resizable)
    {
        SDL_SetWindowResizable(_windowHandle, resizable ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
    }

    public static void SetWindowTitle(string title)
    {
        SDL_SetWindowTitle(_windowHandle, title);
    }

    public static string GetWindowTitle()
    {
        return SDL_GetWindowTitle(_windowHandle);
    }

    public static void ShowCursor(bool show)
    {
        _ = SDL_ShowCursor(show ? 1 : 0);
    }

    public static bool CursorVisible()
    {
        var state = SDL_ShowCursor(SDL_QUERY);
        return state == SDL_ENABLE;
    }

    public static WindowFlags GetWindowFlags()
    {
        return (WindowFlags)SDL_GetWindowFlags(_windowHandle);
    }

    internal static NativeDisplayHandles GetDisplayNativeHandles()
    {
        var info = new SDL_SysWMinfo();

        SDL_GetWindowWMInfo(_windowHandle, ref info);

        return PlatformId switch
        {
            PlatformId.Windows => new NativeDisplayHandles
            {
                WindowHandle = info.info.win.window,
                DisplayType = null
            },
            PlatformId.Linux => new NativeDisplayHandles
            {
                WindowHandle = info.info.x11.window,
                DisplayType = info.info.x11.display
            },
            PlatformId.Mac => new NativeDisplayHandles
            {
                WindowHandle = info.info.cocoa.window,
                DisplayType = null
            },
            _ => throw new ApplicationException("Could not retrieve native window handle")
        };
    }

    private static void ProcessWindowEvent(SDL_Event ev)
    {
        switch (ev.window.windowEvent)
        {
            case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:

                var newW = ev.window.data1;
                var newH = ev.window.data2;
                WindowResized.Invoke(new Size(newW, newH));
                break;

            case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
                WindowMinimized.Invoke();
                break;
            case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
                WindowRestored.Invoke();
                break;
        }
    }

    private static void DestroyWindow()
    {
        if (_windowHandle != IntPtr.Zero)
        {
            SDL_DestroyWindow(_windowHandle);
        }
    }

    private static IntPtr _windowHandle;
    private static uint _windowId;
}
