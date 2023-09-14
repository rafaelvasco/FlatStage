using FlatStage.Graphics;
using System.Collections.Generic;
using System.Text;

namespace FlatStage.Toolkit;
public class GuiSkinFlat : GuiSkin
{
    private float _textScale = 1.0f;
    private const int Border = 1;
    private Gui _gui;

    public GuiSkinFlat(Gui gui)
    {
        _gui = gui;
    }

    public interface IBaseColors
    {
        Color Background { get; }
        Color Border { get; }

    }

    public readonly struct ButtonColors : IBaseColors
    {
        public Color Background { get; init; }
        public Color Border { get; init; }
        public Color Text { get; init; }
    }

    public readonly struct PanelColors : IBaseColors
    {
        public Color Background { get; init; }
        public Color Border { get; init; }
    }

    public readonly struct CheckboxColors : IBaseColors
    {
        public Color Background { get; init; }
        public Color Border { get; init; }
        public Color Indicator { get; init; }
        public Color Text { get; init; }
    }

    public readonly struct SliderColors : IBaseColors
    {
        public Color Background { get; init; }
        public Color Border { get; init; }
        public Color Thumb { get; init; }
        public Color Text { get; init; }
    }

    public readonly struct TextboxColors : IBaseColors
    {
        public Color Background { get; init; }
        public Color Border { get; init; }
        public Color Caret { get; init; }
        public Color Text { get; init; }
    }

    public Dictionary<GuiControlState, ButtonColors> _buttonColors = new()
    {
        {
            GuiControlState.Idle, new ButtonColors()
            {
                Background = new Color(20, 20, 20),
                Border = new Color(60, 60, 60),
                Text = Color.White,
            }

        },
        {
            GuiControlState.Hover, new ButtonColors()
            {
                Background = new Color(30, 30, 30),
                Border = Color.DodgerBlue,
                Text = Color.White,
            }

        },
        {
            GuiControlState.Active, new ButtonColors()
            {
                Background = new Color(5, 5, 5),
                Border = Color.Cyan,
                Text = new Color(150, 150, 150),
            }

        },
        {
            GuiControlState.Disabled, new ButtonColors()
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                Text = Color.LightGray,
            }

        }
    };

    public PanelColors _panelColors = new()
    {
        Background = new Color(20, 20, 20),
        Border = Color.DodgerBlue,
    };

    public Dictionary<GuiControlState, CheckboxColors> _checkboxColors = new()
    {
        {
            GuiControlState.Idle, new CheckboxColors()
            {
                Background = new Color(20, 20, 20),
                Border = new Color(60, 60, 60),
                Indicator = new Color(10, 10, 10),
                Text = Color.White
            }

        },
        {
            GuiControlState.Hover, new CheckboxColors()
            {
                Background = new Color(30, 30, 30),
                Border = Color.DodgerBlue,
                Indicator = new Color(20, 20, 20),
                Text = Color.White
            }
        },
        {
            GuiControlState.Active, new CheckboxColors()
            {
                Background = new Color(5, 5, 5),
                Border = Color.Cyan,
                Indicator = Color.DeepSkyBlue,
                Text = Color.DeepSkyBlue
            }

        },
        {
            GuiControlState.Disabled, new CheckboxColors()
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                Indicator = Color.LightGray,
                Text = Color.Gray
            }

        }
    };

    public Dictionary<GuiControlState, SliderColors> _sliderColors = new()
    {
        {
            GuiControlState.Idle, new SliderColors()
            {
                Background = new Color(20, 20, 20),
                Border = new Color(60, 60, 60),
                Thumb = new Color(10, 10, 10),
                Text = Color.White
            }

        },
        {
            GuiControlState.Hover, new SliderColors()
            {
                Background = new Color(30, 30, 30),
                Border = Color.DodgerBlue,
                Thumb = new Color(20, 20, 20),
                Text = Color.White
            }
        },
        {
            GuiControlState.Active, new SliderColors()
            {
                Background = new Color(5, 5, 5),
                Border = Color.Cyan,
                Thumb = Color.DeepSkyBlue,
                Text = Color.DeepSkyBlue
            }

        },
        {
            GuiControlState.Disabled, new SliderColors()
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                Thumb = Color.LightGray,
                Text = Color.Gray
            }

        }
    };

    public Dictionary<GuiControlState, TextboxColors> _textboxColors = new()
    {
        {
            GuiControlState.Idle, new TextboxColors()
            {
                Background = new Color(20, 20, 20),
                Border = new Color(60, 60, 60),
                Text = Color.White,
                Caret = Color.Transparent
            }

        },
        {
            GuiControlState.Hover, new TextboxColors()
            {
                Background = new Color(30, 30, 30),
                Border = Color.DodgerBlue,
                Text = Color.White,
                Caret = Color.Cyan
            }
        },
        {
            GuiControlState.Active, new TextboxColors()
            {
                Background = new Color(5, 5, 5),
                Border = Color.Cyan,
                Text = Color.DeepSkyBlue,
                Caret = Color.Cyan
            }

        },
        {
            GuiControlState.Focused, new TextboxColors()
            {
                Background = new Color(5, 5, 5),
                Border = Color.Green,
                Text = Color.DeepSkyBlue,
                Caret = Color.Cyan
            }

        },
        {
            GuiControlState.Disabled, new TextboxColors()
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                Text = Color.Gray,
                Caret = Color.Transparent
            }

        }
    };

    public override void DrawButton(Canvas canvas, GuiButton button)
    {
        var colors = _buttonColors[button.State];

        var font = _gui.Font;

        var offset = 4.0f;

        DrawControlRect(canvas, button, colors);

        var labelSize = MeasureText(button.Label);

        var labelX = button.GlobalX + Border + ((button.Width - (2 * Border)) / 2.0f) - (labelSize.X / 2.0f);
        var labelY = button.GlobalY + Border + ((button.Height - (2 * Border)) / 2.0f) - (labelSize.Y / 2.0f);

        canvas.DrawText(font, button.Label, new Vec2(labelX, !button.Active ? labelY : labelY + (offset / 2)), new Vec2(_textScale), colors.Text);

    }

    public override void DrawPanel(Canvas canvas, GuiPanel panel)
    {
        var colors = _panelColors;

        DrawControlRect(canvas, panel, colors);
    }

    private static void DrawControlRect(Canvas canvas, GuiControl control, IBaseColors baseColors)
    {
        DrawBorderedRect(canvas, control.GlobalX, control.GlobalY, control.Width, control.Height, Border, baseColors.Background, baseColors.Border);
    }

    private static void DrawBorderedRect(Canvas canvas, int x, int y, int width, int height, int borderSize, Color bgColor, Color borderColor)
    {
        canvas.DrawRect(x, y, width, height, borderSize, borderColor);
        canvas.FillRect(x + borderSize, y + borderSize, width - (2 * borderSize), height - (2 * borderSize), bgColor);
    }

    public override void DrawCheckbox(Canvas canvas, GuiCheckbox checkbox)
    {
        var colors = _checkboxColors[checkbox.State];

        var font = _gui.Font;

        const int IndicatorOffset = 10;
        const int LabelMargin = 10;

        DrawControlRect(canvas, checkbox, colors);

        var x = checkbox.GlobalX;

        var y = checkbox.GlobalY;

        var indicatorRect = new Rect(x + IndicatorOffset, y + IndicatorOffset, checkbox.Height - (IndicatorOffset * 2), checkbox.Height - (IndicatorOffset * 2));

        var indicatorBgColor = !checkbox.Checked ? colors.Indicator : _checkboxColors[GuiControlState.Active].Indicator;
        var indicatorBorderColor = !checkbox.Checked ? colors.Border : _checkboxColors[GuiControlState.Active].Border;

        DrawBorderedRect(canvas, indicatorRect.X, indicatorRect.Y, indicatorRect.Width, indicatorRect.Height, Border, indicatorBgColor, indicatorBorderColor);

        var label = checkbox.Checked ? "ON" : "OFF";
        var labelSize = MeasureText(label);
        var labelColor = !checkbox.Checked ? colors.Text : _checkboxColors[GuiControlState.Active].Text;

        canvas.DrawText(
            font,
            checkbox.Checked ? "ON" : "OFF",
            new Vec2(
                indicatorRect.X + indicatorRect.Width + LabelMargin,
                y + Border + ((checkbox.Height - (2 * Border)) / 2.0f) - (labelSize.Y / 2)),
            new Vec2(_textScale), labelColor
        );
    }

    public override void DrawSlider(Canvas canvas, GuiSlider slider)
    {
        var colors = _sliderColors[slider.State];

        var x = slider.GlobalX;
        var y = slider.GlobalY;

        DrawControlRect(canvas, slider, colors);
        DrawBorderedRect(canvas, x + slider.ThumbRect.X, y + slider.ThumbRect.Y, slider.ThumbRect.Width, slider.ThumbRect.Height, Border, colors.Thumb, colors.Border);
    }

    public override void DrawTextbox(Canvas canvas, GuiTextbox textbox)
    {
        var colors = _textboxColors[textbox.State];

        var font = _gui.Font;

        var x = textbox.GlobalX;
        var y = textbox.GlobalY;

        var caretWidth = GuiTextbox.CaretWidth;
        var caretHeight = GuiTextbox.CaretHeight;
        var padding = GuiTextbox.Padding;

        var textSelected = textbox.SelectionEndIndex > textbox.SelectionStartIndex;

        DrawControlRect(canvas, textbox, colors);

        var textSizeToCursor = MeasureText(textbox.InternalText, textbox.CaretOffset);
        var fixedTextSize = MeasureText("L");
        var textSize = MeasureText(textbox.InternalText, textbox.SelectionEndIndex - textbox.SelectionStartIndex);

        // Selection Rect
        if (textSelected)
        {
            canvas.FillRect(
                x + Border + padding + (textbox.SelectionStartIndex * fixedTextSize.X),
                y + Border + ((textbox.Height - (2 * Border)) / 2) - (caretHeight / 2),
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
                    x + Border + padding,
                    y + Border + ((textbox.Height - (2 * Border)) / 2) - (fixedTextSize.Y / 2)
                ),
                new Vec2(_textScale),
                colors.Text
            );
        }

        // Cursor
        if (textbox.Focused && textbox.ShowCursor)
        {
            canvas.FillRect(
                x + Border + padding + textSizeToCursor.X,
                y + Border + ((textbox.Height - (2 * Border)) / 2) - (caretHeight / 2),
                caretWidth,
                caretHeight,
                colors.Caret
            );
        }
    }

    public override Vec2 MeasureText(string @string, int stringLength = -1, int startIndex = 0)
    {
        return _gui.Font.MeasureString(@string, stringLength >= 0 ? stringLength : @string.Length, startIndex, _textScale, _textScale);
    }

    public override Vec2 MeasureText(StringBuilder @string, int stringLength = -1, int startIndex = 0)
    {
        return _gui.Font.MeasureString(@string, stringLength >= 0 ? stringLength : @string.Length, startIndex, _textScale, _textScale);
    }
}
