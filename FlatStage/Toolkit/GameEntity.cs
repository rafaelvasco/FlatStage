using FlatStage.Graphics;

namespace FlatStage.Toolkit;
public class GameEntity : BaseGameEntity
{
    public string Name { get; init; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Width { get; set; }

    public float Height { get; set; }

    public float GlobalX => Parent?.GlobalX + X ?? X;

    public float GlobalY => Parent?.GlobalY + Y ?? Y;

    public bool DebugDraw { get; set; } = false;

    public GameEntity? Parent { get; internal set; }

    public float Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            if (_rotation is > MathUtils.TwoPi or < -MathUtils.TwoPi)
            {
                _rotation = 0f;
            }
        }
    }

    public RectF Bounds => new(X, Y, Width, Height);

    public RectF GlobalBounds => new(GlobalX, GlobalY, Width, Height);

    public bool Visible { get; set; } = true;

    internal Stage Stage { get; }

    public Graphic? MainGraphic { get; private set; }

    internal static GameEntity FromDefinition(Stage stage, GameObjectDef definition, GameEntity? parent = null)
    {
        var gameObject = new GameEntity(definition.Name, stage, parent)
        {
            Visible = definition.VisibleAtStart,
            X = definition.X,
            Y = definition.Y,
            Width = definition.Width,
            Height = definition.Height,
            Rotation = definition.Rotation,
        };

        for (int i = 0; i < definition.Graphics?.Length; ++i)
        {
            var graphicDef = definition.Graphics[i];

            var graphic = Graphic.CreateFromDefinition(graphicDef);
            gameObject.AddGraphic(graphic);
        }

        for (int i = 0; i < definition.Children?.Length; ++i)
        {
            var childDef = definition.Children[i];

            var child = FromDefinition(stage, childDef, gameObject);
            gameObject.AddChild(child);
        }

        return gameObject;

    }

    private GameEntity(string name, Stage stage, GameEntity? parent = null)
    {
        Stage = stage;
        Name = name;
        Parent = parent;

        Stage.Register(this);
    }

    public void AddChild(GameEntity child)
    {
        if (_children == null)
        {
            _children = new();
            _childrenMap = new();
        }

        _children.Add(child);
        _childrenMap!.Add(child.Name, child);
    }

    public void AddGraphic(Graphic graphic)
    {
        if (_graphics == null)
        {
            _graphics = new();
            _graphicsMap = new();
        }

        _graphics.Add(graphic);
        _graphicsMap!.Add(graphic.Name, graphic);
        MainGraphic ??= graphic;
    }

    internal void Update(float dt)
    {

    }

    internal void Draw(Canvas canvas)
    {
        if (!Visible)
        {
            return;
        }

        if (DebugDraw)
        {
            canvas.DrawRect(GlobalBounds.X, GlobalBounds.Y, GlobalBounds.Width, GlobalBounds.Height, 1, Color.Fuchsia);
        }

        if (_graphics != null)
        {
            var list = _graphics.ReadOnlySpan;
            foreach (var graphic in list)
            {
                if (graphic.Visible)
                {
                    graphic.Draw(canvas, GlobalX, GlobalY);
                }
            }
        }

        if (_children != null)
        {
            var list = _children.ReadOnlySpan;

            foreach (var child in list)
            {
                child.Draw(canvas);
            }
        }

    }

    protected Bounds _boundingBox;
    private float _rotation;

    private FastList<Graphic>? _graphics;
    private FastList<GameEntity>? _children;

    private FastDictionary<string, GameEntity>? _childrenMap;
    private FastDictionary<string, Graphic>? _graphicsMap;

}
