using System;
using FlatStage.Foundation.BGFX;

namespace FlatStage;

public unsafe class DynamicVertexBuffer<T> : Disposable where T : struct
{
    public int VertexCount { get; private set; }

    public DynamicVertexBuffer(VertexLayout layout, Memory<T>? vertices = null)
    {
        if (vertices.HasValue)
        {
            Handle = Bgfx.create_dynamic_vertex_buffer_mem(
                BgfxUtils.MakeRef(vertices.Value),
                layout.LayoutData,
                (ushort)Bgfx.BufferFlags.AllowResize
            );

            VertexCount = vertices.Value.Length;
        }
        else
        {
            Handle = Bgfx.create_dynamic_vertex_buffer(
                0,
                layout.LayoutData,
                (ushort)Bgfx.BufferFlags.AllowResize
            );

            VertexCount = 0;
        }

        Graphics.RegisterRenderResource(this);
    }

    public void SetData(Memory<T> data, int startVertex = 0)
    {
        Bgfx.update_dynamic_vertex_buffer(
            Handle,
            (uint)startVertex,
            BgfxUtils.MakeRef(data)
        );

        VertexCount = data.Length;
    }

    public void SetData(Span<T> data, int startVertex = 0)
    {
        Bgfx.update_dynamic_vertex_buffer(
            Handle,
            (uint)startVertex,
            BgfxUtils.MakeRef(data)
        );

        VertexCount = data.Length;
    }


    protected override void Free()
    {
        if (Handle.Valid)
        {
            Bgfx.destroy_dynamic_vertex_buffer(Handle);
        }
    }

    internal Bgfx.DynamicVertexBufferHandle Handle { get; }
}