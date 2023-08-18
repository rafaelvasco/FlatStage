namespace FlatStage.Graphics;

using Foundation.BGFX;

internal struct RenderTargetProps : IDefinitionData
{
    public int Width = 0;
    public int Height = 0;
    public const Bgfx.TextureFormat Format = Bgfx.TextureFormat.BGRA8;

    public const Bgfx.SamplerFlags Flags = Bgfx.SamplerFlags.UClamp | Bgfx.SamplerFlags.VClamp |
                                           Bgfx.SamplerFlags.MinPoint | Bgfx.SamplerFlags.MagPoint;

    public override readonly string ToString()
    {
        return $"[Width: {Width}, Height: {Height}]";
    }

    public RenderTargetProps()
    {
    }

    public readonly bool IsValid()
    {
        return Width > 0 || Height > 0;
    }
}

public class RenderTarget : Disposable
{
    public Texture Texture => _texture;

    public int Width => _texture.Width;

    public int Height => _texture.Height;

    internal RenderTarget(Bgfx.FrameBufferHandle handle, int width, int height)
    {
        Handle = handle;
        _texture = new Texture("renderTargetTex" + handle.idx, GraphicsContext.GetFrameBufferTexture(handle, 0), width,
            height);
    }

    protected override void Free()
    {
    }

    internal readonly Bgfx.FrameBufferHandle Handle;

    private readonly Texture _texture;
}