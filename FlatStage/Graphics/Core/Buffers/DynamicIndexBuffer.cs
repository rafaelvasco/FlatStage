using FlatStage.Foundation.BGFX;
using System;

namespace FlatStage.Graphics;

public unsafe class DynamicIndexBuffer<T> : Disposable where T : struct
{
    public int IndexCount { get; private set; }

    public DynamicIndexBuffer(Memory<T>? indices = null)
    {
        if (indices.HasValue)
        {
            Handle = Bgfx.create_dynamic_index_buffer_mem(
                BgfxUtils.MakeRef(indices.Value),
                (ushort)Bgfx.BufferFlags.AllowResize
            );

            IndexCount = indices.Value.Length;
        }
        else
        {
            Handle = Bgfx.create_dynamic_index_buffer(
                0,
                (ushort)Bgfx.BufferFlags.AllowResize
            );

            IndexCount = 0;
        }

        GraphicsContext.RegisterRenderResource(this);
    }

    public void SetData(T[] data, int startIndex = 0)
    {
        Bgfx.update_dynamic_index_buffer(
            Handle,
            (uint)startIndex,
            BgfxUtils.MakeRef(data)
        );

        IndexCount = data.Length;
    }

    protected override void Free()
    {
        if (Handle.Valid)
        {
            Bgfx.destroy_dynamic_index_buffer(Handle);
        }
    }

    internal Bgfx.DynamicIndexBufferHandle Handle { get; }
}