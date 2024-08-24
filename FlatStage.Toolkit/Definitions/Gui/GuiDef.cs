using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class RuleDef
{
    [JsonRequired]
    public required string Target { get; init; }

    [JsonRequired]
    public required (string, object)[] RuleValues { get; init; }
}

public class StyleDef
{
    [JsonRequired]
    public required RuleDef[] Rules { get; init; }
}

public class GuiDef : GameObjectDef
{
    public StyleDef? Style { get; init; }

}
