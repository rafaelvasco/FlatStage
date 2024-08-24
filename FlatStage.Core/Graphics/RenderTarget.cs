namespace FlatStage;

using BGFX;

public class RenderTarget : Disposable
{
    public Texture Texture => _texture;

    public int Width => _texture.Width;

    public int Height => _texture.Height;

    internal RenderTarget(Bgfx.FrameBufferHandle handle, int width, int height)
    {
        Handle = handle;
        _texture = new Texture("renderTargetTex" + handle.idx, Graphics.GetFrameBufferTexture(handle, 0), width,
            height);
    }

    protected override void Free()
    {
    }

    internal readonly Bgfx.FrameBufferHandle Handle;

    private readonly Texture _texture;
}
