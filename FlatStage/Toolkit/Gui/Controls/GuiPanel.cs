using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiPanel : GuiControl
{
    public GuiPanel(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
        CanGetFocus = false;
    }

    public override Size SizeHint => new(200, 200);

    protected override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawPanel(canvas, this);
    }
}
