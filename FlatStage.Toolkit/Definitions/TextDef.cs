using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class TextDef : GameObjectDef
{
    [JsonRequired]
    public required string Label { get; init; }

    public override bool IsValid()
    {
       return base.IsValid() && !string.IsNullOrEmpty(Label);
    }
}
