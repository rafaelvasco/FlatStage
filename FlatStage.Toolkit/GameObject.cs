namespace FlatStage.Toolkit;

public class GameObject : IEquatable<GameObject>
{
    internal static int SUid;

    internal int UId { get; }

    public string Id { get; internal set; }

    public string? Name { get; set; }

    public bool HasBehaviors { get; internal set; }

    public bool Equals(GameObject? other)
    {
        return UId.Equals(other?.UId);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GameObject);
    }

    public override int GetHashCode()
    {
        return UId.GetHashCode();
    }

    public float X { get; set; }

    public float Y { get; set; }

    public Vec2 Origin { get; set; } = Vec2.Half;

    public float GlobalX => Parent?.GlobalX + X ?? X;
    public float GlobalY => Parent?.GlobalY + Y ?? Y;

    public virtual float Width { get; set; }

    public virtual float Height { get; set; }

    public float Depth { get; set; }

    public int TreeDepth { get; internal set; }

    public RectF Bounds => new(X - (Width * Origin.X), Y - (Height * Origin.Y), Width, Height);

    public RectF GlobalBounds => new(GlobalX - (Width * Origin.X), GlobalY - (Height * Origin.Y), Width, Height);

    public bool Visible { get; set; } = true;

    public Color Tint { get; set; } = Color.White;

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

    public bool DrawBoundaries { get; set; }

    public GameObject? Parent
    {
        get => _parent;
        private set
        {
            _parent = value;
            RecalculateTreeDepth();
        }
    }

    public FastList<GameObject>? Children { get; private set; }

    public T Find<T>(string id) where T : GameObject
    {
        var result = Find<T>(id, this);

        if (result != null)
        {
            return result;
        }

        FlatException.Throw($"Could not find child GameObject: {id}");
        return null!;
    }

    private static T? Find<T>(string id, GameObject parent) where T : GameObject
    {
        if (parent.Id.Equals(id))
        {
            return (parent as T)!;
        }

        if (parent.Children == null) return null;

        var children = parent.Children!.ReadOnlySpan;

        foreach (var child in children)
        {
            var result = Find<T>(id, child);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void Add(GameObject gameObject)
    {
        Children ??= new FastList<GameObject>();

        if (Children.Contains(gameObject)) return;

        gameObject.Parent?.Remove(this);

        Children.Add(gameObject);

        gameObject.Parent = this;

        AfterAdd(gameObject);
    }

    public void Remove(GameObject gameObject)
    {
        if (Children != null && Children.Contains(gameObject))
        {
            gameObject.Parent = null;
            Children?.Remove(gameObject);
        }

        AfterRemove(gameObject);
    }

    protected virtual void AfterAdd(GameObject gameObject){}

    protected virtual void AfterRemove(GameObject gameObject){}


    public void Kill()
    {

    }

    public GameObject(string id)
    {
        UId = ++SUid;
        Id = id;
        Name = id;
    }

    public virtual void InitFromDefinition(GameObjectDef definition)
    {
        X = definition.X;
        Y = definition.Y;
        Origin = definition.Origin;
        Depth = definition.Depth;
        Visible = definition.VisibleOnStart;
        Tint = definition.Tint;
        Rotation = definition.Rotation;
    }

    public virtual void Update(float dt)
    {
        if (Children == null) return;

        for (var i = 0; i < Children.Count; ++i)
        {
            Children[i].Update(dt);
        }
    }

    public virtual void Draw(Canvas canvas)
    {
        if (!Visible)
        {
            return;
        }

        if (DrawBoundaries)
        {
            canvas.DrawRect(GlobalBounds.X, GlobalBounds.Y, GlobalBounds.Width, GlobalBounds.Height, 1, Color.Fuchsia);
        }

        if (Children == null) return;

        for (var i = 0; i < Children.Count; ++i)
        {
            Children[i].Draw(canvas);
        }
    }

    private void RecalculateTreeDepth()
    {
        TreeDepth = 0;
        RecalculateTreeDepth(this);
    }

    private void RecalculateTreeDepth(GameObject gameObject)
    {
        while (true)
        {
            if (gameObject.Parent == null) return;
            TreeDepth++;
            gameObject = gameObject.Parent;
        }
    }

    private float _rotation;
    private GameObject? _parent;
}
