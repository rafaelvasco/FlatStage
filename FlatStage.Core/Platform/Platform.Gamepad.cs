using System.Runtime.CompilerServices;
using SDL;
using static SDL.SDL3;

namespace FlatStage;

internal static unsafe partial class Platform
{
    public static int ConnectedGamePads { get; set; }

    private static readonly nint[] GamepadDevices = new nint[Gamepad.MaxCount];
    private static readonly Dictionary<int, int> GamepadInstances = new();
    private static readonly string[] GamepadGuids = GenStringArray();
    private static readonly string[] GamepadLightBars = GenStringArray();
    private static readonly GamePadState[] GamepadStates = new GamePadState[Gamepad.MaxCount];
    private static readonly GamePadCapabilities[] GamepadCaps = new GamePadCapabilities[Gamepad.MaxCount];


    public static GamePadCapabilities GetGamePadCapabilities(int index)
    {
        return GamepadDevices[index] == IntPtr.Zero ? new GamePadCapabilities() : GamepadCaps[index];
    }

    public static string GetGamePadName(int index)
    {
        index = MathUtils.Clamp(index, 0, GamepadDevices.Length - 1);

        return SDL_GetGamepadName((SDL_Gamepad*)GamepadDevices[index]) ?? "Unknown";
    }

    public static GamePadState GetGamePadState(int index, GamePadDeadZone deadZoneMode)
    {
        var device = (SDL_Gamepad*)GamepadDevices[index];
        if (device == null)
        {
            return new GamePadState();
        }

        GamePadButtons gamepadButtonState = 0;

        // Sticks
        var stickLeft = new Vec2(
            SDL_GetGamepadAxis(
                device,
                SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTX
            ) / 32767.0f,
            SDL_GetGamepadAxis(
                device,
                SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTY
            ) / -32767.0f
        );
        var stickRight = new Vec2(
            SDL_GetGamepadAxis(
                device,
                SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTX
            ) / 32767.0f,
            SDL_GetGamepadAxis(
                device,
                SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTY
            ) / -32767.0f
        );

        gamepadButtonState |= ConvertStickValuesToButtons(
            stickLeft,
            GamePadButtons.LeftThumbstickLeft,
            GamePadButtons.LeftThumbstickRight,
            GamePadButtons.LeftThumbstickUp,
            GamePadButtons.LeftThumbstickDown,
            Gamepad.LeftDeadZone
        );
        gamepadButtonState |= ConvertStickValuesToButtons(
            stickRight,
            GamePadButtons.RightThumbstickLeft,
            GamePadButtons.RightThumbstickRight,
            GamePadButtons.RightThumbstickUp,
            GamePadButtons.RightThumbstickDown,
            Gamepad.RightDeadZone
        );

        // Triggers
        float triggerLeft = SDL_GetGamepadAxis(
            device,
            SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFT_TRIGGER
        ) / 32767.0f;
        float triggerRight = SDL_GetGamepadAxis(
            device,
            SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHT_TRIGGER
        ) / 32767.0f;
        if (triggerLeft > Gamepad.TriggerThreshold)
        {
            gamepadButtonState |= GamePadButtons.LeftTrigger;
        }

        if (triggerRight > Gamepad.TriggerThreshold)
        {
            gamepadButtonState |= GamePadButtons.RightTrigger;
        }

        // Buttons
        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH) != 0)
        {
            gamepadButtonState |= GamePadButtons.South;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST) != 0)
        {
            gamepadButtonState |= GamePadButtons.East;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST) != 0)
        {
            gamepadButtonState |= GamePadButtons.West;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_NORTH) != 0)
        {
            gamepadButtonState |= GamePadButtons.North;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_BACK) != 0)
        {
            gamepadButtonState |= GamePadButtons.Back;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_GUIDE) != 0)
        {
            gamepadButtonState |= GamePadButtons.BigButton;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_START) != 0)
        {
            gamepadButtonState |= GamePadButtons.Start;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_LEFT_STICK) != 0)
        {
            gamepadButtonState |= GamePadButtons.LeftStick;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_STICK) != 0)
        {
            gamepadButtonState |= GamePadButtons.RightStick;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_LEFT_SHOULDER) != 0)
        {
            gamepadButtonState |= GamePadButtons.LeftShoulder;
        }


        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_SHOULDER) != 0)
        {
            gamepadButtonState |= GamePadButtons.RightShoulder;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP) != 0)
        {
            gamepadButtonState |= GamePadButtons.DPadUp;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN) != 0)
        {
            gamepadButtonState |= GamePadButtons.DPadDown;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT) != 0)
        {
            gamepadButtonState |= GamePadButtons.DPadLeft;
        }

        if (SDL_GetGamepadButton(device, SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT) != 0)
        {
            gamepadButtonState |= GamePadButtons.DPadRight;
        }

        // Build the GamePadState, increment PacketNumber if state changed.

        GamePadState gcBuiltState = new GamePadState(
            new GamePadThumbSticks(stickLeft, stickRight, deadZoneMode),
            new GamePadTriggers(triggerLeft, triggerRight, deadZoneMode),
            gamepadButtonState
        )
        {
            IsConnected = true,
            PacketNumber = GamepadStates[index].PacketNumber
        };
        if (gcBuiltState != GamepadStates[index])
        {
            gcBuiltState.PacketNumber += 1;
            GamepadStates[index] = gcBuiltState;
        }

        return gcBuiltState;
    }

    public static bool SetGamePadVibration(int index, float leftMotor, float rightMotor)
    {
        var device = (SDL_Gamepad*)GamepadDevices[index];
        if (device == null)
        {
            return false;
        }

        return SDL_RumbleGamepad(
            device,
            (ushort)(MathUtils.Clamp(leftMotor, 0.0f, 1.0f) * 0xFFFF),
            (ushort)(MathUtils.Clamp(rightMotor, 0.0f, 1.0f) * 0xFFFF),
            0
        ) == 0;
    }

    public static string GetGamePadGuid(int gamepadIndex)
    {
        return GamepadGuids[gamepadIndex];
    }

    public static void SetGamePadLightBar(int index, Color color)
    {
        if (string.IsNullOrEmpty(GamepadLightBars[index]))
        {
            return;
        }

        string baseDir = GamepadLightBars[index];
        try
        {
            File.WriteAllText(baseDir + "red/brightness", color.R.ToString());
            File.WriteAllText(baseDir + "green/brightness", color.G.ToString());
            File.WriteAllText(baseDir + "blue/brightness", color.B.ToString());
        }
        catch
        {
            // If something went wrong, assume the worst and just remove it.
            GamepadLightBars[index] = string.Empty;
        }
    }

    public static void SetGamePadMappingsFile(string fileContent)
    {
        SDL_AddGamepadMapping(fileContent);
    }

    public static void PreLookForGamepads()
    {
        var evt = stackalloc SDL_Event[1];
        SDL_PumpEvents();
        while (SDL_PeepEvents(
                   (SDL_Event*)Unsafe.AsPointer(ref evt[0]),
                   1,
                   SDL_EventAction.SDL_GETEVENT,
                   (uint)SDL_EventType.SDL_EVENT_GAMEPAD_ADDED,
                   (uint)SDL_EventType.SDL_EVENT_GAMEPAD_ADDED
               ) == 1)
        {
            AddGamePadInstance((int)evt[0].gdevice.which);
        }
    }

    private static void AddGamePadInstance(int gamepadId)
    {
        if (ConnectedGamePads == Gamepad.MaxCount)
        {
            return;
        }

        SDL_ClearError();

        int which = ConnectedGamePads++;

        // Open the device!
        var gamepadDevice = GamepadDevices[which] = (IntPtr)SDL_OpenGamepad((SDL_JoystickID)gamepadId);
        var gamepadDevicePtr = (SDL_Gamepad*)gamepadDevice;

        SDL_JoystickID joystickId = SDL_GetGamepadID(gamepadDevicePtr);

        GamepadInstances.Add(gamepadId, which);

        // Start with a fresh state.
        GamepadStates[which] = new GamePadState
        {
            IsConnected = true
        };

        // Initialize the haptics for the joystick, if applicable.
        bool hasRumble = SDL_RumbleGamepad(
            gamepadDevicePtr,
            0,
            0,
            0
        ) == 0;

        var gamepadBindings = SDL_GetGamepadBindings(gamepadDevicePtr);
        var caps = new GamePadCapabilities
        {
            HasRumble = hasRumble
        };

        if (gamepadBindings != null)
        {
            for (int i = 0; i < gamepadBindings.Count; ++i)
            {
                var binding = gamepadBindings[i];
                switch (binding.output.button)
                {
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_SOUTH:
                        caps.HasSouthButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_NORTH:
                        caps.HasNorthButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_EAST:
                        caps.HasEastButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_WEST:
                        caps.HasWestButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_BACK:
                        caps.HasBackButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_GUIDE:
                        caps.HasBigButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_START:
                        caps.HasStartButton = binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_LEFT_STICK:
                        caps.HasLeftStickButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_STICK:
                        caps.HasRightStickButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_LEFT_SHOULDER:
                         caps.HasLeftShoulderButton =
                             binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_RIGHT_SHOULDER:
                        caps.HasRightShoulderButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_UP:
                        caps.HasDPadUpButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_DOWN:
                        caps.HasDPadDownButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_LEFT:
                        caps.HasDPadLeftButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadButton.SDL_GAMEPAD_BUTTON_DPAD_RIGHT:
                        caps.HasDPadRightButton =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;

                }

                switch (binding.output.axis.axis)
                {
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTX:
                        caps.HasLeftXThumbStick =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFTY:
                        caps.HasLeftYThumbStick =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTX:
                        caps.HasRightXThumbStick =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHTY:
                        caps.HasRightYThumbStick =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_LEFT_TRIGGER:
                        caps.HasLeftTrigger =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                    case SDL_GamepadAxis.SDL_GAMEPAD_AXIS_RIGHT_TRIGGER:
                        caps.HasRightTrigger =
                            binding.output_type != SDL_GamepadBindingType.SDL_GAMEPAD_BINDTYPE_NONE;
                        break;
                }
            }
        }

        GamepadCaps[which] = caps;

        ushort vendor = SDL_GetGamepadVendorForID(joystickId);
        ushort product = SDL_GetGamepadProductForID(joystickId);
        if (vendor == 0x00 && product == 0x00)
        {
            GamepadGuids[which] = "xinput";
        }
        else
        {
            GamepadGuids[which] = $"{vendor & 0xFF:x2}{vendor >> 8:x2}{product & 0xFF:x2}{product >> 8:x2}";
        }

        // Initialize light bar
        if (PlatformId is PlatformId.LinuxX11 or PlatformId.LinuxWayland  &&
            (GamepadGuids[which].Equals("4c05c405") ||
             GamepadGuids[which].Equals("4c05cc09")))
        {
            // Get all the individual PS4 LED instances
            var ledList = new List<string>();
            string[] dirs = Directory.GetDirectories("/sys/class/leds/");
            foreach (string dir in dirs)
            {
                if (dir.EndsWith("blue") &&
                    (dir.Contains("054C:05C4") ||
                     dir.Contains("054C:09CC")))
                {
                    ledList.Add(dir[..(dir.LastIndexOf(':') + 1)]);
                }
            }

            // Find how many of these are already in use
            int numLights = 0;
            for (int i = 0; i < GamepadLightBars.Length; i += 1)
            {
                if (!string.IsNullOrEmpty(GamepadLightBars[i]))
                {
                    numLights += 1;
                }
            }

            // If all are not already in use, use the first unused light
            if (numLights < ledList.Count)
            {
                GamepadLightBars[which] = ledList[numLights];
            }
        }

        Console.WriteLine($"{GamepadCaps[which].GamePadType} Added: {GetGamePadName(which)}");
    }

    private static void RemoveGamePadInstance(int gamepadId)
    {
        Console.WriteLine("GamePad Removed");
        if (!GamepadInstances.Remove(gamepadId, out int gamepadIndex))
        {
            return;
        }

        SDL_CloseGamepad((SDL_Gamepad*)GamepadDevices[gamepadIndex]);

        GamepadDevices[gamepadIndex] = IntPtr.Zero;
        GamepadStates[gamepadIndex] = new GamePadState();
        GamepadGuids[gamepadIndex] = string.Empty;

        SDL_ClearError();

        ConnectedGamePads--;
    }

    private static GamePadButtons ConvertStickValuesToButtons(Vec2 stick, GamePadButtons left, GamePadButtons right,
        GamePadButtons up, GamePadButtons down, float deadZoneSize)
    {
        GamePadButtons b = 0;

        var (x, y) = stick;
        if (x > deadZoneSize)
        {
            b |= right;
        }

        if (x < -deadZoneSize)
        {
            b |= left;
        }

        if (y > deadZoneSize)
        {
            b |= up;
        }

        if (y < -deadZoneSize)
        {
            b |= down;
        }

        return b;
    }

    private static string[] GenStringArray()
    {
        string[] result = new string[Gamepad.MaxCount];
        for (int i = 0; i < result.Length; i += 1)
        {
            result[i] = string.Empty;
        }

        return result;
    }

    private static void InitGamePad()
    {
        string? hint = SDL_GetHint(SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS);
        if (string.IsNullOrEmpty(hint))
        {
            SDL_SetHint(
                SDL_HINT_JOYSTICK_ALLOW_BACKGROUND_EVENTS,
                "1"
            );
        }

    }

    private static void ProcessGamePadEvent(SDL_Event evt)
    {
        switch (evt.type)
        {
            case (uint)SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
                AddGamePadInstance((int)evt.gdevice.which);
                break;
            case (uint)SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
                RemoveGamePadInstance((int)evt.gdevice.which);
                break;
        }
    }
}
