using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class SpriteDef : GraphicDef
{
    public required string Texture { get; init; }

    public Rect? Region { get; init; }

    public FlipMode FlipMode { get; init; } = FlipMode.None;
}
