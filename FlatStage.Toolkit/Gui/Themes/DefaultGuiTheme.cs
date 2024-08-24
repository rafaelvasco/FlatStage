namespace FlatStage.Toolkit;

public class DefaultGuiTheme : GuiTheme
{
    private static readonly Color MainColor = Color.DodgerBlue;

    private static readonly Color BackgroundColor = new(25, 25, 25);

    private static readonly Color DarkBackgroundColor = new(20, 20, 20);

    private static readonly Color MainTextColor = new(250, 250, 250);

    public DefaultGuiTheme(Gui gui) : base(gui)
    {
        // All Controls Base Style
        StyleSheet.SetProperty(GuiStyleProperties.AllElementsTypeId, GuiControlState.Idle, GuiStyleProperties.BorderSize, 1);
        StyleSheet.SetProperty(GuiStyleProperties.AllElementsTypeId, GuiControlState.Idle, GuiStyleProperties.BackgroundColor, BackgroundColor);
        StyleSheet.SetProperty(GuiStyleProperties.AllElementsTypeId, GuiControlState.Idle, GuiStyleProperties.BorderColor, DarkBackgroundColor);
        StyleSheet.SetProperty(GuiStyleProperties.AllElementsTypeId, GuiControlState.Idle, GuiStyleProperties.InnerBorderColor, BackgroundColor * 2.0f);
        StyleSheet.SetProperty(GuiStyleProperties.AllElementsTypeId, GuiControlState.Idle, GuiStyleProperties.TextColor, MainTextColor);

        // Button Base Style
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Hover, GuiStyleProperties.BackgroundColor, MainColor);
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Hover, GuiStyleProperties.BorderColor, MainColor);
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Hover, GuiStyleProperties.InnerBorderColor, BackgroundColor * 0.5f);
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Active, GuiStyleProperties.BackgroundColor, BackgroundColor * 0.5f);
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Active, GuiStyleProperties.BorderColor, BackgroundColor * 2.0f);
        StyleSheet.SetProperty(nameof(GuiButton), GuiControlState.Active, GuiStyleProperties.InnerBorderColor, BackgroundColor * 0.3f);


        // // CheckBox Base Style
        // StyleSheet.SetProperty(nameof(GuiCheckbox), GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckBgColor, DarkBackgroundColor);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckBorderColor, DarkBackgroundColor * 2.0f);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckSize, 30);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxLabelMargin, 5);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Active, DefaultStyleProperties.CheckBoxCheckBgColor, MainColor * 0.5f);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Active, DefaultStyleProperties.CheckBoxCheckBorderColor, MainColor);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Checked, DefaultStyleProperties.CheckBoxCheckBgColor, MainColor);
        // StyleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Checked, DefaultStyleProperties.CheckBoxCheckBorderColor, MainColor * 2.0f);
        //
        // // ManuBar Base Style
        // StyleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Idle, DefaultStyleProperties.MenuMenuItemColor, BackgroundColor);
        // StyleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.MenuMenuItemColor, MainColor * 0.5f);
        // StyleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.BorderColor, DarkBackgroundColor);
        // StyleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, BackgroundColor);
        // StyleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Active, DefaultStyleProperties.MenuMenuItemColor, MainColor);
        //
        // // Slider Base Style
        // StyleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        // StyleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Idle, DefaultStyleProperties.SliderThumbColor, MainColor);
        // StyleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Hover, DefaultStyleProperties.SliderThumbColor, MainColor);
        // StyleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Active, DefaultStyleProperties.SliderThumbColor, MainColor * 0.5f);
        // StyleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        //
        // // TextBox Base Style
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.BorderColor, BackgroundColor * 2.0f);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.InnerBorderColor, DarkBackgroundColor * 0.5f);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretColor, MainColor);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretWidth, 15);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretHeight, 30);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretPadding, 10);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxSelectionColor, MainColor);
        // StyleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        //
        // // Window Base Style
        // StyleSheet.SetProperty(GuiWindow.STypeId, GuiControlState.Idle, DefaultStyleProperties.WindowTopBarHeight, 40);
        // StyleSheet.SetProperty(GuiWindow.STypeId, GuiControlState.Idle, DefaultStyleProperties.WindowTopBarColor, MainColor);
        //
        // // Tree Base Style
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeIndicatorColor, MainColor * 0.5f);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeElementsMargin, 5);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeIndicatorSize, 10);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeSpacing, 15);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Hover, DefaultStyleProperties.TreeNodeIndicatorColor, MainColor);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Hover, DefaultStyleProperties.TextColor, MainColor);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Active, DefaultStyleProperties.TreeNodeIndicatorColor, Color.Red);
        // StyleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Active, DefaultStyleProperties.TextColor, MainColor);

    }




    public override void DrawButton(Canvas canvas, GuiButton button)
    {
        const float offset = 4.0f;

        var borderSize = StyleSheet.GetProperty<int>(button, GuiStyleProperties.BorderSize);

        var buttonInteractable = Behaviors.GetComponent<Interactable>(button);

        DrawControlRect(canvas, button, StyleSheet);

        var labelSize = MeasureText(button.Label);

        var labelX = button.GlobalBounds.X + borderSize + ((button.Width - (2 * borderSize)) / 2.0f) - (labelSize.X / 2.0f);
        var labelY = button.GlobalBounds.Y + borderSize + ((button.Height - (2 * borderSize)) / 2.0f) - (labelSize.Y / 2.0f);

        var textColor = StyleSheet.GetProperty<Color>(button, GuiStyleProperties.TextColor);

        canvas.DrawText(_font, button.Label, new Vec2(labelX, !buttonInteractable.Pressed ? labelY : labelY + (offset / 2)), new Vec2(_textScale), textColor);
    }

    private static void DrawControlRect(Canvas canvas, GuiControl control, GuiStyleSheet stylesheet)
    {
        var borderSize = stylesheet.GetProperty<int>(control, GuiStyleProperties.BorderSize);
        var backgroundColor = stylesheet.GetProperty<Color>(control, GuiStyleProperties.BackgroundColor);
        var borderColor = stylesheet.GetProperty<Color>(control, GuiStyleProperties.BorderColor);
        var innerBorderColor = stylesheet.GetProperty<Color>(control, GuiStyleProperties.InnerBorderColor);

        DrawBorderedRect(
            canvas,
            control.GlobalBounds.X,
            control.GlobalBounds.Y,
            control.Width,
            control.Height,
            borderSize,
            backgroundColor,
            borderColor
        );
        canvas.DrawRect(
            control.GlobalBounds.X + borderSize,
            control.GlobalBounds.Y + borderSize,
            control.Width - (2 * borderSize),
            control.Height - (2 * borderSize),
            borderSize,
            innerBorderColor
        );
    }

    private static void DrawBorderedRect(Canvas canvas, float x, float y, float width, float height, int borderSize, Color bgColor, Color borderColor)
    {
        canvas.DrawRect(x, y, width, height, borderSize, borderColor);
        canvas.FillRect(x + borderSize, y + borderSize, width - (2 * borderSize), height - (2 * borderSize), bgColor);
    }

    public override Vec2 MeasureText(ReadOnlySpan<char> text, int stringLength = -1, int startIndex = 0)
    {
        return _font.MeasureString(text, stringLength >= 0 ? stringLength : text.Length, startIndex) * _textScale;
    }

    private readonly float _textScale = 1.0f;
    private readonly TextureFont _font = BuiltinContent.Fonts.Monogram;
}
