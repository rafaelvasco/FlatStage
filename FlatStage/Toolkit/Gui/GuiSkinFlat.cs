using FlatStage.ContentPipeline;
using FlatStage.Graphics;
using System.Collections.Generic;

namespace FlatStage.Toolkit;
public class GuiSkinFlat : GuiSkin
{
    private readonly TextureFont _font;
    private float _textScale = 1f;

    public GuiSkinFlat()
    {
        _font = BuiltinContent.Fonts.Monogram;
    }

    public struct ButtonColors
    {
        public Color Background;
        public Color Border;
        public Color Text;
        public Color Shadow;
    }

    public Dictionary<GuiControlState, ButtonColors> _buttonColors = new()
    {
        {
            GuiControlState.Idle, new ButtonColors()
            {
                Background = Color.Green,
                Border = Color.White,
                Text = Color.White,
                Shadow = Color.Black
            }

        },
        {
            GuiControlState.Hover, new ButtonColors()
            {
                Background = Color.Orange,
                Border = Color.White,
                Text = Color.White,
                Shadow = Color.Black
            }

        },
        {
            GuiControlState.Active, new ButtonColors()
            {
                Background = Color.Red,
                Border = Color.White,
                Text = Color.Gray,
                Shadow = Color.Black
            }

        },
        {
            GuiControlState.Disabled, new ButtonColors()
            {
                Background = Color.Gray,
                Border = Color.DarkGray,
                Text = Color.LightGray,
                Shadow = Color.Black
            }

        }
    };

    public override void DrawButton(Canvas canvas, GuiButton button)
    {
        var colors = _buttonColors[button.State];

        canvas.FillRect(button.GlobalX, button.GlobalY, button.Width, button.Height + 4.0f, colors.Shadow);
        canvas.FillRect(button.GlobalX, button.GlobalY, button.Width, button.Height, colors.Background);
        canvas.DrawRect(button.GlobalX, button.GlobalY, button.Width, button.Height, 1, colors.Border);

        var labelSize = _font.MeasureString(button.Label, _textScale, _textScale);

        var labelX = button.GlobalX + (button.Width / 2.0f) - (labelSize.X / 2.0f);
        var labelY = button.GlobalY + (button.Height / 2.0f) - (labelSize.Y / 2.0f);

        canvas.DrawText(_font, button.Label, new Vec2(labelX, labelY), new Vec2(_textScale), colors.Text);

    }
}
