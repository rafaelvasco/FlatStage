using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

[JsonDerivedType(typeof(SpriteDef), typeDiscriminator: "sprite")]
[JsonDerivedType(typeof(TextDef), typeDiscriminator: "text")]
[JsonDerivedType(typeof(LayoutDef), typeDiscriminator: "layout")]
[JsonDerivedType(typeof(GuiDef), typeDiscriminator: "gui")]
[JsonDerivedType(typeof(GuiButtonDef), typeDiscriminator: "button")]
public class GameObjectDef : IDefinitionData
{
    [JsonRequired]
    public required string Id { get; init; }

    public string? Name { get; init; }

    public float X { get; init; }

    public float Y { get; init; }

    public float Width { get; init; }

    public float Height { get; init; }

    public Vec2 Origin { get; init; } = Vec2.Half;

    public float Depth { get; init; }

    public bool VisibleOnStart { get; init; } = true;

    public Color Tint { get; init; } = Color.White;

    public float Rotation { get; init; }

    public GameObjectDef[]? Children { get; init; }

    public virtual bool IsValid()
    {
        return !string.IsNullOrEmpty(Id);
    }
}
