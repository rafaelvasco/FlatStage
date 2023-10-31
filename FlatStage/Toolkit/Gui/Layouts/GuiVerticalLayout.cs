namespace FlatStage.Toolkit;
public class GuiVerticalLayout : GuiLayout
{
    internal static new readonly int STypeId;

    static GuiVerticalLayout()
    {
        STypeId = ++SBTypeId;
    }

    public GuiVerticalLayout(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
    }

    internal override void ProcessLayout()
    {
        base.ProcessLayout();

        var lastLayoutPos = 0;

        int totalHeight = 0;

        for (var i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            if (child.Anchor != GuiAnchoring.None)
            {
                continue;
            }

            totalHeight += child.Height;
        }

        totalHeight += (Children.Count - 1) * Spacing;

        switch (LayoutMode)
        {
            case GuiLayoutMode.AlignStart:

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(Padding, Padding + lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.AlignCenter:

                lastLayoutPos = (Height / 2) - (totalHeight / 2);

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(Padding, lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.AlignEnd:

                lastLayoutPos = Height - Padding - totalHeight;

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(Padding, lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), child.Height);

                    lastLayoutPos += child.Height;
                }

                break;
            case GuiLayoutMode.Fill:

                int height = (((Height - (2 * Padding) - ((Children.Count - 1) * Spacing)) / Children.Count));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(Padding, Padding + lastLayoutPos + (Spacing * i));
                    child.Resize(Width - (2 * Padding), height);

                    lastLayoutPos += height;
                }

                break;
        }

    }
}
