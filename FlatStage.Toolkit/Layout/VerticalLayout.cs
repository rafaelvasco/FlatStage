namespace FlatStage.Toolkit;

public class VerticalLayout : Layout
{
    internal VerticalLayout(string id, Gui gui) : base(id, gui)
    {
    }

    internal override void DoLayout()
    {
        base.DoLayout();

        if (Children == null)
        {
            return;
        }

        float lastLayoutPos = 0;

        float totalHeight = 0;

        for (var i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            var anchor = GetChildAnchor(child);

            if (anchor != LayoutAnchor.None)
            {
                continue;
            }

            totalHeight += child.Height;
        }

        totalHeight += (Children.Count - 1) * Spacing;

        switch (Align)
        {
            case LayoutAlign.AlignStart:

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = Padding;
                    child.Y = Padding + lastLayoutPos + (Spacing * i);
                    child.Width = Width - (2 * Padding);

                    lastLayoutPos += child.Height;
                }

                break;
            case LayoutAlign.AlignCenter:

                lastLayoutPos = (Height / 2) - (totalHeight / 2);

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = Padding;
                    child.Y = lastLayoutPos + (Spacing * i);
                    child.Width = Width - (2 * Padding);

                    lastLayoutPos += child.Height;
                }

                break;
            case LayoutAlign.AlignEnd:

                lastLayoutPos = Height - Padding - totalHeight;

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = Padding;
                    child.Y = lastLayoutPos + (Spacing * i);
                    child.Width = Width - (2 * Padding);

                    lastLayoutPos += child.Height;
                }

                break;
            case LayoutAlign.Fill:

                float height = (((Height - (2 * Padding) - ((Children.Count - 1) * Spacing)) / Children.Count));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = Padding;
                    child.Y = Padding + lastLayoutPos + (Spacing * i);
                    child.Width = Width - (2 * Padding);

                    lastLayoutPos += height;
                }

                break;
        }
    }

    public override Size SizeHint => new(100, 100);
}
