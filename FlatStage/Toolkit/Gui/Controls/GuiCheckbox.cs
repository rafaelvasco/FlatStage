using FlatStage.Graphics;
using FlatStage.Input;

namespace FlatStage.Toolkit;
public class GuiCheckbox : GuiControl
{
    public bool Checked { get; set; }

    public GuiCheckbox(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.LastMouseButton == MouseButton.Left && mouseState.MouseButtonDown == false)
        {
            Checked = !Checked;
        }

        return true;
    }

    protected override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawCheckbox(canvas, this);
    }

    public override Size SizeHint => new(100, 40);
}
