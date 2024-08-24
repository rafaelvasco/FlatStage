namespace FlatStage;

public class GameSettings : IDefinitionData
{
    public required string AppTitle { get; init; } = "FlatStage";

    public int CanvasWidth { get; init; } = 800;

    public int CanvasHeight { get; init; } = 600;

    public Color CanvasBackColor { get; init; } = Color.CornflowerBlue;

    public CanvasStretchMode CanvasStretchMode { get; init; } = CanvasStretchMode.LetterBox;

    public bool Fullscreen { get; init; } = false;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(AppTitle) && CanvasWidth > 0 && CanvasHeight > 0;
    }
}
