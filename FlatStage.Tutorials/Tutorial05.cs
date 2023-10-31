using FlatStage.Graphics;
using FlatStage.Toolkit;
using System;

namespace FlatStage.Tutorials;
public class Tutorial05 : BaseTutorial
{
    private Gui _gui = null!;

    public override void Load()
    {
        _gui = new Gui();

        var container = new GuiPanel("panel", _gui, _gui.Desktop)
        {
            Padding = 5
        };

        var layout = new GuiVerticalLayout("mainLayout", _gui, container);

        container.Resize(300, 500);

        container.Anchor = GuiAnchoring.Center;

        layout.Spacing = 4;

        layout.Anchor = GuiAnchoring.Fill;

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

    public Tutorial05(string name) : base(name)
    {

    }

    public override void FixedUpdate(float dt)
    {
        _gui.Update(dt);
    }

    public override void Draw(Canvas canvas)
    {
        _gui.Draw(canvas);
    }

    public override void Update(float dt)
    {
    }
}
