namespace FlatStage.Toolkit;

public class Text : GameObject
{
    public TextureFont Font { get; set; }

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
            }

            UpdateScale();
        }
    }

    public override float Width
    {
        get => _size.X;
        set
        {
            _size.X = value;
            UpdateScale();
        }
    }

    public override float Height
    {
        get => _size.Y;
        set
        {
            _size.Y = value;
            UpdateScale();
        }
    }


    internal Text(string id) : base(id)
    {
        Font = BuiltinContent.Fonts.Monogram;
        String = "Hello World!";
    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        var textDef = (definition as TextDef)!;
        String = textDef.Label;

        if (definition is { Width: > 0, Height: > 0 })
        {
            Width = definition.Width;
            Height = definition.Height;
        }
    }

    public override void Draw(Canvas canvas)
    {
        base.Draw(canvas);
        canvas.DrawText(Font, _text.ReadOnlySpan, new Vec2(GlobalX - (_textWidth * _scale.X * Origin.X), GlobalY - (_textHeight * _scale.Y * Origin.Y)), _scale, Tint);
    }

    private void UpdateScale()
    {
        _scale.X = _size.X / (_textWidth > 0 ? _textWidth : _size.X);
        _scale.Y = _size.Y / (_textHeight > 0 ? _textHeight : _size.Y);
    }



    private float _textWidth;
    private float _textHeight;

    private Vec2 _size;
    private Vec2 _scale;

    private readonly StringBuffer _text = new();
}
