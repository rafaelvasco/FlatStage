using FlatStage.Content;
using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class Sprite : Graphic
{
    public Texture Texture { get; set; } = BuiltinContent.Textures.Logo;

    public Rect Region { get; set; }

    public FlipMode FlipMode { get; set; } = FlipMode.None;

    public override float Width
    {
        get => _size.X;
        set
        {
            _size.X = value;
            _scale.X = _size.X / Region.Width;
        }
    }

    public override float Height
    {
        get => _size.Y;
        set
        {
            _size.Y = value;
            _scale.Y = _size.Y / Region.Height;
        }
    }

    public override RectF Bounds => new(X - (Width * Origin.X), Y - (Height * Origin.Y), Width, Height);

    internal Sprite(string name) : base(name) { }

    public override void Draw(Canvas canvas, float offsetX, float offsetY)
    {
        canvas.Draw(Texture, new Vec2(offsetX + X, offsetY + Y), Region, Tint, Rotation, Origin, _scale, FlipMode, Depth);

        if (DebugDraw)
        {
            canvas.DrawRect(offsetX + Bounds.X, offsetY + Bounds.Y, Bounds.Width, Bounds.Height, 1, Color.LimeGreen);
        }
    }

    internal override void InitFromDefinition(GraphicDef definition)
    {
        if (definition is SpriteDef spriteDef)
        {
            Texture = Assets.Get<Texture>(spriteDef.Texture);
            X = spriteDef.X;
            Y = spriteDef.Y;
            Region = spriteDef.Region ?? new Rect(0, 0, Texture.Width, Texture.Height);
            Depth = spriteDef.Depth;

            if (spriteDef.Width >= 0 && spriteDef.Height >= 0)
            {
                Width = spriteDef.Width;
                Height = spriteDef.Height;
            }
            else
            {
                Width = Region.Width * spriteDef.ScaleX;
                Height = Region.Height * spriteDef.ScaleY;
            }

            Origin = spriteDef.Origin;
            Visible = spriteDef.VisibleAtStart;
            Rotation = spriteDef.Rotation;
            Tint = spriteDef.ColorTint;

            FlipMode = spriteDef.FlipMode;
        }
    }

    private Vec2 _size;
    private Vec2 _scale;
}
