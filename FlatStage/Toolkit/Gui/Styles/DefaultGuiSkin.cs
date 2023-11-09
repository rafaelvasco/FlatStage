using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public class DefaultGuiSkin : GuiSkin
{

    public override GuiStyleSheet StyleSheet => _styleSheet;

    public Color MainColor { get; set; } = Color.DodgerBlue;

    public Color BackgroundColor { get; set; } = new(25, 25, 25);

    public Color DarkBackgroundColor { get; set; } = new(20, 20, 20);

    public Color MainTextColor = new(250, 250, 250);

    public DefaultGuiSkin(Gui gui)
    {
        _gui = gui;
        _styleSheet = new GuiStyleSheet();

        // All Controls Base Style
        _styleSheet.SetProperty(GuiControl.AllControlsTypeId, GuiControlState.Idle, DefaultStyleProperties.BorderSize, 1);
        _styleSheet.SetProperty(GuiControl.AllControlsTypeId, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, BackgroundColor);
        _styleSheet.SetProperty(GuiControl.AllControlsTypeId, GuiControlState.Idle, DefaultStyleProperties.BorderColor, DarkBackgroundColor);
        _styleSheet.SetProperty(GuiControl.AllControlsTypeId, GuiControlState.Idle, DefaultStyleProperties.InnerBorderColor, BackgroundColor * 2.0f);
        _styleSheet.SetProperty(GuiControl.AllControlsTypeId, GuiControlState.Idle, DefaultStyleProperties.TextColor, MainTextColor);

        // Button Base Style
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, BackgroundColor * 1.5f);
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Hover, DefaultStyleProperties.BorderColor, MainColor);
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Hover, DefaultStyleProperties.InnerBorderColor, BackgroundColor * 0.5f);
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Active, DefaultStyleProperties.BackgroundColor, BackgroundColor * 0.5f);
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Active, DefaultStyleProperties.BorderColor, BackgroundColor * 2.0f);
        _styleSheet.SetProperty(GuiButton.STypeId, GuiControlState.Active, DefaultStyleProperties.InnerBorderColor, BackgroundColor * 0.3f);

        // CheckBox Base Style
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckBgColor, DarkBackgroundColor);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckBorderColor, DarkBackgroundColor * 2.0f);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxCheckSize, 30);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.CheckBoxLabelMargin, 5);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Active, DefaultStyleProperties.CheckBoxCheckBgColor, MainColor * 0.5f);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Active, DefaultStyleProperties.CheckBoxCheckBorderColor, MainColor);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Checked, DefaultStyleProperties.CheckBoxCheckBgColor, MainColor);
        _styleSheet.SetProperty(GuiCheckbox.STypeId, GuiControlState.Checked, DefaultStyleProperties.CheckBoxCheckBorderColor, MainColor * 2.0f);

        // ManuBar Base Style
        _styleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Idle, DefaultStyleProperties.MenuMenuItemColor, BackgroundColor);
        _styleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.MenuMenuItemColor, MainColor * 0.5f);
        _styleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.BorderColor, DarkBackgroundColor);
        _styleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, BackgroundColor);
        _styleSheet.SetProperty(GuiMenuBar.STypeId, GuiControlState.Active, DefaultStyleProperties.MenuMenuItemColor, MainColor);

        // Slider Base Style
        _styleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        _styleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Idle, DefaultStyleProperties.SliderThumbColor, MainColor);
        _styleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Hover, DefaultStyleProperties.SliderThumbColor, MainColor);
        _styleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Active, DefaultStyleProperties.SliderThumbColor, MainColor * 0.5f);
        _styleSheet.SetProperty(GuiSlider.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);

        // TextBox Base Style
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.BorderColor, BackgroundColor * 2.0f);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.InnerBorderColor, DarkBackgroundColor * 0.5f);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretColor, MainColor);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretWidth, 15);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretHeight, 30);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxCaretPadding, 10);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Idle, DefaultStyleProperties.TextboxSelectionColor, MainColor);
        _styleSheet.SetProperty(GuiTextbox.STypeId, GuiControlState.Hover, DefaultStyleProperties.BackgroundColor, DarkBackgroundColor);

        // Window Base Style
        _styleSheet.SetProperty(GuiWindow.STypeId, GuiControlState.Idle, DefaultStyleProperties.WindowTopBarHeight, 40);
        _styleSheet.SetProperty(GuiWindow.STypeId, GuiControlState.Idle, DefaultStyleProperties.WindowTopBarColor, MainColor);

        // Tree Base Style
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeIndicatorColor, MainColor * 0.5f);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeElementsMargin, 5);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeIndicatorSize, 10);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Idle, DefaultStyleProperties.TreeNodeSpacing, 15);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Hover, DefaultStyleProperties.TreeNodeIndicatorColor, MainColor);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Hover, DefaultStyleProperties.TextColor, MainColor);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Active, DefaultStyleProperties.TreeNodeIndicatorColor, Color.Red);
        _styleSheet.SetProperty(GuiTree.STypeId, GuiControlState.Active, DefaultStyleProperties.TextColor, MainColor);

    }

    public override void DrawButton(Canvas canvas, GuiButton button)
    {
        var font = _gui.Font;

        var offset = 4.0f;

        var borderSize = StyleSheet.GetProperty<int>(button, DefaultStyleProperties.BorderSize);

        DrawControlRect(canvas, button, StyleSheet);

        var labelSize = MeasureText(button.Label);

        var labelX = button.GlobalX + borderSize + ((button.Width - (2 * borderSize)) / 2.0f) - (labelSize.X / 2.0f);
        var labelY = button.GlobalY + borderSize + ((button.Height - (2 * borderSize)) / 2.0f) - (labelSize.Y / 2.0f);

        var textColor = StyleSheet.GetProperty<Color>(button, DefaultStyleProperties.TextColor);

        canvas.DrawText(font, button.Label, new Vec2(labelX, !button.Active ? labelY : labelY + (offset / 2)), new Vec2(_textScale), textColor);

    }

    public override void DrawPanel(Canvas canvas, GuiPanel panel)
    {
        DrawControlRect(canvas, panel, StyleSheet);
    }

    private static void DrawControlRect(Canvas canvas, GuiControl control, GuiStyleSheet stylesheet)
    {
        var borderSize = stylesheet.GetProperty<int>(control, DefaultStyleProperties.BorderSize);
        var backgroundColor = stylesheet.GetProperty<Color>(control, DefaultStyleProperties.BackgroundColor);
        var borderColor = stylesheet.GetProperty<Color>(control, DefaultStyleProperties.BorderColor);
        var innerBorderColor = stylesheet.GetProperty<Color>(control, DefaultStyleProperties.InnerBorderColor);

        DrawBorderedRect(
            canvas,
            control.GlobalX,
            control.GlobalY,
            control.Width,
            control.Height,
            borderSize,
            backgroundColor,
            borderColor
        );
        canvas.DrawRect(
            control.GlobalX + borderSize,
            control.GlobalY + borderSize,
            control.Width - (2 * borderSize),
            control.Height - (2 * borderSize),
            borderSize,
            innerBorderColor
        );
    }

    private static void DrawBorderedRect(Canvas canvas, int x, int y, int width, int height, int borderSize, Color bgColor, Color borderColor)
    {
        canvas.DrawRect(x, y, width, height, borderSize, borderColor);
        canvas.FillRect(x + borderSize, y + borderSize, width - (2 * borderSize), height - (2 * borderSize), bgColor);
    }

    public override void DrawCheckbox(Canvas canvas, GuiCheckbox checkbox)
    {
        var font = _gui.Font;

        DrawControlRect(canvas, checkbox, StyleSheet);

        var x = checkbox.GlobalX;
        var y = checkbox.GlobalY;

        var indicatorBgColor = StyleSheet.GetProperty<Color>(checkbox, DefaultStyleProperties.CheckBoxCheckBgColor);
        var indicatorBorderColor = StyleSheet.GetProperty<Color>(checkbox, DefaultStyleProperties.CheckBoxCheckBorderColor);
        var textColor = StyleSheet.GetProperty<Color>(checkbox, DefaultStyleProperties.TextColor);
        var borderSize = StyleSheet.GetProperty<int>(checkbox, DefaultStyleProperties.BorderSize);
        var checkSize = StyleSheet.GetProperty<int>(checkbox, DefaultStyleProperties.CheckBoxCheckSize);
        int labelMargin = StyleSheet.GetProperty<int>(checkbox, DefaultStyleProperties.CheckBoxLabelMargin);

        var checkRect = new Rect(
            x + (checkbox.Width / 2) - (checkSize / 2),
            y + (checkbox.Height / 2) - (checkSize / 2),
            checkSize, checkSize);

        DrawBorderedRect(canvas, checkRect.X, checkRect.Y, checkRect.Width, checkRect.Height, borderSize, indicatorBgColor, indicatorBorderColor);

        var label = checkbox.Checked ? "ON" : "OFF";
        var labelSize = MeasureText(label);

        canvas.DrawText(
            font,
            checkbox.Checked ? "ON" : "OFF",
            new Vec2(
                checkRect.X + checkRect.Width + labelMargin,
                y + borderSize + ((checkbox.Height - (2 * borderSize)) / 2.0f) - (labelSize.Y / 2)),
            new Vec2(_textScale), textColor
        );
    }

    public override void DrawSlider(Canvas canvas, GuiSlider slider)
    {
        var x = slider.GlobalX;
        var y = slider.GlobalY;

        var thumbBgColor = StyleSheet.GetProperty<Color>(slider, DefaultStyleProperties.SliderThumbColor);
        var borderSize = StyleSheet.GetProperty<int>(slider, DefaultStyleProperties.BorderSize);
        var thumbBorderColor = thumbBgColor * 2.0f;

        DrawControlRect(canvas, slider, StyleSheet);
        DrawBorderedRect(canvas, x + slider.ThumbRect.X, y + slider.ThumbRect.Y, slider.ThumbRect.Width, slider.ThumbRect.Height, borderSize, thumbBgColor, thumbBorderColor);
    }

    public override void DrawTextbox(Canvas canvas, GuiTextbox textbox)
    {
        var font = _gui.Font;

        var x = textbox.GlobalX;
        var y = textbox.GlobalY;

        var caretWidth = StyleSheet.GetProperty<int>(textbox, DefaultStyleProperties.TextboxCaretWidth);
        var caretHeight = StyleSheet.GetProperty<int>(textbox, DefaultStyleProperties.TextboxCaretHeight);
        var caretColor = StyleSheet.GetProperty<Color>(textbox, DefaultStyleProperties.TextboxCaretColor);
        var padding = StyleSheet.GetProperty<int>(textbox, DefaultStyleProperties.TextboxCaretPadding);
        var borderSize = StyleSheet.GetProperty<int>(textbox, DefaultStyleProperties.BorderSize);
        var textColor = StyleSheet.GetProperty<Color>(textbox, DefaultStyleProperties.TextColor);
        var selectionColor = StyleSheet.GetProperty<Color>(textbox, DefaultStyleProperties.TextboxSelectionColor);

        var textSelected = textbox.SelectionEndIndex > textbox.SelectionStartIndex;

        DrawControlRect(canvas, textbox, StyleSheet);

        var textSizeToCursor = MeasureText(textbox.InternalText, textbox.CaretOffset);
        var fixedTextSize = MeasureText("L");
        var textSize = MeasureText(textbox.InternalText, textbox.SelectionEndIndex - textbox.SelectionStartIndex);

        // Selection Rect
        if (textSelected)
        {
            canvas.FillRect(
                x + borderSize + padding + (textbox.SelectionStartIndex * fixedTextSize.X),
                y + borderSize + ((textbox.Height - (2 * borderSize)) / 2) - (caretHeight / 2),
                textSize.X,
                caretHeight,
            selectionColor);
        }

        // Text
        if (textbox.InternalText.Length > 0)
        {
            canvas.DrawText(
                font,
                textbox.InternalText,
                new Vec2(
                    x + borderSize + padding,
                    y + borderSize + ((textbox.Height - (2 * borderSize)) / 2) - (fixedTextSize.Y / 2)
                ),
                new Vec2(_textScale),
                textColor
            );
        }

        // Cursor
        if (textbox.Focused && textbox.ShowCursor)
        {
            canvas.FillRect(
                x + borderSize + padding + textSizeToCursor.X,
                y + borderSize + ((textbox.Height - (2 * borderSize)) / 2) - (caretHeight / 2),
                caretWidth,
                caretHeight,
                caretColor
            );
        }
    }

    public override void DrawMenuBar(Canvas canvas, GuiMenuBar menubar)
    {
        DrawControlRect(canvas, menubar, StyleSheet);

        foreach (var item in menubar.MenuItems)
        {
            if (item.Parent == null || item.Parent.IsExpanded)
            {
                DrawMenuItem(canvas, menubar, item);
            }
        }
    }

    private void DrawMenuItem(Canvas canvas, GuiMenuBar menuBar, GuiMenuItem guiMenuItem)
    {
        var font = _gui.Font;

        var itemRect = guiMenuItem.ItemRect;
        var globalRect = new Rect(menuBar.GlobalX + itemRect.X, menuBar.GlobalY + itemRect.Y, itemRect.Width, itemRect.Height);

        var state = GuiControlState.Idle;

        if (guiMenuItem.IsHovered)
        {
            state = GuiControlState.Hover;
        }

        if (guiMenuItem.IsExpanded)
        {
            state = GuiControlState.Active;
        }

        var menuItemColor = StyleSheet.GetProperty<Color>(menuBar, DefaultStyleProperties.MenuMenuItemColor, state);

        if (guiMenuItem.Level == 0 && !guiMenuItem.IsHovered && !guiMenuItem.IsExpanded)
        {
            menuItemColor = Color.Transparent;
        }

        canvas.FillRect(globalRect.X, globalRect.Y, globalRect.Width, globalRect.Height, menuItemColor);

        var labelSize = MeasureText(guiMenuItem.Label);

        var textX = globalRect.X + (globalRect.Width / 2) - (labelSize.X / 2);
        var textY = globalRect.Y + (globalRect.Height / 2) - (labelSize.Y / 2);

        canvas.DrawText(font, guiMenuItem.Label, new Vec2(textX, textY), Color.White);
    }

    public override Vec2 MeasureText(ReadOnlySpan<char> text, int stringLength = -1, int startIndex = 0)
    {
        return _gui.Font.MeasureString(text, stringLength >= 0 ? stringLength : text.Length, startIndex) * _textScale;
    }

    public override void DrawWindow(Canvas canvas, GuiWindow window)
    {
        DrawControlRect(canvas, window, StyleSheet);
    }

    public override void DrawText(Canvas canvas, GuiText text)
    {
        var x = text.TextDrawX;
        var y = text.TextDrawY;
        var font = _gui.Font;

        var textColor = StyleSheet.GetProperty<Color>(text, DefaultStyleProperties.TextColor);

        canvas.DrawText(font, text.Text, new Vec2(x, y), textColor);
    }

    public override void DrawTree(Canvas canvas, GuiTree tree)
    {
        DrawControlRect(canvas, tree, StyleSheet);

        var x = tree.GlobalX;
        var y = tree.GlobalY;
        var font = _gui.Font;

        foreach (var node in tree.Nodes)
        {
            if (node.Parent?.Expanded == false)
            {
                continue;
            }

            var nodeX = node.Rect.X + x;
            var nodeY = node.Rect.Y + y;

            var nodeLabelSize = MeasureText(node.Label);
            var nodeRect = new Rect(nodeX, nodeY, node.Rect.Width, node.Rect.Height);
            var nodeElementsMargin = StyleSheet.GetProperty<int>(tree, DefaultStyleProperties.TreeNodeElementsMargin);

            var nodeIndicatorSize = StyleSheet.GetProperty<int>(tree, DefaultStyleProperties.TreeNodeIndicatorSize);

            var indicatorState = GuiControlState.Idle;

            if (node.Expanded)
            {
                indicatorState = GuiControlState.Active;
            }
            else if (node.Hovered)
            {
                indicatorState = GuiControlState.Hover;
            }

            var textColor = StyleSheet.GetProperty<Color>(tree, DefaultStyleProperties.TextColor, indicatorState);

            if (node.HasChildren)
            {
                var indicatorColor = StyleSheet.GetProperty<Color>(tree, DefaultStyleProperties.TreeNodeIndicatorColor, indicatorState);
                canvas.FillRect(nodeRect.X + nodeElementsMargin, nodeRect.Y + (nodeRect.Height / 2) - (nodeIndicatorSize / 2), nodeIndicatorSize, nodeIndicatorSize, indicatorColor);
            }

            canvas.DrawText(font, node.Label, new Vec2(nodeRect.X + nodeIndicatorSize + (nodeElementsMargin * 2), nodeY + (node.Rect.Height / 2) - (nodeLabelSize.Y / 2)), textColor);

        }

    }

    private readonly GuiStyleSheet _styleSheet;
    private readonly float _textScale = 1.0f;
    private readonly Gui _gui;

}
