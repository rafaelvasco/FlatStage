namespace FlatStage.Toolkit;

public class LayoutDef : GameObjectDef
{
    public int Spacing { get; init; }

    public int Padding { get; init; }

    public required LayoutMode Mode { get; init; }

    public LayoutAlign Align { get; init; } = LayoutAlign.AlignStart;
}
