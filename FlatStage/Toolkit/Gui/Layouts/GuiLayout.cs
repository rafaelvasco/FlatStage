namespace FlatStage.Toolkit;
public abstract class GuiLayout : GuiContainer
{

    public int Spacing
    {
        get => _spacing;
        set
        {
            if (_spacing != value)
            {
                _spacing = value;
                Gui.InvalidateLayout();
            }
        }
    }

    public GuiLayoutMode LayoutMode
    {
        get => _layoutMode;
        set
        {
            if (_layoutMode != value)
            {
                _layoutMode = value;
                Gui.InvalidateLayout();
            }
        }
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiLayoutDef layoutDef)
        {
            Spacing = layoutDef.Spacing;
            LayoutMode = layoutDef.LayoutMode;
        }

    }

    private int _spacing = 0;
    private GuiLayoutMode _layoutMode = GuiLayoutMode.AlignStart;

    protected GuiLayout(string id, Gui gui, GuiContainer? parent) : base(id, gui, parent)
    {
    }
}
