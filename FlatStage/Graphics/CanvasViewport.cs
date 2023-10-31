namespace FlatStage.Graphics;
public class CanvasViewport
{
    public RenderTarget RenderTarget { get; private set; }

    public Color BackgroundColor { get; set; } = Color.Black;

    public Texture Texture => RenderTarget.Texture;

    public int Width => RenderTarget.Width;

    public int Height => RenderTarget.Height;

    public Matrix ProjectionMatrix => _projMatrix;

    private Matrix _projMatrix;

    public CanvasViewport(int width, int height)
    {
        RenderTarget = GraphicsContext.CreateRenderTarget(width, height);

        _projMatrix = Matrix.CreateOrthographicOffCenter(0f, width, height, 0f,
           -1.0f, 1.0f);
    }

    public void SetSource(Rect rect)
    {
        _projMatrix = Matrix.CreateOrthographicOffCenter(rect.Left, rect.Right, rect.Bottom, rect.Top,
           -1.0f, 1.0f);
    }
}
