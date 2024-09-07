using SDL;
using static SDL.SDL3;

namespace FlatStage;

internal static unsafe partial class Platform
{
    public static Action<MouseButton>? MouseUp;
    public static Action<MouseButton>? MouseDown;
    public static Action<int, int>? MouseMove;

    private static int _mWheelValue;

    public static MouseState GetMouseState()
    {
        SDL_MouseButtonFlags flags;
        float x, y;
        if (GetRelativeMouseMode())
        {
            flags = SDL_GetRelativeMouseState(&x, &y);
        }
        else
        {
            flags = SDL_GetGlobalMouseState(&x, &y);
            int windowX;
            int windowY;
            SDL_GetWindowPosition(_windowHandle, &windowX, &windowY);
            x -= windowX;
            y -= windowY;
        }

        var left = (flags & (SDL_MouseButtonFlags)SDL_BUTTON_LMASK) > 0;
        var middle = (flags & (SDL_MouseButtonFlags)SDL_BUTTON_MMASK) > 0;
        var right = (flags & (SDL_MouseButtonFlags)SDL_BUTTON_RMASK) > 0;

        return new MouseState((int)x, (int)y, _mWheelValue, left, middle, right);
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

    private static void ProcessMouseEvent(SDL_Event evt)
    {
        var button = TranslateBooPlatformMouseButton(evt.button.button);

        switch (evt.type)
        {
            case (uint)SDL_EventType.SDL_EVENT_MOUSE_MOTION when MouseMove is not null:
                MouseMove((int)evt.motion.x, (int)evt.motion.y);
                break;
            case (uint)SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN when MouseDown is not null:
                MouseDown(button);
                break;
            case (uint)SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP when MouseUp is not null:
                MouseUp(button);
                break;
        }

        if (evt.type == (uint)SDL_EventType.SDL_EVENT_MOUSE_WHEEL)
        {
            _mWheelValue += (int)(evt.wheel.y * 120);
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
