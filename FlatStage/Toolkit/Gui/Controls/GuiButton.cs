using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiButton : GuiControl
{
    public string Label { get; set; } = "BUTTON";

    public GuiButton(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
    }

    public override Size SizeHint => new(100, 40);

    protected override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawButton(canvas, this);
    }
}
