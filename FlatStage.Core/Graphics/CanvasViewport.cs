﻿namespace FlatStage;
public class CanvasViewport(int width, int height)
{
    public RenderTarget RenderTarget { get; } = Graphics.CreateRenderTarget(width, height);

    public Color BackgroundColor { get; set; } = Color.Black;

    public Texture Texture => RenderTarget.Texture;

    public int Width => RenderTarget.Width;

    public int Height => RenderTarget.Height;

    public Matrix ProjectionMatrix => _projMatrix;

    private Matrix _projMatrix = Matrix.CreateOrthographicOffCenter(0f, width, height, 0f,
        -1.0f, 1.0f);

    public void SetSource(Rect rect)
    {
        _projMatrix = Matrix.CreateOrthographicOffCenter(rect.Left, rect.Right, rect.Bottom, rect.Top,
           -1.0f, 1.0f);
    }
}
