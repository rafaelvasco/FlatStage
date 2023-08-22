using FlatStage.ContentPipeline;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace FlatStage.Graphics;

[Flags]
public enum FlipMode
{
    None = 0,
    Horizontal = 1,
    Vertical = 2,
}

public class Canvas2D
{

    public Canvas2D(int maxQuads = 2048)
    {
        if (!Calc.IsPowerOfTwo(maxQuads))
        {
            maxQuads = Calc.NextPowerOfTwo(maxQuads);
        }

        GraphicsContext.BackBufferSizeChanged = ApplyRenderViewArea;

        _quadBatcher = new QuadBatcher(maxQuads);

        _viewport = Rect.Empty;

        _defaultShader = Content.Get<ShaderProgram>("canvas2d", embeddedAsset: true);

        _defaultTexture = Content.Get<Texture>("stagelogo", embeddedAsset: true);

        _currentShader = _defaultShader;

        _beginCalled = false;

        _blendState = BlendState.AlphaBlend;

        _samplerState = SamplerState.PointClamp;

        _rasterizerState = RasterizerState.CullCounterClockWise;

        ApplyRenderViewArea(new Size(GraphicsContext.BackbufferWidth, GraphicsContext.BackbufferHeight));
    }

    public void Begin(
        BlendState? blendState = null,
        SamplerState? samplerState = null,
        RasterizerState? rasterizerState = null,
        Matrix? transformMatrix = null,
        ShaderProgram? shader = null
    )
    {
        if (_beginCalled)
        {
            throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
        }

        _beginCalled = true;

        _blendState = blendState ?? BlendState.AlphaBlend;
        _samplerState = samplerState ?? SamplerState.PointWrap;
        _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockWise;
        _currentShader = shader ?? _defaultShader;
        _transformMatrix = transformMatrix ?? Matrix.Identity;
    }

    public void End()
    {
        if (!_beginCalled)
        {
            throw new InvalidOperationException("End was called, but Begin has not yet been called.");
        }

        Flush();

        _drawCalls = 0;

        _beginCalled = false;
    }

    void CheckValid(Texture texture)
    {
        if (texture == null)
            throw new ArgumentNullException("texture");
        if (!_beginCalled)
            throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
    }

    void CheckValid(TextureFont font, in CharSource? text)
    {
        if (font == null)
            throw new ArgumentNullException("font");
        if (text == null)
            throw new ArgumentNullException("text");
        if (!_beginCalled)
            throw new InvalidOperationException("DrawText was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawText.");
    }

    public void Draw(Texture texture, Vec2 position, Color color)
    {
        CheckValid(texture);

        var quad = new Quad();

        PopulateQuad(
           ref quad,
           position.X,
           position.Y,
           texture.Width,
           texture.Height,
           color,
           Vec2.Zero,
           Vec2.One,
           0f
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, Vec2 position, Vec2 origin, Color color)
    {
        CheckValid(texture);

        var quad = new Quad();

        float dx = -texture.Width * origin.X;
        float dy = -texture.Height * origin.Y;

        PopulateQuad(
            ref quad,
            position.X + dx,
            position.Y + dy,
            texture.Width,
            texture.Height,
            color,
            Vec2.Zero,
            Vec2.One,
            0f
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, Vec2 position, Rect? textureRegion, Vec2 origin, Color color)
    {
        CheckValid(texture);

        var srcRect = textureRegion.GetValueOrDefault();

        float dx, dy, w, h;

        if (textureRegion.HasValue)
        {
            _texCoordTL.X = srcRect.X * texture.TexelWidth;
            _texCoordTL.Y = srcRect.Y * texture.TexelHeight;
            _texCoordBR.X = (srcRect.X + srcRect.Width) * texture.TexelWidth;
            _texCoordBR.Y = (srcRect.Y + srcRect.Height) * texture.TexelHeight;

            w = srcRect.Width;
            h = srcRect.Height;

        }
        else
        {
            _texCoordTL = Vec2.Zero;
            _texCoordBR = Vec2.One;
            w = texture.Width;
            h = texture.Height;
        }

        dx = -w * origin.X;
        dy = -h * origin.Y;

        var quad = new Quad();

        PopulateQuad(
            ref quad,
            position.X + dx,
            position.Y + dy,
            w,
            h,
            color,
            _texCoordTL,
            _texCoordBR,
            0f
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, RectF destination, Rect? textureRegion, Color color)
    {
        CheckValid(texture);

        if (textureRegion.HasValue)
        {
            var srcRect = textureRegion.GetValueOrDefault();

            _texCoordTL.X = srcRect.X * texture.TexelWidth;
            _texCoordTL.Y = srcRect.Y * texture.TexelHeight;
            _texCoordBR.X = (srcRect.X + srcRect.Width) * texture.TexelWidth;
            _texCoordBR.Y = (srcRect.Y + srcRect.Height) * texture.TexelHeight;

        }
        else
        {
            _texCoordTL = Vec2.Zero;
            _texCoordBR = Vec2.One;
        }

        var quad = new Quad();

        PopulateQuad(
            ref quad,
            destination.X,
            destination.Y,
            destination.Width,
            destination.Height,
            color,
            _texCoordTL,
            _texCoordBR,
            0f
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        Vec2 position,
        Rect? textureRegion,
        Color color,
        float rotation,
        Vec2 origin,
        Vec2 scale,
        FlipMode flipMode,
        float layerDepth

    )
    {
        CheckValid(texture);

        float w, h;

        if (textureRegion.HasValue)
        {
            var srcRect = textureRegion.GetValueOrDefault();

            w = srcRect.Width * scale.X;
            h = srcRect.Height * scale.Y;

            _texCoordTL.X = srcRect.X * texture.TexelWidth;
            _texCoordTL.Y = srcRect.Y * texture.TexelHeight;
            _texCoordBR.X = (srcRect.X + srcRect.Width) * texture.TexelWidth;
            _texCoordBR.Y = (srcRect.Y + srcRect.Height) * texture.TexelHeight;

        }
        else
        {
            w = texture.Width * scale.X;
            h = texture.Height * scale.Y;
            _texCoordTL = Vec2.Zero;
            _texCoordBR = Vec2.One;
        }

        if ((flipMode & FlipMode.Horizontal) != 0)
        {
            var temp = _texCoordBR.X;
            _texCoordBR.X = _texCoordTL.X;
            _texCoordTL.X = temp;
        }

        if ((flipMode & FlipMode.Vertical) != 0)
        {
            var temp = _texCoordBR.Y;
            _texCoordBR.Y = _texCoordTL.Y;
            _texCoordTL.Y = temp;
        }

        float dx = (-w * origin.X);
        float dy = (-h * origin.Y);

        var quad = new Quad();

        if (rotation == 0f)
        {
            PopulateQuad(
                ref quad,
                position.X + dx,
                position.Y + dy,
                w,
                h,
                color,
                _texCoordTL,
                _texCoordBR,
                layerDepth
            );
        }
        else
        {
            PopulateQuad(
                ref quad,
                position.X,
                position.Y,
                dx,
                dy,
                w * scale.X,
                h * scale.Y,
                Calc.Sin(rotation),
                Calc.Cos(rotation),
                color,
                _texCoordTL,
                _texCoordBR,
                layerDepth
            );
        }

        PushQuad(texture, ref quad);
    }

    public void DrawText(TextureFont font, string text, Vec2 position, Color color)
    {
        var charSource = new CharSource(text);
        DrawText(font, in charSource, position, color);
    }

    public void DrawText(TextureFont font, string text, Vec2 position, Vec2 scale, Color color)
    {
        var charSource = new CharSource(text);
        DrawText(font, in charSource, position, scale, color);
    }

    public void DrawText(TextureFont font, StringBuilder text, Vec2 position, Color color)
    {
        var charSource = new CharSource(text);
        DrawText(font, in charSource, position, color);
    }

    public void DrawText(TextureFont font, StringBuilder text, Vec2 position, Vec2 scale, Color color)
    {
        var charSource = new CharSource(text);
        DrawText(font, in charSource, position, scale, color);
    }

    internal unsafe void DrawText(TextureFont font, in CharSource text, Vec2 position, Color color)
    {
        CheckValid(font, text);

        var offset = Vec2.Zero;
        var firstGlyphOfLine = true;

        fixed (TextureFont.Glyph* glyphsPtr = font.Glyphs)
        {
            for (var i = 0; i < text.Length; ++i)
            {
                var c = text[i];

                if (c == '\r')
                {
                    continue;
                }

                if (c == '\n')
                {
                    offset.X = 0;
                    offset.Y = font.LineSpacing;
                    firstGlyphOfLine = true;
                    continue;
                }

                var currentGlyphIndex = font.GetGlyphIndexOrDefault(c);

                var currentGlyphPtr = glyphsPtr + currentGlyphIndex;

                // The first character on a line might have a negative left side bearing.
                // In this scenario, Canvas2D/Texture2D normally offset the text to the right,
                //  so that text does not hang off the left side of its rectangle.
                if (firstGlyphOfLine)
                {
                    offset.X = Calc.Max(currentGlyphPtr->LeftSideBearing, 0);
                    firstGlyphOfLine = false;
                }
                else
                {
                    offset.X += font.Spacing * currentGlyphPtr->LeftSideBearing;
                }

                var p = offset;

                p.X += currentGlyphPtr->Cropping.X;
                p.Y += currentGlyphPtr->Cropping.Y;
                p += position;

                _texCoordTL.X = currentGlyphPtr->BoundsInTexture.X * font.Texture.TexelWidth;
                _texCoordTL.Y = currentGlyphPtr->BoundsInTexture.Y * font.Texture.TexelHeight;
                _texCoordBR.X = (currentGlyphPtr->BoundsInTexture.X + currentGlyphPtr->BoundsInTexture.Width) * font.Texture.TexelWidth;
                _texCoordBR.Y = (currentGlyphPtr->BoundsInTexture.Y + currentGlyphPtr->BoundsInTexture.Height) * font.Texture.TexelHeight;

                var quad = new Quad();

                PopulateQuad(
                    ref quad,
                    p.X,
                    p.Y,
                    currentGlyphPtr->BoundsInTexture.Width,
                    currentGlyphPtr->BoundsInTexture.Height,
                    color,
                    _texCoordTL,
                    _texCoordBR,
                    0f
                );

                PushQuad(font.Texture, ref quad);

                offset.X += currentGlyphPtr->Width + currentGlyphPtr->RightSideBearing;
            }
        }

    }

    internal unsafe void DrawText(TextureFont font, in CharSource text, Vec2 position, Vec2 scale, Color color)
    {
        CheckValid(font, text);

        var offset = Vec2.Zero;
        var firstGlyphOfLine = true;

        Matrix transformation = Matrix.Identity;

        transformation.M11 = scale.X;
        transformation.M22 = scale.Y;
        transformation.M41 = position.X;
        transformation.M42 = position.Y;

        fixed (TextureFont.Glyph* glyphsPtr = font.Glyphs)
        {
            for (var i = 0; i < text.Length; ++i)
            {
                var c = text[i];

                if (c == '\r')
                {
                    continue;
                }

                if (c == '\n')
                {
                    offset.X = 0;
                    offset.Y = font.LineSpacing;
                    firstGlyphOfLine = true;
                    continue;
                }

                var currentGlyphIndex = font.GetGlyphIndexOrDefault(c);

                var currentGlyphPtr = glyphsPtr + currentGlyphIndex;

                // The first character on a line might have a negative left side bearing.
                // In this scenario, Canvas2D/Texture2D normally offset the text to the right,
                //  so that text does not hang off the left side of its rectangle.
                if (firstGlyphOfLine)
                {
                    offset.X = Calc.Max(currentGlyphPtr->LeftSideBearing, 0);
                    firstGlyphOfLine = false;
                }
                else
                {
                    offset.X += font.Spacing * currentGlyphPtr->LeftSideBearing;
                }

                var p = offset;

                p.X += currentGlyphPtr->Cropping.X;
                p.Y += currentGlyphPtr->Cropping.Y;

                _texCoordTL.X = currentGlyphPtr->BoundsInTexture.X * font.Texture.TexelWidth;
                _texCoordTL.Y = currentGlyphPtr->BoundsInTexture.Y * font.Texture.TexelHeight;
                _texCoordBR.X = (currentGlyphPtr->BoundsInTexture.X + currentGlyphPtr->BoundsInTexture.Width) * font.Texture.TexelWidth;
                _texCoordBR.Y = (currentGlyphPtr->BoundsInTexture.Y + currentGlyphPtr->BoundsInTexture.Height) * font.Texture.TexelHeight;

                Vec2.Transform(ref p, ref transformation, out p);

                var quad = new Quad();

                PopulateQuad(
                    ref quad,
                    p.X,
                    p.Y,
                    currentGlyphPtr->BoundsInTexture.Width * scale.X,
                    currentGlyphPtr->BoundsInTexture.Height * scale.Y,
                    color,
                    _texCoordTL,
                    _texCoordBR,
                    0f
                );

                PushQuad(font.Texture, ref quad);

                offset.X += currentGlyphPtr->Width + currentGlyphPtr->RightSideBearing;
            }
        }

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PushQuad(Texture texture, ref Quad quad)
    {
        if (_currentTexture == null)
        {
            _currentTexture = texture;
        }
        else if (!_currentTexture.Equals(texture))
        {
            Flush();
            _currentTexture = texture;
        }

        if (_quadBatcher.VertexCount + 4 > _quadBatcher.MaxVertices)
        {
            Flush();
        }

        _quadBatcher.PushQuad(ref quad);
    }

    private static void PopulateQuad(
        ref Quad quad,
        float x,
        float y,
        float w,
        float h,
        Color color,
        Vec2 texCoordTL,
        Vec2 texCoordBR, float depth
    )
    {

        quad.TopLeft.X = x;
        quad.TopLeft.Y = y;
        quad.TopLeft.U = texCoordTL.X;
        quad.TopLeft.V = texCoordTL.Y;
        quad.TopLeft.Z = depth;

        quad.TopRight.X = x + w;
        quad.TopRight.Y = y;
        quad.TopRight.U = texCoordBR.X;
        quad.TopRight.V = texCoordTL.Y;
        quad.TopRight.Z = depth;

        quad.BottomRight.X = x + w;
        quad.BottomRight.Y = y + h;
        quad.BottomRight.U = texCoordBR.X;
        quad.BottomRight.V = texCoordBR.Y;
        quad.BottomRight.Z = depth;

        quad.BottomLeft.X = x;
        quad.BottomLeft.Y = y + h;
        quad.BottomLeft.U = texCoordTL.X;
        quad.BottomLeft.V = texCoordBR.Y;
        quad.BottomLeft.Z = depth;

        quad.TopLeft.Color = color;
        quad.TopRight.Color = color;
        quad.BottomRight.Color = color;
        quad.BottomLeft.Color = color;
    }

    private static void PopulateQuad(
        ref Quad quad,
        float x,
        float y,
        float dx,
        float dy,
        float w,
        float h,
        float sin,
        float cos,
        Color color,
        Vec2 texCoordTL,
        Vec2 texCoordBR,
        float depth

    )
    {
        quad.TopLeft.X = x + (dx * cos) - (dy * sin);
        quad.TopLeft.Y = y + (dx * sin) + (dy * cos);
        quad.TopLeft.U = texCoordTL.X;
        quad.TopLeft.V = texCoordTL.Y;
        quad.TopLeft.Z = depth;

        quad.TopRight.X = x + ((dx + w) * cos) - (dy * sin);
        quad.TopRight.Y = y + ((dx + w) * sin) + (dy * cos);
        quad.TopRight.U = texCoordBR.X;
        quad.TopRight.V = texCoordTL.Y;
        quad.TopRight.Z = depth;

        quad.BottomRight.X = x + ((dx + w) * cos) - ((dy + h) * sin);
        quad.BottomRight.Y = y + ((dx + w) * sin) + ((dy + h) * cos);
        quad.BottomRight.U = texCoordBR.X;
        quad.BottomRight.V = texCoordBR.Y;
        quad.BottomRight.Z = depth;

        quad.BottomLeft.X = x + (dx * cos) - ((dy + h) * sin);
        quad.BottomLeft.Y = y + (dx * sin) + ((dy + h) * cos);
        quad.BottomLeft.U = texCoordTL.X;
        quad.BottomLeft.V = texCoordBR.Y;
        quad.BottomLeft.Z = depth;

        quad.TopLeft.Z = depth;
        quad.TopRight.Z = depth;
        quad.BottomRight.Z = depth;
        quad.BottomLeft.Z = depth;

        quad.TopLeft.Color = color;
        quad.TopRight.Color = color;
        quad.BottomRight.Color = color;
        quad.BottomLeft.Color = color;
    }

    private void CheckBegin(string method)
    {
        if (!_beginCalled)
        {
            throw new InvalidOperationException(
                method + " was called, but Begin has" +
                " not yet been called. Begin must be" +
                " called successfully before you can" +
                " call " + method + "."
            );
        }
    }

    private void ApplyRenderViewArea(Size size)
    {
        _viewport.Width = size.Width;
        _viewport.Height = size.Height;

        GraphicsContext.SetViewRect(_renderPass, _viewport.X, _viewport.Y, _viewport.Width, _viewport.Height);

        _projectionMatrix = Matrix.CreateOrthographicOffCenter(0f, _viewport.Width, _viewport.Height, 0f,
            -1.0f, 1.0f);
    }

    private void Flush()
    {
        GraphicsContext.Touch(_renderPass);

        ApplyRenderState();

        if (_quadBatcher.VertexCount == 0)
        {
            return;
        }

        _quadBatcher.Submit();

        GraphicsContext.Render(_renderPass, _currentShader);

        _drawCalls++;

        _quadBatcher.Reset();

        _currentTexture = null;

        if (_drawCalls > _maxDrawCalls)
        {
            _maxDrawCalls = _drawCalls;
        }
    }

    private void ApplyRenderState()
    {
        GraphicsContext.SetState(_blendState, _rasterizerState);
        GraphicsContext.SetSampleState(0, _samplerState);

        GraphicsContext.SetViewTransform(_renderPass, _transformMatrix, _projectionMatrix);
        _currentShader.SetTexture(_renderPass, _currentTexture ?? _defaultTexture);
    }

    private Vec2 _texCoordTL = new();
    private Vec2 _texCoordBR = new();

    private readonly QuadBatcher _quadBatcher;

    private bool _beginCalled;

    private Texture? _currentTexture;

    private readonly Texture _defaultTexture;

    private readonly ShaderProgram _defaultShader;
    private ShaderProgram _currentShader;

    private BlendState _blendState;
    private SamplerState _samplerState;
    private RasterizerState _rasterizerState;

    // How many draw calls in current batch
    private int _drawCalls;

    // Max reached draw calls
    private int _maxDrawCalls;

    private ushort _renderPass = 0;

    private Matrix _transformMatrix;

    private Matrix _projectionMatrix;

    private Rect _viewport;
}