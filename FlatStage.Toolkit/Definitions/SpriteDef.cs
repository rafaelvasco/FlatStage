using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class SpriteDef : GameObjectDef
{
    [JsonRequired]
    public required string Texture { get; init; }

    public Rect? Region { get; init; }

    public FlipMode FlipMode { get; init; } = FlipMode.None;

    public override bool IsValid()
    {
        return base.IsValid() && !string.IsNullOrEmpty(Texture);
    }
}
