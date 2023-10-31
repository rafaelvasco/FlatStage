using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

public enum VertexAttributeRole
{
    Position = Bgfx.Attrib.Position,
    Color0 = Bgfx.Attrib.Color0,
    Color1 = Bgfx.Attrib.Color1,
    Color2 = Bgfx.Attrib.Color2,
    Color3 = Bgfx.Attrib.Color3,
    TexCoord0 = Bgfx.Attrib.TexCoord0,
    TexCoord1 = Bgfx.Attrib.TexCoord1,
    TexCoord2 = Bgfx.Attrib.TexCoord2,
    TexCoord3 = Bgfx.Attrib.TexCoord3,
    TexCoord4 = Bgfx.Attrib.TexCoord4,
    TexCoord5 = Bgfx.Attrib.TexCoord5,
    TexCoord6 = Bgfx.Attrib.TexCoord6,
    TexCoord7 = Bgfx.Attrib.TexCoord7
}

public enum VertexAttributeType
{
    Float = Bgfx.AttribType.Float,
    UInt8 = Bgfx.AttribType.Uint8
}

public struct VertexAttribute
{
    public VertexAttributeRole AttributeRole { get; }

    public byte Count { get; }

    public VertexAttributeType AttributeType { get; }

    public bool Normalized { get; }

    public bool AsInt { get; }

    public int Size { get; }

    public VertexAttribute(VertexAttributeRole role, byte count, VertexAttributeType type, bool normalized, bool asInt)
    {
        AttributeRole = role;
        Count = count;
        AttributeType = type;
        Normalized = normalized;
        AsInt = asInt;
        Size = CalculateSizeInBytes(AttributeType, Count);
    }

    private static int CalculateSizeInBytes(VertexAttributeType type, int count)
    {
        Bgfx.AttribType internalType = (Bgfx.AttribType)type;
        return internalType switch
        {
            Bgfx.AttribType.Float => 4 * count,
            Bgfx.AttribType.Uint8 => 1 * count,
            _ => 0
        };
    }
}