using FlatStage.Graphics;
using System.Collections.Generic;

namespace FlatStage.Toolkit;

public class Style
{
    public Color BackgroundColor { get; set; }

    public Color BorderColor { get; set; }

    public Color InnerBorderColor { get; set; }

    public Color TextColor { get; set; }

    public Color ShadowColor { get; set; }

    public int BorderSize { get; set; } = 1;

    public Dictionary<string, Color>? CustomElements { get; set; }

}
