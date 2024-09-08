

namespace FlatStage;

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


internal static unsafe partial class Platform
{
    public static Action<Size>? WindowResized;
    public static Action? WindowMinimized;
    public static Action? WindowRestored;
    public static Action? WindowEntered;
    public static Action? WindowExited;


    private static void CreateWindow(GameSettings settings)
    {
        var windowFlags =
            (SDL.SDL_WindowFlags.Hidden |
             SDL.SDL_WindowFlags.InputFocus |
             SDL.SDL_WindowFlags.MouseFocus);

        if (settings.Fullscreen)
        {
            windowFlags |= SDL.SDL_WindowFlags.Fullscreen;
        }

        SDL_PropertiesID windowProps = SDL.SDL_CreateProperties();

        SDL_SetStringProperty(windowProps, "title", settings.AppTitle);
        SDL_SetNumberProperty(windowProps, "x", SDL_WINDOWPOS_CENTERED);
        SDL_SetNumberProperty(windowProps, "y", SDL_WINDOWPOS_CENTERED);
        SDL_SetNumberProperty(windowProps, "width", settings.CanvasWidth);
        SDL_SetNumberProperty(windowProps, "height", settings.CanvasHeight);
        SDL_SetNumberProperty(windowProps, "flags", (long)windowFlags);

        _windowHandle = SDL_CreateWindowWithProperties(windowProps);

        if (_windowHandle == null)
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
        var mode = SDL_GetWindowFullscreenMode(_windowHandle);

        return new WindowMode
        {
            Fullscreen = IsFullscreen(),
            Width = mode->w,
            Height = mode->h,
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
            _ = SDL_SetWindowFullscreen(_windowHandle, fullscreen ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE);
        }
    }

    public static void SetWindowSize(int width, int height)
    {
        if (IsFullscreen())
        {
            return;
        }

        SDL_SetWindowSize(_windowHandle, width, height);
        SDL_SetWindowPosition(_windowHandle, (int)SDL_WINDOWPOS_CENTERED, (int)SDL_WINDOWPOS_CENTERED);
    }

    public static Size GetWindowSize()
    {
        int w;
        int h;

        SDL_GetWindowSize(_windowHandle, &w, &h);
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
        return SDL_GetWindowTitle(_windowHandle) ?? string.Empty;
    }

    public static void ShowCursor(bool show)
    {
        if (show)
        {
            SDL_ShowCursor();
        }
        else
        {
            SDL_HideCursor();
        }
    }

    public static bool CursorVisible()
    {
        return SDL_CursorVisible() == SDL_bool.SDL_TRUE;
    }

    public static WindowFlags GetWindowFlags()
    {
        return (WindowFlags)SDL_GetWindowFlags(_windowHandle);
    }

    internal static NativeDisplayHandles GetDisplayNativeHandles()
    {
        string GetWindowHandlePropName()
        {
            return PlatformId switch
            {
                PlatformId.Windows => "SDL.window.win32.hwnd",
                PlatformId.Mac => "SDL.window.cocoa.window",
                PlatformId.LinuxX11 => "SDL.window.x11.window",
                PlatformId.LinuxWayland => "SDL.window.wayland.window",
                _ => throw new Exception($"Can't get WindowHandlePropName for this Platform: {PlatformId}")
            };
        }

        string GetDisplayHandlePropName()
        {
            return PlatformId switch
            {
                PlatformId.Windows => string.Empty,
                PlatformId.Mac => string.Empty,
                PlatformId.LinuxX11 => "SDL.window.x11.display",
                PlatformId.LinuxWayland => "SDL.window.wayland.display",
                _ => throw new Exception($"Can't get WindowHandlePropName for this Platform: {PlatformId}")
            };
        }

        var windowProps = SDL_GetWindowProperties(_windowHandle);

        var nativeWindowHandle = SDL_GetPointerProperty(windowProps, GetWindowHandlePropName(), nint.Zero);
        var nativeDisplayHandle = SDL_GetPointerProperty(windowProps, GetDisplayHandlePropName(), nint.Zero);

        return new NativeDisplayHandles()
        {
            WindowHandle = nativeWindowHandle,
            DisplayType = nativeDisplayHandle
        };
    }

    private static void ProcessWindowEvent(SDL_Event ev)
    {
        switch (ev.window.type)
        {
            case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:

                var newW = ev.window.data1;
                var newH = ev.window.data2;
                WindowResized?.Invoke(new Size(newW, newH));
                break;

            case SDL_EventType.SDL_EVENT_WINDOW_MINIMIZED:
                WindowMinimized?.Invoke();
                break;
            case SDL_EventType.SDL_EVENT_WINDOW_RESTORED:
                WindowRestored?.Invoke();
                break;
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER:
                WindowEntered?.Invoke();
                break;
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:
                WindowExited?.Invoke();
                break;
        }
    }

    private static void DestroyWindow()
    {
        if (_windowHandle != null)
        {
            SDL_DestroyWindow(_windowHandle);
            _windowHandle = null;
        }
    }

    private static SDL_Window* _windowHandle;
    private static SDL_WindowID _windowId;
}
