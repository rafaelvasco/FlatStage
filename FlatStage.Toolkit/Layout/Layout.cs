namespace FlatStage.Toolkit;

public enum LayoutAlign
{
    AlignStart,
    AlignCenter,
    AlignEnd,
    Fill
}

public enum LayoutAnchor
{
    None,
    Top,
    Fill,
    Center,
    Bottom,
    Left,
    Right
}

public enum LayoutMode
{
    Horizontal,
    Vertical
}

public abstract class Layout : GuiControl
{
    public LayoutAlign Align
    {
        get => _align;
        set
        {
            if (value != _align)
            {
                _align = value;
                DoLayout();
            }
        }
    }

    public int Padding
    {
        get => _padding;
        set
        {
            if (value != _padding)
            {
                _padding = value;
                DoLayout();
            }
        }
    }

    public int Spacing
    {
        get => _spacing;
        set
        {
            if (value != _spacing)
            {
                _spacing = value;
                DoLayout();
            }
        }
    }

    public override float Width
    {
        get => _boundaries.Rect.Width;
        set
        {
            _boundaries.Set(_boundaries.Rect.X, _boundaries.Rect.Y, value, _boundaries.Rect.Height);
            _gui.RecalculateLayouts();
        }
    }

    public override float Height
    {
        get => _boundaries.Rect.Height;
        set
        {
            _boundaries.Set(_boundaries.Rect.X, _boundaries.Rect.Y, _boundaries.Rect.Width, value);
            _gui.RecalculateLayouts();
        }
    }

    protected Layout(string id, Gui gui) : base(id, gui)
    {
        _childrenAnchorsMap = new FastDictionary<int, LayoutAnchor>();
    }

    public void SetAnchor(GameObject gameObject, LayoutAnchor anchor)
    {
        _childrenAnchorsMap[gameObject.UId] = anchor;
        DoLayout();
    }

    protected override void AfterAdd(GameObject gameObject)
    {
        RecalculateBoundaries();

        DoLayout();
    }

    protected override void AfterRemove(GameObject gameObject)
    {
        RecalculateBoundaries();

        DoLayout();
    }

    private void RecalculateBoundaries()
    {
        if (Children == null)
        {
            return;
        }

        _boundaries.Set(0, 0, 0, 0);

        for (int i = 0; i < Children.Count; ++i)
        {
            _boundaries.AddRect(Children[i].Bounds);
        }
    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        var layoutDef = (definition as LayoutDef)!;

        Spacing = layoutDef.Spacing;
        Align = layoutDef.Align;
        Padding = layoutDef.Padding;
    }

    internal virtual void DoLayout()
    {
        ProcessAnchoring();
    }

    private void ProcessAnchoring()
    {
        if (Children == null) return;
        var children = Children.ReadOnlySpan;

        foreach (var child in children)
        {
            var anchor = GetChildAnchor(child);

            if (anchor == LayoutAnchor.None)
            {
                continue;
            }

            switch (anchor)
            {
                case LayoutAnchor.Left:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    break;

                case LayoutAnchor.Right:

                    child.X = Width - child.Width - Padding;
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    break;

                case LayoutAnchor.Top:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Width = Width - (2 * Padding);

                    break;

                case LayoutAnchor.Bottom:

                    child.X = Padding;
                    child.Y = Height - child.Height - Padding;
                    child.Width = Width - (2 * Padding);

                    break;

                case LayoutAnchor.Fill:

                    child.X = Padding;
                    child.Y = Padding;
                    child.Width = Width - (2 * Padding);
                    child.Height = Height - (2 * Padding);

                    break;

                case LayoutAnchor.Center:

                    child.X = (Width / 2) - (child.Width / 2.0f);
                    child.Y = (Height / 2) - (child.Height / 2.0f);

                    break;
            }
        }
    }

    protected LayoutAnchor GetChildAnchor(GameObject child)
    {
        return _childrenAnchorsMap.TryGetValue(child.UId, out var anchor) ? anchor : LayoutAnchor.None;
    }


    private int _spacing;
    private int _padding;
    private LayoutAlign _align = LayoutAlign.AlignStart;
    private readonly FastDictionary<int, LayoutAnchor> _childrenAnchorsMap;
    private Bounds _boundaries;
}
