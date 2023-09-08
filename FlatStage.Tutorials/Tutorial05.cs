using FlatStage.Graphics;
using FlatStage.Toolkit;

namespace FlatStage.Tutorials;
public class Tutorial05 : BaseTutorial
{
    private readonly Gui _gui;
    private readonly GuiButton _button;

    public override void Load()
    {

    }

    public Tutorial05(string name) : base(name)
    {
        _gui = new Gui();

        _button = new GuiButton(_gui);

        _button.SetPosition(
            (_gui.Width / 2) - (_button.Width / 2),
            (_gui.Height / 2) - (_button.Height / 2)
        );
    }

    public override void Draw(Canvas canvas, float dt)
    {
        _gui.Draw(canvas);
    }

}
