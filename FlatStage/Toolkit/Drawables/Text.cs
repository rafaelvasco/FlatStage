using FlatStage.Content;
using FlatStage.Graphics;
using System;

namespace FlatStage.Toolkit;
public class Text : Graphic
{
    public TextureFont Font { get; set; } = BuiltinContent.Fonts.Monogram;

    public ReadOnlySpan<char> String
    {
        get => _text.ReadOnlySpan;
        set
        {
            _text.Clear();
            _text.Append(value);

            if (_text.Length > 0)
            {
                var size = Font.MeasureString(value);

                _textWidth = size.X;
                _textHeight = size.Y;

                if (_textWidth > Width)
                {
                    Width = _textWidth;
                }

                if (_textHeight > Height)
                {
                    Height = _textHeight;
                }
            }
            else
            {
                _textWidth = _textHeight = 0;
                Width = 0;
                Height = 0;
            }
        }
    }

    public override float Width
    {
        get => _size.X;
        set
        {
            if (_textWidth > 0)
            {
                _size.X = value;
                _scale.X = _size.X / _textWidth;
            }
            else
            {
                _size.X = 0;
                _scale.X = 0;
            }

        }
    }

    public override float Height
    {
        get => _size.Y;
        set
        {
            if (_textHeight > 0)
            {
                _size.Y = value;
                _scale.Y = _size.Y / _textHeight;
            }
            else
            {
                _size.Y = 0;
                _scale.Y = 0;
            }

        }
    }

    public override RectF Bounds => new(X - (Width * Origin.X), Y - (Height * Origin.Y), Width, Height);

    public Text(string name) : base(name)
    {
    }

    internal override void InitFromDefinition(GraphicDef definition)
    {
        if (definition is TextDef textDef)
        {
            if (textDef.Font != null)
            {
                Font = Assets.Get<TextureFont>(textDef.Font);
            }

            X = textDef.X;
            Y = textDef.Y;

            _scale = new Vec2(textDef.ScaleX, textDef.ScaleY);

            String = textDef.Text;

            if (textDef.Width >= 0 && textDef.Height >= 0)
            {
                Width = textDef.Width;
                Height = textDef.Height;
            }
            else
            {
                Width = _textWidth * textDef.ScaleX;
                Height = _textHeight * textDef.ScaleY;
            }

            Depth = textDef.Depth;
            Origin = textDef.Origin;
            Visible = textDef.VisibleAtStart;
            Rotation = textDef.Rotation;
            Tint = textDef.ColorTint;
        }
    }

    public override void Draw(Canvas canvas, float offsetX, float offsetY)
    {
        if (_text.Length > 0)
        {
            canvas.DrawText(Font, _text.ReadOnlySpan, new Vec2(offsetX + X - (_textWidth * _scale.X * Origin.X), offsetY + Y - (_textHeight * _scale.Y * Origin.Y)), _scale, Tint);

            if (DebugDraw)
            {
                canvas.DrawRect(offsetX + Bounds.X, offsetY + Bounds.Y, Bounds.Width, Bounds.Height, 1, Color.LimeGreen);
            }
        }
    }

    private float _textWidth;
    private float _textHeight;

    private Vec2 _size;
    private Vec2 _scale;

    private readonly StringBuffer _text = new();

}
