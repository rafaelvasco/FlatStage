using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public class GuiButton : GuiControl
{
    public string Label { get; set; } = "Click Me";

    public GuiButton(Gui gui, GuiContainer? parent = null) : base(gui, parent)
    {
    }

    public override Size SizeHint => new(100, 40);

    internal override void Draw(Canvas canvas, GuiSkin skin)
    {
        skin.DrawButton(canvas, this);
    }

    internal override void ProcessMouseEvent(GuiMouseState mouseState)
    {
        Console.WriteLine("Button Mouse Event");
    }
}
