using System;

using FlatStage.Content;
using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

internal struct TextureProps : IDefinitionData
{
    public Memory<byte> Data = Memory<byte>.Empty;
    public int Width = 0;
    public int Height = 0;
    public int BytesPerPixel = 4;
    public const Bgfx.TextureFormat Format = Bgfx.TextureFormat.BGRA8;

    public const Bgfx.SamplerFlags Flags = Bgfx.SamplerFlags.UClamp | Bgfx.SamplerFlags.VClamp |
                                           Bgfx.SamplerFlags.MinPoint | Bgfx.SamplerFlags.MagPoint;

    public TextureProps()
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

public class Texture : Asset, IEquatable<Texture>
{
    public readonly int Width;

    public readonly int Height;

    internal float TexelWidth { get; private set; }
    internal float TexelHeight { get; private set; }

    internal Bgfx.TextureHandle Handle { get; }

    internal Texture(string id, Bgfx.TextureHandle handle, int width, int height) : base(id)
    {
        Handle = handle;
        Width = width;
        Height = height;
        TexelWidth = 1 / (float)Width;
        TexelHeight = 1 / (float)Height;
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

    public bool Equals(Texture? other)
    {
        return other != null && Handle.idx == other.Handle.idx;
    }

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as Texture);
    }

    public override int GetHashCode()
    {
        return Handle.idx.GetHashCode();
    }
}