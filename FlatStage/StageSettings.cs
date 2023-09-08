namespace FlatStage;

public class StageSettings : IDefinitionData
{
    public string? AppTitle { get; set; }

    public int CanvasWidth { get; set; }

    public int CanvasHeight { get; set; }

    public bool Fullscreen { get; set; }

    public override string ToString() => $"[Title: {AppTitle}, Canvas Size: [{CanvasWidth},{CanvasHeight}]]";

    public StageSettings()
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
