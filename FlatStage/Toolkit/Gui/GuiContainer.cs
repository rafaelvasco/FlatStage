using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiContainer : GuiControl
{
    public GuiContainer(Gui gui, GuiContainer? parent = null) : base(gui, parent)
    {
    }

    public override Size SizeHint => new(200, 200);

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
    }

    internal override void ProcessMouseEvent(GuiMouseState mouseState)
    {
    }
}
