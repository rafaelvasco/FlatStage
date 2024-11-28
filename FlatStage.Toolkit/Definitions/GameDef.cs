using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class GameDef : IDefinitionData
{
    [JsonRequired]
    public required GameSettings GameSettings { get; init; }

    [JsonRequired]
    public required string[] GameObjectsPaths { get; init; }

    [JsonRequired]
    public required string StartingGameObject { get; init; }

    [JsonIgnore] public GameObjectDef[] GameObjects { get; internal set; } = null!;

    public bool IsValid()
    {
        return GameObjectsPaths.Length > 0 && GameSettings.IsValid();
    }
}
