namespace FlatStage.Toolkit;

public class HorizontalLayout : Layout
{
    internal HorizontalLayout(string id, Gui gui) : base(id, gui)
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

        float totalWidth = 0;

        for (var i = 0; i < Children.Count; ++i)
        {
            var child = Children[i];

            var anchor = GetChildAnchor(child);

            if (anchor != LayoutAnchor.None)
            {
                continue;
            }

            totalWidth += child.Width;
        }

        totalWidth += (Children.Count - 1) * Spacing;

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

                    child.X = Padding + lastLayoutPos + (Spacing * i);
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    lastLayoutPos += child.Width;
                }

                break;
            case LayoutAlign.AlignCenter:

                lastLayoutPos = (Width / 2) - (totalWidth / 2);

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = lastLayoutPos + (Spacing * i);
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    lastLayoutPos += child.Width;
                }

                break;
            case LayoutAlign.AlignEnd:

                lastLayoutPos = Width - Padding - totalWidth;

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = lastLayoutPos + (Spacing * i);
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    lastLayoutPos += child.Width;
                }

                break;
            case LayoutAlign.Fill:

                var width = (((Width - (2 * Padding) - ((Children.Count - 1) * Spacing)) / Children.Count));

                for (var i = 0; i < Children.Count; ++i)
                {
                    var child = Children[i];

                    var anchor = GetChildAnchor(child);

                    if (anchor != LayoutAnchor.None)
                    {
                        continue;
                    }

                    child.X = Padding + lastLayoutPos + (Spacing * i);
                    child.Y = Padding;
                    child.Height = Height - (2 * Padding);

                    lastLayoutPos += width;
                }

                break;
        }
    }

    public override Size SizeHint => new(100, 100);
}
