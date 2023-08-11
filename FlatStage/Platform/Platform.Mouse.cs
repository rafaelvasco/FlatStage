using System;
using static FlatStage.Foundation.SDL2.SDL;

namespace FlatStage;

internal static partial class Platform
{
    private static bool _supportsGlobalMouse;

    public static Action<MouseButton>? MouseUp;
    public static Action<MouseButton>? MouseDown;
    public static Action<int, int>? MouseMove;

    private static int _mWheelValue;

    public static MouseState GetMouseState()
    {
        uint flags;
        int x, y;
        if (GetRelativeMouseMode())
        {
            flags = SDL_GetRelativeMouseState(out x, out y);
        }
        else if (_supportsGlobalMouse)
        {
            flags = SDL_GetGlobalMouseState(out x, out y);
            SDL_GetWindowPosition(_windowHandle, out int wx, out int wy);
            x -= wx;
            y -= wy;
        }
        else
        {
            flags = SDL_GetMouseState(out x, out y);
        }

        var left = (flags & SDL_BUTTON_LMASK) > 0;
        var middle = ((flags & SDL_BUTTON_MMASK) >> 1) > 0;
        var right = ((flags & SDL_BUTTON_RMASK) >> 2) > 0;

        return new MouseState(x, y, _mWheelValue, left, middle, right);
    }

    public static void SetMousePosition(int x, int y)
    {
        SDL_WarpMouseInWindow(_windowHandle, x, y);
    }

    public static bool GetRelativeMouseMode()
    {
        return SDL_GetRelativeMouseMode() == SDL_bool.SDL_TRUE;
    }

    public static void SetRelativeMouseMode(bool enable)
    {
        _ = SDL_SetRelativeMouseMode(
            enable ? SDL_bool.SDL_TRUE : SDL_bool.SDL_FALSE
        );
    }

    private static void InitMouse()
    {
        _supportsGlobalMouse =
            PlatformId is PlatformId.Windows or PlatformId.Mac or PlatformId.Linux;
    }

    private static void ProcessMouseEvent(SDL_Event evt)
    {
        var button = TranslateBooPlatformMouseButton(evt.button.button);

        switch (evt.type)
        {
            case SDL_EventType.SDL_MOUSEMOTION when MouseMove is not null:
                MouseMove(evt.motion.x, evt.motion.y);
                break;
            case SDL_EventType.SDL_MOUSEBUTTONDOWN when MouseDown is not null:
                MouseDown(button);
                break;
            case SDL_EventType.SDL_MOUSEBUTTONUP when MouseUp is not null:
                MouseUp(button);
                break;
        }

        if (evt.type == SDL_EventType.SDL_MOUSEWHEEL)
        {
            _mWheelValue += evt.wheel.y * 120;
        }
    }

    private static MouseButton TranslateBooPlatformMouseButton(byte button)
    {
        return button switch
        {
            1 => MouseButton.Left,
            2 => MouseButton.Middle,
            3 => MouseButton.Right,
            _ => MouseButton.None
        };
    }
}