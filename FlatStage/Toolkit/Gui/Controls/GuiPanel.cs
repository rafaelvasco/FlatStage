using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiPanel : GuiContainer
{
    internal static new readonly int STypeId;

    static GuiPanel()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public GuiPanel(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
        CanGetFocus = false;
    }

    public override Size SizeHint => new(200, 200);

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawPanel(canvas, this);

        base.Draw(canvas, skin);
    }
}
