namespace FlatStage.Toolkit;
public class GuiHorizontalLayout : GuiLayout
{
    internal static new readonly int STypeId;

    static GuiHorizontalLayout()
    {
        STypeId = ++SBTypeId;
    }

    public GuiHorizontalLayout(string id, Gui gui, GuiContainer? parent = null) : base(id, gui, parent)
    {
    }

    internal override void ProcessLayout()
    {
        base.ProcessLayout();

        var lastLayoutPos = 0;

        int totalWidth = 0;

        for (var i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            if (child.Anchor != GuiAnchoring.None)
            {
                continue;
            }

            totalWidth += child.Width;
        }

        totalWidth += (Children.Count - 1) * Spacing;

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

                    child.SetPosition(Padding + lastLayoutPos + (Spacing * i), Padding);
                    child.Resize(child.Width, Height - (2 * Padding));

                    lastLayoutPos += child.Width;
                }

                break;
            case GuiLayoutMode.AlignCenter:

                lastLayoutPos = (Width / 2) - (totalWidth / 2);

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(lastLayoutPos + (Spacing * i), Padding);
                    child.Resize(child.Width, Height - (2 * Padding));

                    lastLayoutPos += child.Width;
                }

                break;
            case GuiLayoutMode.AlignEnd:

                lastLayoutPos = Width - Padding - totalWidth;

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(lastLayoutPos + (Spacing * i), Padding);
                    child.Resize(child.Width, Height - (2 * Padding));

                    lastLayoutPos += child.Width;
                }

                break;
            case GuiLayoutMode.Fill:

                int width = (((Width - (2 * Padding) - ((Children.Count - 1) * Spacing)) / Children.Count));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    if (child.Anchor != GuiAnchoring.None)
                    {
                        continue;
                    }

                    child.SetPosition(Padding + lastLayoutPos + (Spacing * i), Padding);
                    child.Resize(width, Height - (2 * Padding));

                    lastLayoutPos += width;
                }

                break;
        }

    }
}
