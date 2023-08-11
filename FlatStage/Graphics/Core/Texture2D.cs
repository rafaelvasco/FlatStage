using System;
using FlatStage.Foundation.BGFX;

namespace FlatStage;

internal struct Texture2DProps : IDefinitionData
{
    public Memory<byte> Data = Memory<byte>.Empty;
    public int Width = 0;
    public int Height = 0;
    public int BytesPerPixel = 4;
    public const Bgfx.TextureFormat Format = Bgfx.TextureFormat.BGRA8;

    public const Bgfx.SamplerFlags Flags = Bgfx.SamplerFlags.UClamp | Bgfx.SamplerFlags.VClamp |
                                           Bgfx.SamplerFlags.MinPoint | Bgfx.SamplerFlags.MagPoint;

    public Texture2DProps()
    {
    }

    public override string ToString()
    {
        return $"[Width: {Width}, Height: {Height}; Data: {Data.Length}, BytesPerPixel: {BytesPerPixel}]";
    }

    public readonly bool IsValid()
    {
        return Width > 0 || Height > 0 || !Data.IsEmpty;
    }
}

public class Texture2D : Asset, IEquatable<Texture2D>
{
    public readonly int Width;

    public readonly int Height;


    internal Bgfx.TextureHandle Handle { get; }

    internal Texture2D(string id, Bgfx.TextureHandle handle, int width, int height) : base(id)
    {
        Handle = handle;
        Width = width;
        Height = height;
    }

    public unsafe void SetData(Memory<byte> pixels, int targetX = 0, int targetY = 0, int targetW = 0, int targetH = 0)
    {
        var data = BgfxUtils.MakeRef(pixels);

        if (targetW == 0)
        {
            targetW = Width;
        }

        if (targetH == 0)
        {
            targetH = Height;
        }

        Bgfx.update_texture_2d(Handle, 0, 0, (ushort)targetX, (ushort)targetY, (ushort)targetW, (ushort)targetH, data,
            ushort.MaxValue);
    }

    protected override void Free()
    {
        Bgfx.destroy_texture(Handle);
    }

    public bool Equals(Texture2D? other)
    {
        return other != null && Handle.idx == other.Handle.idx;
    }

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as Texture2D);
    }

    public override int GetHashCode()
    {
        return Handle.idx.GetHashCode();
    }
}