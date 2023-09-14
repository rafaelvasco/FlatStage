using FlatStage.Graphics;
using System.Text;

namespace FlatStage.Toolkit;
public abstract class GuiSkin
{
    public abstract Vec2 MeasureText(string @string, int stringLength, int startIndex = 0);

    public abstract Vec2 MeasureText(StringBuilder @string, int stringLength, int startIndex = 0);

    public abstract void DrawButton(Canvas canvas, GuiButton button);

    public abstract void DrawPanel(Canvas canvas, GuiPanel panel);

    public abstract void DrawCheckbox(Canvas canvas, GuiCheckbox checkbox);

    public abstract void DrawSlider(Canvas canvas, GuiSlider slider);

    public abstract void DrawTextbox(Canvas canvas, GuiTextbox textbox);
}
