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

public enum CanvasStretchMode
{
    PixelPerfect,
    LetterBox,
    Stretch
}

public class Canvas
{

    #region STATIC

    private static Canvas _instance = null!;

    public static CanvasStretchMode StretchMode
    {
        get => _instance._stretchMode;
        set => _instance._stretchMode = value;
    }

    public static int Width => _instance._width;

    public static int Height => _instance._height;

    public static void SetViewRegion(Rect rect)
    {
        _instance._mainViewport.SetSource(rect);
    }
    #endregion

    internal Canvas(int maxQuads = 2048)
    {
        _instance = this;

        if (!Calc.IsPowerOfTwo(maxQuads))
        {
            maxQuads = Calc.NextPowerOfTwo(maxQuads);
        }

        GraphicsContext.BackBufferSizeChanged = CalculateSizeFromDisplaySize;

        _width = Stage.Settings.CanvasWidth;
        _height = Stage.Settings.CanvasHeight;

        _quadBatcher = new QuadBatcher(maxQuads);

        _defaultShader = Content.Get<ShaderProgram>("canvas2d", embeddedAsset: true);

        _primitiveTexture = GraphicsContext.CreateTexture("primitiveTex", new TextureProps()
        {
            Width = 1,
            Height = 1,
            Data = new byte[4] { 255, 255, 255, 255 }

        });

        _currentShader = _defaultShader;

        _blendState = BlendState.AlphaBlend;

        _samplerState = SamplerState.PointWrap;

        _rasterizerState = RasterizerState.CullCounterClockWise;

        _mainViewport = new CanvasViewport(Width, Height);

        _currentViewport = _mainViewport;

        _transformMatrix = Matrix.Identity;

        CalculateSizeFromDisplaySize(Stage.WindowSize.Width, Stage.WindowSize.Height);
    }

    #region SET_STATE
    public void SetShader(ShaderProgram? shader = null)
    {
        Submit();

        _currentShader = shader ?? _defaultShader;

        _renderPass++;
    }

    public void SetViewport(CanvasViewport? viewport = null)
    {
        Submit();

        _renderPass++;

        if (viewport != null)
        {
            _currentViewport = viewport;
        }
        else
        {
            _currentViewport = _mainViewport;
        }

        GraphicsContext.Touch(_renderPass);
        GraphicsContext.SetViewClear(_renderPass, _currentViewport.BackgroundColor);
        GraphicsContext.SetRenderTarget(_renderPass, _currentViewport.RenderTarget);
        GraphicsContext.SetViewRect(_renderPass, 0, 0, _currentViewport.Width, _currentViewport.Height);
        GraphicsContext.SetViewTransform(_renderPass, _transformMatrix, _currentViewport.ProjectionMatrix);
    }
    #endregion

    #region TEXTURE_DRAW
    public void Draw(Texture texture, Vec2 position, Color color)
    {
        CheckValid(texture);

        var quad = BuildQuad(
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

        float dx = -texture.Width * origin.X;
        float dy = -texture.Height * origin.Y;

        var quad = BuildQuad(
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

        var quad = BuildQuad(
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

        var quad = BuildQuad(
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

        if (rotation == 0f)
        {
            var quad = BuildQuad(
                position.X + dx,
                position.Y + dy,
                w,
                h,
                color,
                _texCoordTL,
                _texCoordBR,
                layerDepth
            );
            PushQuad(texture, ref quad);
            return;
        }
        else
        {
            var quad = BuildQuad(
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
            PushQuad(texture, ref quad);
            return;
        }
    }
    #endregion

    #region TEXT_DRAW
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

                var quad = BuildQuad(
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

                var quad = BuildQuad(
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
    #endregion

    #region PRIMITIVE_DRAW

    public unsafe void DrawRect(float x, float y, float width, float height, int lineSize, Color color)
    {
        var quads = stackalloc Quad[4];

        quads[0] = BuildQuad(x, y + lineSize, lineSize, height - lineSize, color, Vec2.Zero, Vec2.Zero, 0.0f);
        quads[1] = BuildQuad(x + width - lineSize, y + lineSize, lineSize, height - lineSize, color, Vec2.Zero, Vec2.Zero, 0.0f);
        quads[2] = BuildQuad(x, y, width, lineSize, color, Vec2.Zero, Vec2.Zero, 0.0f);
        quads[3] = BuildQuad(x + lineSize, y + height - lineSize, width - lineSize, lineSize, color, Vec2.Zero, Vec2.Zero, 0.0f);

        PushQuads(_primitiveTexture, new Span<Quad>(quads, 4));
    }

    public void FillRect(float x, float y, float width, float height, Color color)
    {
        var quad = BuildQuad(x, y, width, height, color, Vec2.Zero, Vec2.Zero, 0.0f);

        PushQuad(_primitiveTexture, ref quad);
    }

    #endregion

    #region PRIVATE_INTERNAL
    internal void BeginRendering()
    {
        _renderPass = 0;

        _drawCalls = 0;

        SetViewport();
    }

    internal void EndRendering()
    {
        Submit();

        _renderPass++;

        RenderMainViewport();
    }

    private void RenderMainViewport()
    {
        GraphicsContext.SetViewClear(_renderPass, Color.Black);
        GraphicsContext.SetRenderTarget(_renderPass);
        GraphicsContext.SetViewRect(_renderPass, 0, 0, Stage.WindowSize.Width, Stage.WindowSize.Height);
        GraphicsContext.SetViewTransform(_renderPass, Matrix.Identity, _mainProjectionMatrix);

        var quad = BuildQuad(
            _mainViewportDestinationRect.X,
            _mainViewportDestinationRect.Y,
            _mainViewportDestinationRect.Width,
            _mainViewportDestinationRect.Height,
            Color.White,
            Vec2.Zero,
            Vec2.One,
            0.0f
        );

        _currentTexture = _mainViewport.Texture;

        _currentShader = _defaultShader;

        _quadBatcher.PushQuad(ref quad);

        Submit();
    }

    private static void CheckValid(Texture texture)
    {
        if (texture == null)
            throw new ArgumentNullException("texture");
    }

    private static void CheckValid(TextureFont font, in CharSource? text)
    {
        if (font == null)
            throw new ArgumentNullException("font");
        if (text == null)
            throw new ArgumentNullException("text");
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
            Submit();
            _currentTexture = texture;
        }

        if (_quadBatcher.VertexCount + 4 > _quadBatcher.MaxVertices)
        {
            Submit();
        }

        _quadBatcher.PushQuad(ref quad);
    }

    private void PushQuads(Texture texture, Span<Quad> quads)
    {
        if (_currentTexture == null)
        {
            _currentTexture = texture;
        }
        else if (!_currentTexture.Equals(texture))
        {
            Submit();
            _currentTexture = texture;
        }

        if (_quadBatcher.VertexCount + (quads.Length * 4) > _quadBatcher.MaxVertices)
        {
            Submit();
        }

        _quadBatcher.PushQuads(quads);
    }

    private static Quad BuildQuad(
        float x,
        float y,
        float w,
        float h,
        Color color,
        Vec2 texCoordTL,
        Vec2 texCoordBR, float depth
    )
    {

        var quad = new Quad();

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

        return quad;
    }

    private static Quad BuildQuad(
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
        var quad = new Quad();

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

        return quad;
    }

    private void Submit()
    {
        if (_quadBatcher.VertexCount == 0)
        {
            return;
        }

        ApplyRenderState();

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

        _currentShader.SetTexture(0, _currentTexture ?? _primitiveTexture);
    }

    private void CalculateSizeFromDisplaySize(int displayWidth, int displayHeight)
    {
        _mainProjectionMatrix = Matrix.CreateOrthographicOffCenter(0f, displayWidth, displayHeight, 0f,
           -1.0f, 1.0f);

        switch (StretchMode)
        {
            case CanvasStretchMode.PixelPerfect:

                if (displayWidth > Width || displayHeight > Height)
                {
                    float aspectRatioCanvas = (float)Width / Height;
                    float aspectRatioDisplay = (float)displayWidth / displayHeight;

                    int scaleW = (int)Calc.Round((float)displayWidth / Width);
                    int scaleH = (int)Calc.Round((float)displayHeight / Height);

                    if (aspectRatioDisplay > aspectRatioCanvas)
                    {
                        scaleW = scaleH;
                    }
                    else
                    {
                        scaleH = scaleW;
                    }

                    if ((Width * scaleW) > displayWidth || (Height * scaleH) > displayHeight)
                    {
                        scaleW = 1;
                        scaleH = 1;
                    }

                    int marginX = (displayWidth - (Width * scaleW)) / 2;
                    int marginY = (displayHeight - (Height * scaleH)) / 2;

                    _mainViewportDestinationRect = new Rect(marginX, marginY, Width * scaleW, Height * scaleH);

                    Console.WriteLine($"Display Size: {displayWidth}, {displayHeight}");
                    Console.WriteLine($"Canvas Size: {Width}, {Height}");
                    Console.WriteLine($"AR Canvas: {aspectRatioCanvas}");
                    Console.WriteLine($"AR Display: {aspectRatioDisplay}");

                    Console.WriteLine($"ScaleW: {scaleW}");
                    Console.WriteLine($"ScaleH: {scaleH}");

                    Console.WriteLine($"MarginX: {marginX}");
                    Console.WriteLine($"MarginY: {marginY}");

                }
                else
                {
                    _mainViewportDestinationRect = new Rect(0, 0, Width, Height);
                }

                break;

            case CanvasStretchMode.LetterBox:

                if (displayWidth > Width || displayHeight > Height)
                {
                    float aspectRatioCanvas = (float)Width / Height;
                    float aspectRatioDisplay = (float)displayWidth / displayHeight;

                    float scaleW = (float)displayWidth / Width;
                    float scaleH = (float)displayHeight / Height;

                    if (aspectRatioDisplay > aspectRatioCanvas)
                    {
                        scaleW = scaleH;
                    }
                    else
                    {
                        scaleH = scaleW;
                    }

                    int marginX = (int)((displayWidth - Width * scaleW) / 2);
                    int marginY = (int)((displayHeight - Height * scaleH) / 2);

                    _mainViewportDestinationRect = new Rect(marginX, marginY, (int)(Width * scaleW), (int)(Height * scaleH));

                    Console.WriteLine($"Display Size: {displayWidth}, {displayHeight}");
                    Console.WriteLine($"Canvas Size: {Width}, {Height}");
                    Console.WriteLine($"AR Canvas: {aspectRatioCanvas}");
                    Console.WriteLine($"AR Display: {aspectRatioDisplay}");

                    Console.WriteLine($"ScaleW: {scaleW}");
                    Console.WriteLine($"ScaleH: {scaleH}");

                    Console.WriteLine($"MarginX: {marginX}");
                    Console.WriteLine($"MarginY: {marginY}");

                }
                else
                {
                    _mainViewportDestinationRect = new Rect(0, 0, Width, Height);
                }

                break;

            case CanvasStretchMode.Stretch:

                _mainViewportDestinationRect = new Rect(0, 0, displayWidth, displayHeight);

                break;
        }
    }
    #endregion

    #region MEMBERS
    private int _width;
    private int _height;

    private ushort _renderPass = 0;

    private Vec2 _texCoordTL = new();
    private Vec2 _texCoordBR = new();

    private readonly QuadBatcher _quadBatcher;
    private readonly ShaderProgram _defaultShader;

    private Texture? _currentTexture;
    private Texture _primitiveTexture;

    private ShaderProgram _currentShader;

    private BlendState _blendState;
    private SamplerState _samplerState;
    private RasterizerState _rasterizerState;

    private int _drawCalls;
    private int _maxDrawCalls;

    private readonly CanvasViewport _mainViewport;
    private CanvasViewport _currentViewport;
    private Matrix _mainProjectionMatrix;
    private Rect _mainViewportDestinationRect;
    private Matrix _transformMatrix;

    private CanvasStretchMode _stretchMode = CanvasStretchMode.PixelPerfect;
    #endregion
}
