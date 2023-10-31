using FlatStage.Graphics;
using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

[JsonDerivedType(typeof(SpriteDef), typeDiscriminator: "sprite")]
[JsonDerivedType(typeof(TextDef), typeDiscriminator: "text")]
public class GraphicDef : IDefinitionData
{
    public required string Name { get; init; }

    public float X { get; init; }

    public float Y { get; init; }

    public bool DebugDraw { get; init; } = false;

    public Vec2 Origin { get; init; } = Vec2.Half;

    public float Rotation { get; init; }

    public float Depth { get; init; }

    public bool VisibleAtStart { get; init; } = true;

    public Color ColorTint { get; init; } = Color.White;

    public float ScaleX { get; init; } = 1.0f;

    public float ScaleY { get; init; } = 1.0f;

    public float Width { get; init; } = -1.0f;

    public float Height { get; init; } = -1.0f;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name);
    }
}
