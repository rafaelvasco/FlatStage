using System;
using System.Runtime.CompilerServices;
using FlatStage.Foundation.BGFX;

namespace FlatStage;

public readonly unsafe struct TransientVertexBuffer<T> where T : struct
{
    internal readonly Bgfx.TransientVertexBuffer Handle;

    public TransientVertexBuffer(Span<T> vertices, int vertexByteSize, VertexLayout layout)
    {
        var handle = new Bgfx.TransientVertexBuffer();

        Bgfx.alloc_transient_vertex_buffer(&handle, (uint)vertices.Length, layout.LayoutData);

        var dataSize = vertices.Length * vertexByteSize;

        Unsafe.CopyBlockUnaligned(handle.data, Unsafe.AsPointer(ref vertices[0]), (uint)dataSize);

        Handle = handle;
    }
}