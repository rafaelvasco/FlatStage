using FlatStage.Platform;

namespace FlatStage.Input;

public static class Control
{
    public static event FileDropEvent? OnFileDrop;

    public static Keyboard Keyboard { get; private set; } = null!;

    public static Mouse Mouse { get; private set; } = null!;

    public static Gamepad Gamepad { get; private set; } = null!;

    internal static void Init(StageSettings settings)
    {
        Keyboard = new Keyboard();
        Mouse = new Mouse();
        Gamepad = new Gamepad();

        PlatformContext.OnFileDrop = ProcessFileDrop;

        MapDefaultControls();
    }

    private static void MapDefaultControls()
    {
    }

    internal static void Update()
    {
        Keyboard.UpdateState();
        Mouse.UpdateState();
        Gamepad.UpdateState();
    }

    private static void ProcessFileDrop(FileDropEventArgs fileDropArgs) => OnFileDrop?.Invoke(fileDropArgs);
}