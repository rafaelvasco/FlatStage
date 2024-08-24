using BGFX;
using System.Runtime.InteropServices;

namespace FlatStage;

public sealed unsafe class VertexLayout : Disposable
{
    public IReadOnlyList<VertexAttribute> Attributes { get; }

    public int Stride { get; }

    public VertexLayout(params VertexAttribute[] attributes)
    {
        LayoutData = (Bgfx.VertexLayout*)Marshal.AllocHGlobal(sizeof(Bgfx.VertexLayout)).ToPointer();

        Bgfx.vertex_layout_begin(LayoutData, Bgfx.get_renderer_type());

        foreach (var attribute in attributes)
        {
            Bgfx.vertex_layout_add(
                LayoutData,
                (Bgfx.Attrib)attribute.AttributeRole,
                attribute.Count,
                (Bgfx.AttribType)attribute.AttributeType,
                attribute.Normalized,
                attribute.AsInt
            );
        }

        Bgfx.vertex_layout_end(LayoutData);
        Handle = Bgfx.create_vertex_layout(LayoutData);

        Attributes = new List<VertexAttribute>(attributes);
        Stride = LayoutData->stride;

        Graphics.RegisterRenderResource(this);
    }

    protected override void Free()
    {
        if (LayoutData == null) return;
        Marshal.FreeHGlobal((IntPtr)LayoutData);
        LayoutData = null;
    }

    internal readonly Bgfx.VertexLayoutHandle Handle;
    internal Bgfx.VertexLayout* LayoutData { get; private set; }
}
