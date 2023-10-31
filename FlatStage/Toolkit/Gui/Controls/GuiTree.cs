namespace FlatStage.Toolkit;
public class GuiTree : GuiControl
{
    public GuiTree(string id, Gui gui, GuiContainer? parent) : base(id, gui, parent)
    {
    }

    public override Size SizeHint => throw new System.NotImplementedException();

    internal override int TypeId => throw new System.NotImplementedException();
}
