using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public class DefaultGuiSkin : GuiSkin
{
    public override GuiStyleSheet StyleSheet => _styleSheet;

    public DefaultGuiSkin(Gui gui)
    {
        _gui = gui;
        _styleSheet = new GuiStyleSheet();
    }

    public override void DrawButton(Canvas canvas, GuiButton button)
    {
        var style = StyleSheet.GetControlStyle(button).Get(button.State);

        var font = _gui.Font;

        var offset = 4.0f;

        var borderSize = style.BorderSize;

        DrawControlRect(canvas, button, style);

        var labelSize = MeasureText(button.Label);

        var labelX = button.GlobalX + borderSize + ((button.Width - (2 * borderSize)) / 2.0f) - (labelSize.X / 2.0f);
        var labelY = button.GlobalY + borderSize + ((button.Height - (2 * borderSize)) / 2.0f) - (labelSize.Y / 2.0f);

        canvas.DrawText(font, button.Label, new Vec2(labelX, !button.Active ? labelY : labelY + (offset / 2)), new Vec2(_textScale), style.TextColor);

    }

    public override void DrawPanel(Canvas canvas, GuiPanel panel)
    {
        var style = StyleSheet.GetControlStyle(panel).Get(panel.State);

        DrawControlRect(canvas, panel, style);
    }

    private static void DrawControlRect(Canvas canvas, GuiControl control, Style style)
    {
        DrawBorderedRect(canvas, control.GlobalX, control.GlobalY, control.Width, control.Height, style.BorderSize, style.BackgroundColor, style.BorderColor);
    }

    private static void DrawBorderedRect(Canvas canvas, int x, int y, int width, int height, int borderSize, Color bgColor, Color borderColor)
    {
        canvas.DrawRect(x, y, width, height, borderSize, borderColor);
        canvas.FillRect(x + borderSize, y + borderSize, width - (2 * borderSize), height - (2 * borderSize), bgColor);
    }

    public override void DrawCheckbox(Canvas canvas, GuiCheckbox checkbox)
    {
        var style = StyleSheet.GetControlStyle(checkbox).Get(checkbox.State);
        var checkedStyle = StyleSheet.GetControlStyle(checkbox).Get(GuiControlState.Active);

        var font = _gui.Font;

        const int IndicatorOffset = 10;
        const int LabelMargin = 10;

        DrawControlRect(canvas, checkbox, style);

        var x = checkbox.GlobalX;

        var y = checkbox.GlobalY;

        var indicatorRect = new Rect(x + IndicatorOffset, y + IndicatorOffset, checkbox.Height - (IndicatorOffset * 2), checkbox.Height - (IndicatorOffset * 2));

        var indicatorBgColor = style.CustomElements![GuiCheckbox.CheckCustomElementId];
        var indicatorBorderColor = style.BorderColor;

        DrawBorderedRect(canvas, indicatorRect.X, indicatorRect.Y, indicatorRect.Width, indicatorRect.Height, style.BorderSize, indicatorBgColor, indicatorBorderColor);

        var label = checkbox.Checked ? "ON" : "OFF";
        var labelSize = MeasureText(label);
        var labelColor = checkedStyle.TextColor;

        canvas.DrawText(
            font,
            checkbox.Checked ? "ON" : "OFF",
            new Vec2(
                indicatorRect.X + indicatorRect.Width + LabelMargin,
                y + style.BorderSize + ((checkbox.Height - (2 * style.BorderSize)) / 2.0f) - (labelSize.Y / 2)),
            new Vec2(_textScale), labelColor
        );
    }

    public override void DrawSlider(Canvas canvas, GuiSlider slider)
    {
        var style = StyleSheet.GetControlStyle(slider).Get(slider.State);

        var x = slider.GlobalX;
        var y = slider.GlobalY;

        var thumbBgColor = style.CustomElements![GuiSlider.ThumbCustomElementId];
        var thumbBorderColor = style.BorderColor;

        DrawControlRect(canvas, slider, style);
        DrawBorderedRect(canvas, x + slider.ThumbRect.X, y + slider.ThumbRect.Y, slider.ThumbRect.Width, slider.ThumbRect.Height, style.BorderSize, thumbBgColor, thumbBorderColor);
    }

    public override void DrawTextbox(Canvas canvas, GuiTextbox textbox)
    {
        var style = StyleSheet.GetControlStyle(textbox).Get(textbox.State);

        var font = _gui.Font;

        var x = textbox.GlobalX;
        var y = textbox.GlobalY;

        var caretWidth = GuiTextbox.CaretWidth;
        var caretHeight = GuiTextbox.CaretHeight;
        var padding = GuiTextbox.Padding;

        var textSelected = textbox.SelectionEndIndex > textbox.SelectionStartIndex;

        DrawControlRect(canvas, textbox, style);

        var textSizeToCursor = MeasureText(textbox.InternalText, textbox.CaretOffset);
        var fixedTextSize = MeasureText("L");
        var textSize = MeasureText(textbox.InternalText, textbox.SelectionEndIndex - textbox.SelectionStartIndex);

        // Selection Rect
        if (textSelected)
        {
            canvas.FillRect(
                x + style.BorderSize + padding + (textbox.SelectionStartIndex * fixedTextSize.X),
                y + style.BorderSize + ((textbox.Height - (2 * style.BorderSize)) / 2) - (caretHeight / 2),
                textSize.X,
                caretHeight,
            Color.Fuchsia);
        }

        // Text
        if (textbox.InternalText.Length > 0)
        {
            canvas.DrawText(
                font,
                textbox.InternalText,
                new Vec2(
                    x + style.BorderSize + padding,
                    y + style.BorderSize + ((textbox.Height - (2 * style.BorderSize)) / 2) - (fixedTextSize.Y / 2)
                ),
                new Vec2(_textScale),
                style.TextColor
            );
        }

        var caretColor = style.CustomElements![GuiTextbox.CaretCustomElementId];

        // Cursor
        if (textbox.Focused && textbox.ShowCursor)
        {
            canvas.FillRect(
                x + style.BorderSize + padding + textSizeToCursor.X,
                y + style.BorderSize + ((textbox.Height - (2 * style.BorderSize)) / 2) - (caretHeight / 2),
                caretWidth,
                caretHeight,
                caretColor
            );
        }
    }

    public override void DrawMenuBar(Canvas canvas, GuiMenuBar menubar)
    {
        var style = StyleSheet.GetControlStyle(menubar).Get(menubar.State);

        DrawControlRect(canvas, menubar, style);

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

        var style = StyleSheet.GetControlStyle(menuBar);

        if (guiMenuItem.Level > 0)
        {
            canvas.FillRect(globalRect.X, globalRect.Y, globalRect.Width, globalRect.Height, style.Get(menuBar.State).BackgroundColor);
        }

        var menuItemBgColorHover = style.Get(GuiControlState.Hover).CustomElements![GuiMenuBar.MenuItemCustomElementId];
        var menuItemBgColorPressed = style.Get(GuiControlState.Active).CustomElements![GuiMenuBar.MenuItemCustomElementId];

        if (guiMenuItem.IsHovered)
        {
            canvas.FillRect(globalRect.X, globalRect.Y, globalRect.Width, globalRect.Height, menuItemBgColorHover);
        }
        if (guiMenuItem.IsExpanded)
        {
            canvas.FillRect(globalRect.X, globalRect.Y, globalRect.Width, globalRect.Height, menuItemBgColorPressed);
        }

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
        var style = StyleSheet.GetControlStyle(window).Get(window.State);
        DrawControlRect(canvas, window, style);
    }

    public override void DrawText(Canvas canvas, GuiText text)
    {
        var x = text.TextDrawX;
        var y = text.TextDrawY;
        var font = _gui.Font;

        var style = StyleSheet.GetControlStyle(text).Get(text.State);

        canvas.DrawText(font, text.Text, new Vec2(x, y), style.TextColor);
    }

    private readonly GuiStyleSheet _styleSheet;
    private readonly float _textScale = 1.0f;
    private readonly Gui _gui;

}
