using FlatStage.Graphics;
using FlatStage.Input;

namespace FlatStage.Toolkit;
public class GuiCheckbox : GuiControl
{
    internal static readonly int STypeId;

    static GuiCheckbox()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public bool Checked { get; set; }

    public override GuiControlState State
    {
        get
        {
            if (Checked)
            {
                return GuiControlState.Checked;
            }

            if (Focused)
            {
                return GuiControlState.Focused;
            }

            if (!Hovered && !Active)
            {
                return GuiControlState.Idle;
            }

            if (Hovered && !Active)
            {
                return GuiControlState.Hover;
            }

            return GuiControlState.Active;
        }
    }

    public GuiCheckbox(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiCheckBoxDef checkDef)
        {
            Checked = checkDef.Checked;
        }
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.LastMouseButton == MouseButton.Left && mouseState.MouseButtonDown == false)
        {
            Checked = !Checked;
        }

        return true;
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawCheckbox(canvas, this);
    }

    public override Size SizeHint => new(100, 40);
}
