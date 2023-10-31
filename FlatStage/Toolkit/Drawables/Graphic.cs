using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public abstract class Graphic
{
    public string Name { get; internal set; }

    public float X { get; set; }

    public float Y { get; set; }

    public bool DebugDraw { get; set; } = false;

    public Vec2 Origin
    {
        get => _origin;
        set
        {
            _origin = new Vec2(MathUtils.Clamp(value.X, 0f, 1f), MathUtils.Clamp(value.Y, 0f, 1f));
        }
    }

    public float Depth { get; set; } = 0f;

    public float Rotation
    {
        get => _rotation;
        set
        {
            _rotation = MathUtils.Clamp(value, -MathUtils.HalfPi, MathUtils.Pi);
        }
    }

    public bool Visible { get; set; } = true;

    public Color Tint { get; set; } = Color.White;

    public abstract float Width { get; set; }

    public abstract float Height { get; set; }

    public virtual RectF Bounds => new(X, Y, Width, Height);

    protected Graphic(string name)
    {
        Name = name;
    }

    public abstract void Draw(Canvas canvas, float offsetX, float offsetY);

    internal static Graphic CreateFromDefinition(GraphicDef definition)
    {
        if (definition is SpriteDef spriteDef)
        {
            var sprite = new Sprite(spriteDef.Name)
            {
                DebugDraw = definition.DebugDraw
            };
            sprite.InitFromDefinition(spriteDef);
            return sprite;
        }
        else if (definition is TextDef textDef)
        {
            var text = new Text(textDef.Name)
            {
                DebugDraw = definition.DebugDraw
            };
            text.InitFromDefinition(textDef);
            return text;
        }

        FlatException.Throw("Invalid Graphic Definition");
        return null!;
    }

    internal abstract void InitFromDefinition(GraphicDef definition);

    private float _rotation;
    private Vec2 _origin = new(0.5f, 0.5f);

}
