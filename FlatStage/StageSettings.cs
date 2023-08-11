namespace FlatStage;

public class StageSettings : IDefinitionData
{
    public string? AppTitle { get; set; }

    public int WindowWidth { get; set; }

    public int WindowHeight { get; set; }

    public override string ToString() => $"[Title: {AppTitle}, Display Size: [{WindowWidth},{WindowHeight}]]";

    public StageSettings()
    {
        AppTitle = "FlatStage";
        WindowWidth = 800;
        WindowHeight = 600;
    }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(AppTitle) && WindowWidth > 0 && WindowHeight > 0;
    }
}