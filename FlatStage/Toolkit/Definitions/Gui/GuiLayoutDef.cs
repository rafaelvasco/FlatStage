namespace FlatStage.Toolkit;
public class GuiLayoutDef : GuiContainerDef
{
    public GuiLayoutMode LayoutMode { get; init; } = GuiLayoutMode.AlignStart;

    public GuiLayoutDirection Direction { get; init; } = GuiLayoutDirection.Vertical;

    public int Spacing { get; init; }
}
