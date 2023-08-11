using System;
using FlatStage.Foundation.BGFX;

namespace FlatStage;

public sealed unsafe class VertexBuffer<T> : Disposable where T : struct
{
    public readonly int VertexCount;

    public VertexBuffer(VertexLayout layout, Memory<T> vertices)
    {
        Handle = Bgfx.create_vertex_buffer(
            BgfxUtils.MakeRef(vertices),
            layout.LayoutData,
            (ushort)Bgfx.BufferFlags.None
        );

        VertexCount = vertices.Length;

        Graphics.RegisterRenderResource(this);
    }

    protected override void Free()
    {
        if (Handle.Valid)
        {
            Bgfx.destroy_vertex_buffer(Handle);
        }
    }

    internal Bgfx.VertexBufferHandle Handle { get; }
}