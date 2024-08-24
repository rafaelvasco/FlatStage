using BGFX;
namespace FlatStage;

public sealed unsafe class IndexBuffer<T> : Disposable where T : struct
{
    public int IndexCount { get; }

    public IndexBuffer(Memory<T> indices)
    {
        Handle = Bgfx.create_index_buffer(BgfxUtils.MakeRef(indices), (ushort)Bgfx.BufferFlags.None);

        IndexCount = indices.Length;

        Graphics.RegisterRenderResource(this);
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
