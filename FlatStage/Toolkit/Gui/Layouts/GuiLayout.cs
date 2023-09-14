using System.Collections.Generic;

namespace FlatStage.Toolkit;

public enum GuiLayoutMode
{
    AlignStart,
    AlignCenter,
    AlignEnd,
    Fill
}

public abstract class GuiLayout : GuiControl
{
    public int Padding
    {
        get => _padding;
        set
        {
            if (_padding != value)
            {
                _padding = value;
                Gui.InvalidateLayout();
            }
        }
    }

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

    public List<GuiControl> Children => _children;

    public GuiLayout(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
    }

    public override Size SizeHint => new(200, 200);

    protected override void OnChildAdded(GuiControl control)
    {
        _children.Add(control);
        Gui.InvalidateLayout();
    }

    protected override void OnResize(int width, int height)
    {
        Gui.InvalidateLayout();
    }

    internal abstract void ProcessLayout();

    private GuiLayoutMode _layoutMode = GuiLayoutMode.AlignStart;
    private int _padding = 0;
    private int _spacing = 0;

    private readonly List<GuiControl> _children = new();

}
