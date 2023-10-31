using FlatStage.Graphics;

namespace FlatStage.Toolkit;

public class GuiContainer : GuiControl
{

    internal static readonly int STypeId;

    static GuiContainer()
    {
        STypeId = ++SBTypeId;
    }

    internal override int TypeId => STypeId;

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

    public FastList<GuiControl> Children => _children;

    public GuiContainer(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
    }

    internal override void InitFromDefinition(GuiControlDef definition)
    {
        base.InitFromDefinition(definition);

        if (definition is GuiContainerDef containerDef)
        {
            Padding = containerDef.Padding;

            if (containerDef.Children != null)
            {
                for (var i = 0; i < containerDef.Children.Length; ++i)
                {
                    Gui.CreateFromDefinition(Gui, containerDef.Children[i], this);
                }
            }
        }
    }

    public override Size SizeHint => new(200, 200);

    internal void Add(GuiControl child)
    {
        if (!_children.Contains(child))
        {
            _children.Add(child);
            Gui.InvalidateLayout();
        }
    }

    internal void Remove(GuiControl child)
    {
        _children.Remove(child);
        Gui.InvalidateLayout();
    }

    private void PreProcessLayout()
    {
        var children = Children.ReadOnlySpan;

        foreach (var child in children)
        {
            if (child.Width >= Width)
            {
                child.Width = Width - (2 * Padding);
            }

            if (child.Height >= Height)
            {
                child.Height = Height - (2 * Padding);
            }

            if (child.X < Padding)
            {
                child.X = Padding;
            }

            if (child.X + child.Width > Width - Padding)
            {
                child.X = Width - Padding - child.Width;
            }

            if (child.Y < Padding)
            {
                child.Y = Padding;
            }

            if (child.Y + child.Height > Height - Padding)
            {
                child.Y = Height - Padding - child.Height;
            }
        }
    }

    internal virtual void ProcessLayout()
    {
        PreProcessLayout();

        if (Children.Count == 0)
        {
            return;
        }

        ProcessAnchoredChildren();
    }

    private void ProcessAnchoredChildren()
    {
        var children = Children.ReadOnlySpan;

        foreach (var child in children)
        {
            if (child.Anchor == GuiAnchoring.None)
            {
                continue;
            }

            switch (child.Anchor)
            {

                case GuiAnchoring.Left:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Height = this.Height - (2 * Padding);

                    break;

                case GuiAnchoring.Right:

                    child.X = this.Width - child.Width - Padding;
                    child.Y = Padding;
                    child.Height = this.Height - (2 * Padding);

                    break;

                case GuiAnchoring.Top:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Width = this.Width - (2 * Padding);

                    break;

                case GuiAnchoring.Bottom:

                    child.X = Padding;
                    child.Y = this.Height - child.Height - Padding;
                    child.Width = this.Width - (2 * Padding);

                    break;

                case GuiAnchoring.Fill:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Width = this.Width - (2 * Padding);
                    child.Height = this.Height - (2 * Padding);

                    break;

                case GuiAnchoring.Center:

                    child.X = (this.Width / 2) - (child.Width / 2);
                    child.Y = (this.Height / 2) - (child.Height / 2);

                    break;
            }
        }
    }

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        if (Hidden)
        {
            return;
        }

        for (int i = 0; i < _children.Count; ++i)
        {
            var child = _children[i];

            if (!child.Hidden)
            {
                child.Draw(canvas, skin);
            }
        }
    }

    protected override void OnResize(int width, int height)
    {
        Gui.InvalidateLayout();
    }

    private readonly FastList<GuiControl> _children = new();
    private int _padding = 0;
}
