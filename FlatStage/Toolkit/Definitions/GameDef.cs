using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;
public class GameDef : IDefinitionData
{
    [JsonRequired]
    public required GameSettings GameSettings { get; init; }

    [JsonRequired]
    public required GameObjectDef[] Scenes { get; init; }

    [JsonRequired]
    public required string StartingScene { get; init; }

    public GuiDef? Gui { get; init; }

    public string? PreloadPak { get; init; }

    public bool IsValid()
    {
        var valid = GameSettings.IsValid() && Scenes.Length > 0 && !string.IsNullOrEmpty(StartingScene);

        if (Gui != null)
        {
            valid &= Gui.IsValid();
        }

        for (int i = 0; i < Scenes.Length; ++i)
        {
            valid &= Scenes[i].IsValid();
        }

        return valid;
    }
}
