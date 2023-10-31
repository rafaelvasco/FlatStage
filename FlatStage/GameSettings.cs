using FlatStage.Graphics;

namespace FlatStage;

public class GameSettings : IDefinitionData
{
    public required string AppTitle { get; init; }

    public int CanvasWidth { get; init; }

    public int CanvasHeight { get; init; }

    public CanvasStretchMode CanvasStretchMode { get; init; } = CanvasStretchMode.LetterBox;

    public bool Fullscreen { get; init; }

    public GameSettings()
    {
        AppTitle = "FlatStage";
        CanvasWidth = 800;
        CanvasHeight = 600;
        Fullscreen = false;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(AppTitle) && CanvasWidth > 0 && CanvasHeight > 0;
    }
}
