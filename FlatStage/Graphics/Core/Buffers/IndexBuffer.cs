using FlatStage.Foundation.BGFX;
using System;

namespace FlatStage.Graphics;

public sealed unsafe class IndexBuffer<T> : Disposable where T : struct
{
    public int IndexCount { get; }

    public IndexBuffer(Memory<T> indices)
    {
        Handle = Bgfx.create_index_buffer(BgfxUtils.MakeRef(indices), (ushort)Bgfx.BufferFlags.None);

        IndexCount = indices.Length;

        GraphicsContext.RegisterRenderResource(this);
    }

    protected override void Free()
    {
        if (Handle.Valid)
        {
            Bgfx.destroy_index_buffer(Handle);
        }
    }

    internal Bgfx.IndexBufferHandle Handle { get; }
}