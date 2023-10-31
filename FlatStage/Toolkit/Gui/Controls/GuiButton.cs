using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GuiButton : GuiControl
{
    internal static readonly int STypeId;

    static GuiButton()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

    public event MouseButtonEventHandler? OnClick;

    public string Label { get; set; } = "BUTTON";

    public GuiButton(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiButtonDef buttonDef)
        {
            Label = buttonDef.Label;
        }
    }

    protected override bool ProcessMouseButton(GuiMouseState mouseState)
    {
        if (mouseState.LastMouseButton == Input.MouseButton.Left && !mouseState.MouseButtonDown)
        {
            OnClick?.Invoke(mouseState.LastMouseButton);
        }

        return base.ProcessMouseButton(mouseState);
    }

    public override Size SizeHint => new(100, 40);



    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawButton(canvas, this);
    }
}
