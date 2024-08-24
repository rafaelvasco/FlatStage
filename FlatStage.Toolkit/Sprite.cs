namespace FlatStage.Toolkit;

public class Sprite : GameObject
{
    public Texture Texture { get; set; }

    public Rect Region
    {
        get => _region;
        set
        {
            _region = value;

            if (_region.IsEmpty)
            {
                _region.Width = Texture.Width;
                _region.Height = Texture.Height;
            }

            if (_region.Width > Width || _region.Height > Height)
            {
                Width = _region.Width;
                Height = _region.Height;
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

    public FlipMode FlipMode { get; set; } = FlipMode.None;

    internal Sprite(string id) : base(id)
    {
        Texture = BuiltinContent.Textures.Logo;
        Region = new Rect(0, 0, Texture.Width, Texture.Height);
        Width = Region.Width;
        Height = Region.Height;
    }

    public override void InitFromDefinition(GameObjectDef definition)
    {
        base.InitFromDefinition(definition);

        var spriteDef = (definition as SpriteDef)!;

        Texture = Content.Get<Texture>(spriteDef.Texture);
        Region = spriteDef.Region ?? new Rect(0, 0, Texture.Width, Texture.Height);

        if (definition is { Width: > 0, Height: > 0 })
        {
            Width = definition.Width;
            Height = definition.Height;
        }

    }

    public override void Draw(Canvas canvas)
    {
        base.Draw(canvas);
        canvas.Draw(Texture, new Vec2(GlobalX, GlobalY), Region, Tint, Rotation, Origin, _scale, FlipMode, Depth);
    }

    private void UpdateScale()
    {
        _scale.X = _size.X / Region.Width;
        _scale.Y = _size.Y / Region.Height;
    }

    private Rect _region;
    private Vec2 _size;
    private Vec2 _scale;
}
