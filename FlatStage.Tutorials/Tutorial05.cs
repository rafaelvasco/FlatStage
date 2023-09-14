using FlatStage.Graphics;
using FlatStage.Toolkit;
using System;

namespace FlatStage.Tutorials;
public class Tutorial05 : BaseTutorial
{
    private readonly Gui _gui;

    public override void Load()
    {

    }

    public Tutorial05(string name) : base(name)
    {
        _gui = new Gui();

        var container = new GuiPanel("panel", _gui);

        var layout = new GuiVerticalLayout("verticalLayout", _gui, container);

        container.Resize(300, 500);

        container.SetPosition(
            (_gui.Width / 2) - (container.Width / 2),
            (_gui.Height / 2) - (container.Height / 2)
        );

        layout.SetPosition(1, 1);

        layout.Resize(container.Width - 2, container.Height - 2);

        layout.Spacing = 4;
        layout.Padding = 10;

        var button = new GuiButton("button", _gui, layout);

        button.OnMouseUp += (_) => Console.WriteLine("Button1 Click");

        var button2 = new GuiButton("button2", _gui, layout);

        button2.OnMouseUp += (_) => Console.WriteLine("Button2 Click");

        var button3 = new GuiButton("button3", _gui, layout);

        button3.OnMouseUp += (_) => Console.WriteLine("Button3 Click");

        var button4 = new GuiButton("button4", _gui, layout);

        button4.OnMouseUp += (_) => Console.WriteLine("Button4 Click");

        var checkBox1 = new GuiCheckbox("checkbox1", _gui, layout);

        var slider1 = new GuiSlider("slider", _gui, layout);

        var textbox = new GuiTextbox("textbox", _gui, layout);

    }

    public override void FixedUpdate(float dt)
    {
        _gui.Update(dt);
    }

    public override void Draw(Canvas canvas, float dt)
    {
        _gui.Draw(canvas);
    }

}
