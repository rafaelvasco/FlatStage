using System;
using System.Collections.Generic;

namespace FlatStage;

public enum InputMapCheckMode
{
    Pressed,
    Released,
    Down
}

public static class Input
{
    private delegate bool InputMapGamepadDelegate(InputMapCheckMode checkMode, GamePadButtons button);

    private delegate bool InputMapKeyboardDelegate(InputMapCheckMode checkMode, Key key);

    private delegate bool InputMapMouseDelegate(InputMapCheckMode checkMode, MouseButton button);

    public static event FileDropEvent? OnFileDrop;

    public static Keyboard Keyboard { get; private set; } = null!;

    public static Mouse Mouse { get; private set; } = null!;

    public static Gamepad Gamepad { get; private set; } = null!;

    public static void Map(string controlName, GamePadButtons button, GamePadIndex index = GamePadIndex.One)
    {
        _controlButtonMappings[controlName] = (int)button;

        if (!_inputMappingsGamepad.TryGetValue((int)button, out _))
        {
            _inputMappingsGamepad[(int)button] = (mode, buttons) =>
            {
                return mode switch
                {
                    InputMapCheckMode.Pressed => Gamepad.ButtonPressed(buttons, index),
                    InputMapCheckMode.Released => Gamepad.ButtonReleased(buttons, index),
                    InputMapCheckMode.Down => Gamepad.ButtonDown(buttons, index),
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
                };
            };
        }
    }

    public static void Map(string controlName, Key key)
    {
        _controlButtonMappings[controlName] = (int)key;

        if (!_inputMappingsKeyboard.TryGetValue((int)key, out _))
        {
            _inputMappingsKeyboard[(int)key] = (mode, keys) =>
            {
                return mode switch
                {
                    InputMapCheckMode.Pressed => Keyboard.KeyPressed(keys),
                    InputMapCheckMode.Released => Keyboard.KeyReleased(keys),
                    InputMapCheckMode.Down => Keyboard.KeyDown(keys),
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
                };
            };
        }
    }

    public static void Map(string controlName, MouseButton button)
    {
        _controlButtonMappings[controlName] = (int)button;

        if (!_inputMappingsMouse.TryGetValue((int)button, out _))
        {
            _inputMappingsMouse[(int)button] = (mode, buttons) =>
            {
                return mode switch
                {
                    InputMapCheckMode.Pressed => Mouse.ButtonPressed(buttons),
                    InputMapCheckMode.Released => Mouse.ButtonReleased(buttons),
                    InputMapCheckMode.Down => Mouse.ButtonDown(buttons),
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
                };
            };
        }
    }

    public static bool CheckControl(string key, InputMapCheckMode check = InputMapCheckMode.Down)
    {
        if (!_controlButtonMappings.TryGetValue(key, out var checkButton))
            throw new ApplicationException("Control key not found");

        if (_inputMappingsGamepad.TryGetValue(checkButton, out var checkerGp))
        {
            return checkerGp(check, (GamePadButtons)checkButton);
        }

        if (_inputMappingsKeyboard.TryGetValue(checkButton, out var checkerKb))
        {
            return checkerKb(check, (Key)checkButton);
        }

        if (_inputMappingsMouse.TryGetValue(checkButton, out var checkerMs))
        {
            return checkerMs(check, (MouseButton)checkButton);
        }

        throw new ApplicationException("Control key not found");
    }

    internal static void Init(StageSettings settings)
    {
        _controlButtonMappings = new Dictionary<string, int>();

        _inputMappingsGamepad = new Dictionary<int, InputMapGamepadDelegate>();

        _inputMappingsKeyboard = new Dictionary<int, InputMapKeyboardDelegate>();

        _inputMappingsMouse = new Dictionary<int, InputMapMouseDelegate>();

        Keyboard = new Keyboard();
        Mouse = new Mouse();
        Gamepad = new Gamepad();

        Platform.OnFileDrop = ProcessFileDrop;

        MapDefaultControls();
    }

    private static void MapDefaultControls()
    {
        Map("InputUp", GamePadButtons.DPadUp);
        Map("InputUp", Key.Up);
        Map("InputDown", GamePadButtons.DPadDown);
        Map("InputDown", Key.Down);
        Map("InputLeft", GamePadButtons.DPadLeft);
        Map("InputLeft", Key.Left);
        Map("InputRight", GamePadButtons.DPadRight);
        Map("InputRight", Key.Right);
        Map("ToggleFullscreen", Key.F11);
        Map("Escape", Key.Escape);
    }

    internal static void Update()
    {
        Keyboard.UpdateState();
        Mouse.UpdateState();
        Gamepad.UpdateState();
    }

    private static void ProcessFileDrop(FileDropEventArgs fileDropArgs) => OnFileDrop?.Invoke(fileDropArgs);

    private static Dictionary<int, InputMapGamepadDelegate> _inputMappingsGamepad = null!;
    private static Dictionary<int, InputMapKeyboardDelegate> _inputMappingsKeyboard = null!;
    private static Dictionary<int, InputMapMouseDelegate> _inputMappingsMouse = null!;

    private static Dictionary<string, int> _controlButtonMappings = null!;

}