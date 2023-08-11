using System.Runtime.CompilerServices;

namespace FlatStage;

public struct Vertex2D
{
    public uint Color;
    public float X;
    public float Y;
    public float Z;
    public float U;
    public float V;

    public static readonly VertexLayout VertexLayout;

    public static readonly int Stride;

    public readonly override string ToString()
    {
        return $"X: {X}, Y: {Y}, Z: {Z}, U: {U}, V: {V}";
    }

    public Vertex2D(uint color, float x, float y, float z, float u, float v)
    {
        Color = color;
        X = x;
        Y = y;
        Z = z;
        U = u;
        V = v;
    }

    static Vertex2D()
    {
        VertexLayout = new VertexLayout(
            new VertexAttribute(VertexAttributeRole.Color0, 4, VertexAttributeType.UInt8, true, false),
            new VertexAttribute(VertexAttributeRole.Position, 3, VertexAttributeType.Float, false, false),
            new VertexAttribute(VertexAttributeRole.TexCoord0, 2, VertexAttributeType.Float, false, false)
        );

        Stride = Unsafe.SizeOf<Vertex2D>();
    }
}