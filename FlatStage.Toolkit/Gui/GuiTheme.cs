namespace FlatStage.Toolkit;

public abstract class GuiTheme
{
    public GuiStyleSheet StyleSheet { get; }

    protected GuiTheme(Gui gui)
    {
        _gui = gui;
        StyleSheet = new GuiStyleSheet(gui);
    }

    public abstract void DrawButton(Canvas canvas, GuiButton button);

    public abstract Vec2 MeasureText(ReadOnlySpan<char> @string, int stringLength = -1, int startIndex = 0);

    protected readonly Gui _gui;
}
