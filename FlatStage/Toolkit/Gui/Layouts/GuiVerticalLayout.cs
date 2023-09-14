namespace FlatStage.Toolkit;
public class GuiVerticalLayout : GuiLayout
{
    public GuiVerticalLayout(string id, Gui gui, GuiControl? parent = null) : base(id, gui, parent)
    {
    }

    internal override void ProcessLayout()
    {
        if (Children.Count == 0)
        {
            return;
        }

        _lastLayoutPos = 0;

        int totalHeight = 0;

        for (var i = 0; i < Children.Count; ++i)
        {
            totalHeight += Children[i].Height;
        }

        totalHeight += (Children.Count - 1) * Spacing;

        switch (LayoutMode)
        {
            case GuiLayoutMode.AlignStart:

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    child.SetPosition(Padding, Padding + _lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    _lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.AlignCenter:

                _lastLayoutPos = (Height / 2) - (totalHeight / 2);

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    child.SetPosition(Padding, _lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    _lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.AlignEnd:

                _lastLayoutPos = Height - Padding - totalHeight;

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    child.SetPosition(Padding, _lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    _lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.Fill:

                int height = (((Height - (2 * Padding) - ((Children.Count - 1) * Spacing)) / Children.Count));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    child.SetPosition(Padding, Padding + _lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), height);

                    _lastLayoutPos += height;
                }

                break;
        }
    }

    private int _lastLayoutPos;
}
