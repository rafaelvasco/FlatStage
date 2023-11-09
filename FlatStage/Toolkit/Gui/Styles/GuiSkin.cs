using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public abstract class GuiSkin
{
    public abstract GuiStyleSheet StyleSheet { get; }

    public abstract Vec2 MeasureText(ReadOnlySpan<char> @string, int stringLength = -1, int startIndex = 0);

    public abstract void DrawButton(Canvas canvas, GuiButton button);

    public abstract void DrawPanel(Canvas canvas, GuiPanel panel);

    public abstract void DrawCheckbox(Canvas canvas, GuiCheckbox checkbox);

    public abstract void DrawSlider(Canvas canvas, GuiSlider slider);

    public abstract void DrawTextbox(Canvas canvas, GuiTextbox textbox);

    public abstract void DrawMenuBar(Canvas canvas, GuiMenuBar menubar);

    public abstract void DrawWindow(Canvas canvas, GuiWindow window);

    public abstract void DrawText(Canvas canvas, GuiText text);

    public abstract void DrawTree(Canvas canvas, GuiTree tree);
}
