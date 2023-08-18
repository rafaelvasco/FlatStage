using FlatStage.ContentPipeline;
using System;
using System.Runtime.CompilerServices;

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
    // Used to calculate texture coordinates
    private static readonly float[] CornerOffsetX =
    {
        0.0f,
        1.0f,
        0.0f,
        1.0f
    };

    private static readonly float[] CornerOffsetY =
    {
        0.0f,
        0.0f,
        1.0f,
        1.0f
    };

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

    public void Begin()
    {
        Begin(
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            RasterizerState.CullCounterClockWise,
            Matrix.Identity,
            null
        );
    }

    public void Begin(Matrix transform)
    {
        Begin(
            BlendState.AlphaBlend,
            SamplerState.PointWrap,
            RasterizerState.CullCounterClockWise,
            transform,
            null
        );
    }

    public void Begin(ShaderProgram shader, Matrix transform)
    {
        Begin(
            BlendState.AlphaBlend,
            SamplerState.PointWrap,
            RasterizerState.CullCounterClockWise,
            transform,
            shader
        );
    }

    public void Begin(BlendState blendState)
    {
        Begin(
            blendState,
            SamplerState.PointWrap,
            RasterizerState.CullCounterClockWise,
            Matrix.Identity,
            null
        );
    }

    public void Begin(
        BlendState blendState,
        SamplerState samplerState,
        RasterizerState rasterizerState,
        ShaderProgram? shader
    )
    {
        Begin(
            blendState,
            samplerState,
            rasterizerState,
            Matrix.Identity,
            shader
        );
    }

    public void Begin(
        BlendState blendState,
        SamplerState samplerState,
        RasterizerState rasterizerState,
        Matrix transformMatrix,
        ShaderProgram? shader
    )
    {
        if (_beginCalled)
        {
            throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
        }

        _beginCalled = true;

        _blendState = blendState;
        _samplerState = samplerState;
        _rasterizerState = rasterizerState;
        _currentShader = shader ?? _defaultShader;
        _transformMatrix = transformMatrix;
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

    public void Draw(Texture texture, float x, float y, Color color)
    {
        CheckBegin("Draw");

        var quad = new Quad();

        GenerateQuad(ref quad, 0f, 0f, 1f, 1f, x, y, texture.Width, texture.Height, color, 0f, 0f, 0f, 1f, 0f, 0);

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, Quad quad)
    {
        CheckBegin("Draw");

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, float x, float y, Rect? srcRect, Color color)
    {
        CheckBegin("Draw");

        float sourceX, sourceY, sourceW, sourceH;
        float destW, destH;
        if (srcRect.HasValue)
        {
            sourceX = srcRect.Value.X * texture.TexelWidth;
            sourceY = srcRect.Value.Y * texture.TexelHeight;
            sourceW = srcRect.Value.Width * texture.TexelWidth;
            sourceH = srcRect.Value.Height * texture.TexelHeight;
            destW = srcRect.Value.Width;
            destH = srcRect.Value.Height;
        }
        else
        {
            sourceX = 0.0f;
            sourceY = 0.0f;
            sourceW = 1.0f;
            sourceH = 1.0f;
            destW = texture.Width;
            destH = texture.Height;
        }

        var quad = new Quad();

        GenerateQuad(ref quad, sourceX, sourceY, sourceW, sourceH, x, y, destW, destH, color, 0f, 0f, 0f, 1f, 0f, 0);

        PushQuad(texture, ref quad);
    }

    public void Draw(Texture texture, float x, float y, Vec2 origin, Color color)
    {
        CheckBegin("Draw");
        var quad = new Quad();

        GenerateQuad(ref quad, 0f, 0f, 1f, 1f, x, y, texture.Width, texture.Height, color, origin.X, origin.Y, 0f, 1f,
            0f, 0);

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        float x,
        float y,
        Rect? srcRect,
        Color color,
        float rotation,
        Vec2 origin,
        float scale,
        FlipMode flipMode,
        float layerDepth
    )
    {
        CheckBegin("Draw");

        float sourceX, sourceY, sourceW, sourceH;
        float destW = scale;
        float destH = scale;
        if (srcRect.HasValue)
        {
            sourceX = srcRect.Value.X * texture.TexelWidth;
            sourceY = srcRect.Value.Y * texture.TexelHeight;
            sourceW = Math.Sign(srcRect.Value.Width) * Math.Max(
                Math.Abs(srcRect.Value.Width),
                Calc.MachineEpsilonFloat
            ) * texture.TexelWidth;
            sourceH = Math.Sign(srcRect.Value.Height) * Math.Max(
                Math.Abs(srcRect.Value.Height),
                Calc.MachineEpsilonFloat
            ) * texture.TexelHeight;
            destW *= srcRect.Value.Width;
            destH *= srcRect.Value.Height;
        }
        else
        {
            sourceX = 0.0f;
            sourceY = 0.0f;
            sourceW = 1.0f;
            sourceH = 1.0f;
            destW *= texture.Width;
            destH *= texture.Height;
        }

        var quad = new Quad();

        GenerateQuad(
            ref quad,
            sourceX,
            sourceY,
            sourceW,
            sourceH,
            x,
            y,
            destW,
            destH,
            color,
            origin.X,
            origin.Y,
            Calc.Sin(rotation),
            Calc.Cos(rotation),
            layerDepth,
            (byte)(flipMode & (FlipMode)0x03)
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        float x,
        float y,
        Rect? srcRect,
        Color color,
        float rotation,
        Vec2 origin,
        Vec2 scale,
        FlipMode flipMode,
        float layerDepth
    )
    {
        CheckBegin("Draw");

        float sourceX, sourceY, sourceW, sourceH;
        if (srcRect.HasValue)
        {
            sourceX = srcRect.Value.X * texture.TexelWidth;
            sourceY = srcRect.Value.Y * texture.TexelHeight;
            sourceW = Math.Sign(srcRect.Value.Width) * Math.Max(
                Math.Abs(srcRect.Value.Width),
                Calc.MachineEpsilonFloat
            ) * texture.TexelWidth;
            sourceH = Math.Sign(srcRect.Value.Height) * Math.Max(
                Math.Abs(srcRect.Value.Height),
                Calc.MachineEpsilonFloat
            ) * texture.TexelHeight;
            scale.X *= srcRect.Value.Width;
            scale.Y *= srcRect.Value.Height;
        }
        else
        {
            sourceX = 0.0f;
            sourceY = 0.0f;
            sourceW = 1.0f;
            sourceH = 1.0f;
            scale.X *= texture.Width;
            scale.Y *= texture.Height;
        }

        var quad = new Quad();

        GenerateQuad(
            ref quad,
            sourceX,
            sourceY,
            sourceW,
            sourceH,
            x,
            y,
            scale.X,
            scale.Y,
            color,
            origin.X,
            origin.Y,
            Calc.Sin(rotation),
            Calc.Cos(rotation),
            layerDepth,
            (byte)(flipMode & (FlipMode)0x03)
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        RectF destRect,
        Color color
    )
    {
        CheckBegin("Draw");

        var quad = new Quad();

        GenerateQuad(
            ref quad,
            0.0f,
            0.0f,
            1.0f,
            1.0f,
            destRect.X,
            destRect.Y,
            destRect.Width,
            destRect.Height,
            color,
            0.0f,
            0.0f,
            0.0f,
            1.0f,
            0.0f,
            0
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        RectF destRect,
        Rect? srcRect,
        Color color
    )
    {
        CheckBegin("Draw");
        float sourceX, sourceY, sourceW, sourceH;
        if (srcRect.HasValue)
        {
            sourceX = srcRect.Value.X * texture.TexelWidth;
            sourceY = srcRect.Value.Y * texture.TexelHeight;
            sourceW = srcRect.Value.Width * texture.TexelWidth;
            sourceH = srcRect.Value.Height * texture.TexelHeight;
        }
        else
        {
            sourceX = 0.0f;
            sourceY = 0.0f;
            sourceW = 1.0f;
            sourceH = 1.0f;
        }

        var quad = new Quad();

        GenerateQuad(
            ref quad,
            sourceX,
            sourceY,
            sourceW,
            sourceH,
            destRect.X,
            destRect.Y,
            destRect.Width,
            destRect.Height,
            color,
            0.0f,
            0.0f,
            0.0f,
            1.0f,
            0.0f,
            0
        );

        PushQuad(texture, ref quad);
    }

    public void Draw(
        Texture texture,
        RectF destRect,
        Rect? srcRect,
        Color color,
        float rotation,
        Vec2 origin,
        FlipMode flipMode,
        float layerDepth
    )
    {
        CheckBegin("Draw");
        float sourceX, sourceY, sourceW, sourceH;
        if (srcRect.HasValue)
        {
            sourceX = srcRect.Value.X * texture.TexelWidth;
            sourceY = srcRect.Value.Y / texture.TexelHeight;
            sourceW = Math.Sign(srcRect.Value.Width) * Math.Max(
                Math.Abs(srcRect.Value.Width),
                Calc.MachineEpsilonFloat
            ) * texture.TexelWidth;
            sourceH = Math.Sign(srcRect.Value.Height) * Math.Max(
                Math.Abs(srcRect.Value.Height),
                Calc.MachineEpsilonFloat
            ) * texture.TexelHeight;
        }
        else
        {
            sourceX = 0.0f;
            sourceY = 0.0f;
            sourceW = 1.0f;
            sourceH = 1.0f;
        }

        var quad = new Quad();

        GenerateQuad(
            ref quad,
            sourceX,
            sourceY,
            sourceW,
            sourceH,
            destRect.X,
            destRect.Y,
            destRect.Width,
            destRect.Height,
            color,
            origin.X,
            origin.Y,
            (float)Math.Sin(rotation),
            (float)Math.Cos(rotation),
            layerDepth,
            (byte)(flipMode & (FlipMode)0x03)
        );

        PushQuad(texture, ref quad);
    }

    /// <summary>
    /// Submit a text string of quads for drawing in the current batch
    /// </summary>
    /// <param name="font">The TextureFont object;</param>
    /// <param name="text">The text to draw;</param>
    /// <param name="position">The drawing location on the screen;</param>
    /// <param name="color">A color mask;</param>
    public unsafe void DrawText(TextureFont font, string text, Vec2 position, Color color)
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

                var quad = new Quad();

                GenerateQuad(
                    ref quad,
                    currentGlyphPtr->BoundsInTexture.X * font.Texture.TexelWidth,
                    currentGlyphPtr->BoundsInTexture.Y * font.Texture.TexelHeight,
                    currentGlyphPtr->BoundsInTexture.Width * font.Texture.TexelWidth,
                    currentGlyphPtr->BoundsInTexture.Height * font.Texture.TexelHeight,
                    p.X,
                    p.Y,
                    currentGlyphPtr->BoundsInTexture.Width,
                    currentGlyphPtr->BoundsInTexture.Height,
                    color,
                    0.0f,
                    0.0f,
                    0.0f,
                    1.0f,
                    0.0f,
                    0
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

    void CheckValid(TextureFont textureFont, string text)
    {
        if (textureFont == null)
            throw new ArgumentNullException("textureFont");
        if (text == null)
            throw new ArgumentNullException("text");
        if (!_beginCalled)
            throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
    }

    private static unsafe void GenerateQuad(
        ref Quad quad,
        float sourceX,
        float sourceY,
        float sourceW,
        float sourceH,
        float destinationX,
        float destinationY,
        float destinationW,
        float destinationH,
        Color color,
        float originX,
        float originY,
        float sin,
        float cos,
        float depth,
        byte effects
    )
    {
        float cornerX = -originX * destinationW;
        float cornerY = -originY * destinationH;

        quad.TopLeft.X = destinationX + cornerX * cos - cornerY * sin;
        quad.TopLeft.Y = destinationY + cornerX * sin + cornerY * cos;
        quad.TopRight.X = destinationX + (cornerX + destinationW) * cos - cornerY * sin;
        quad.TopRight.Y = destinationY + (cornerX + destinationW) * sin + cornerY * cos;
        quad.BottomRight.X = destinationX + (cornerX + destinationW) * cos - (cornerY + destinationH) * sin;
        quad.BottomRight.Y = destinationY + (cornerX + destinationW) * sin + (cornerY + destinationH) * cos;
        quad.BottomLeft.X = destinationX + cornerX * cos - (cornerY + destinationH) * sin;
        quad.BottomLeft.Y = destinationY + cornerX * sin + (cornerY + destinationH) * cos;

        fixed (float* flipX = &CornerOffsetX[0])
        {
            fixed (float* flipY = &CornerOffsetY[0])
            {
                quad.TopLeft.U = (flipX[0 ^ effects] * sourceW) + sourceX;
                quad.TopLeft.V = (flipY[0 ^ effects] * sourceH) + sourceY;
                quad.TopRight.U = (flipX[1 ^ effects] * sourceW) + sourceX;
                quad.TopRight.V = (flipY[1 ^ effects] * sourceH) + sourceY;
                quad.BottomRight.U = (flipX[3 ^ effects] * sourceW) + sourceX;
                quad.BottomRight.V = (flipY[2 ^ effects] * sourceH) + sourceY;
                quad.BottomLeft.U = (flipX[2 ^ effects] * sourceW) + sourceX;
                quad.BottomLeft.V = (flipY[3 ^ effects] * sourceH) + sourceY;
            }
        }

        var colorPremultiplied = new Color(color.Rf * color.Af, color.Gf * color.Af, color.Bf * color.Af, color.Af);

        quad.TopLeft.Z = depth;
        quad.TopRight.Z = depth;
        quad.BottomRight.Z = depth;
        quad.BottomLeft.Z = depth;
        quad.TopLeft.Color = colorPremultiplied;
        quad.TopRight.Color = colorPremultiplied;
        quad.BottomRight.Color = colorPremultiplied;
        quad.BottomLeft.Color = colorPremultiplied;
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