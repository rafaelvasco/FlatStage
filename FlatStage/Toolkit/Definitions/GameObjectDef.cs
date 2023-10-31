using System.Linq;

namespace FlatStage.Toolkit;
public class GameObjectDef : IDefinitionData
{
    public required string Name { get; init; }

    public float X { get; init; }

    public float Y { get; init; }

    public float Width { get; init; }

    public float Height { get; init; }

    public float Rotation { get; init; }

    public bool VisibleAtStart { get; init; } = true;

    public GraphicDef[]? Graphics { get; init; }

    public GameObjectDef[]? Children { get; init; }

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name) && (Graphics == null || Graphics.All(gr => gr.IsValid())) && (Children == null || Children.All(ch => ch.IsValid()));
    }
}
