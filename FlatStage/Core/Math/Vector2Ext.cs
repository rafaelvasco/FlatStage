
namespace FlatStage;

public static class Vec2Ext
{
    public static void Deconstruct(this Vec2 v, out float x, out float y)
    {
        x = v.X;
        y = v.Y;
    }
}
