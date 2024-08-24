namespace FlatStage;
public struct Bounds
{
    public readonly RectF Rect => new(minX, minY, maxX - minX, maxY - minY);

    public Bounds(RectF rect)
    {
        minX = rect.Left;
        minY = rect.Top;
        maxX = rect.Right;
        maxY = rect.Bottom;
    }

    public void AddPoint(Vec2 point)
    {
        minX = MathUtils.Min(point.X, minX);
        maxX = MathUtils.Max(point.X, maxX);
        minY = MathUtils.Min(point.Y, minY);
        maxY = MathUtils.Max(point.Y, maxY);
    }

    public void AddRect(RectF rect)
    {
        AddPoint(rect.TopLeft);
        AddPoint(rect.TopRight);
        AddPoint(rect.BottomLeft);
        AddPoint(rect.BottomRight);
    }

    public void Set(float x, float y, float w, float h)
    {
        minX = x;
        minY = y;
        maxX = x + w;
        maxY = y + h;
    }

    public static Bounds operator +(Bounds a, Bounds b)
    {
        return new Bounds()
        {
            minX = MathUtils.Min(a.minX, b.minX),
            minY = MathUtils.Min(a.maxY, b.maxY),
            maxX = MathUtils.Max(a.maxX, b.maxX),
            maxY = MathUtils.Max(a.maxY, b.maxY)
        };
    }

    private float minX = float.MaxValue;
    private float minY = float.MaxValue;
    private float maxX = float.MinValue;
    private float maxY = float.MinValue;

}
