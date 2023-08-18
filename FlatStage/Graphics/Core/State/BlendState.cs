using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

public class BlendState
{
    public BlendFunction AlphaBlendFunction
    {
        get => _alphaBlendFunc;
        set
        {
            if (_alphaBlendFunc != value)
            {
                _alphaBlendFunc = value;
                UpdateUnionState();
            }
        }
    }

    public Blend AlphaDestinationBlend
    {
        get => _alphaDestBlend;
        set
        {
            if (_alphaDestBlend != value)
            {
                _alphaDestBlend = value;
                UpdateUnionState();
            }
        }
    }

    public Blend AlphaSourceBlend
    {
        get => _alphaSrcBlend;
        set
        {
            if (_alphaSrcBlend != value)
            {
                _alphaSrcBlend = value;
                UpdateUnionState();
            }
        }
    }

    public BlendFunction ColorBlendFunction
    {
        get => _colorBlendFunc;
        set
        {
            if (_colorBlendFunc != value)
            {
                _colorBlendFunc = value;
                UpdateUnionState();
            }
        }
    }

    public Blend ColorDestinationBlend
    {
        get => _colorDestBlend;
        set
        {
            if (_colorDestBlend != value)
            {
                _colorDestBlend = value;
                UpdateUnionState();
            }
        }
    }

    public Blend ColorSourceBlend
    {
        get => _colorSrcBlend;
        set
        {
            if (_colorSrcBlend != value)
            {
                _colorSrcBlend = value;
                UpdateUnionState();
            }
        }
    }

    public Color BlendColor { get; set; } = Color.White;

    private BlendState(
        Blend colorSurfaceBlend,
        Blend alphaSurfaceBlend,
        Blend colorDestBlend,
        Blend alphaDestBlend
    )
    {
        ColorSourceBlend = colorSurfaceBlend;
        AlphaSourceBlend = alphaSurfaceBlend;
        ColorDestinationBlend = colorDestBlend;
        AlphaDestinationBlend = alphaDestBlend;
    }

    private void UpdateUnionState()
    {
        State =
            BgfxUtils.BGFX_STATE_BLEND_EQUATION_SEPARATE((Bgfx.StateFlags)_colorBlendFunc,
                (Bgfx.StateFlags)_alphaBlendFunc) |
            BgfxUtils.STATE_BLEND_FUNC_SEPARATE((Bgfx.StateFlags)_colorSrcBlend, (Bgfx.StateFlags)_colorDestBlend,
                (Bgfx.StateFlags)_alphaSrcBlend, (Bgfx.StateFlags)_alphaDestBlend);

        // State = BgfxUtils.STATE_BLEND_FUNC_SEPARATE(Bgfx.StateFlags.BlendSrcAlpha,
        //         Bgfx.StateFlags.BlendInvSrcAlpha, Bgfx.StateFlags.BlendOne, Bgfx.StateFlags.BlendInvSrcAlpha);
    }

    public static readonly BlendState Additive = new(
        Blend.SourceAlpha,
        Blend.SourceAlpha,
        Blend.One,
        Blend.One
    );

    public static readonly BlendState AlphaBlend = new(
        Blend.One,
        Blend.One,
        Blend.InverseSourceAlpha,
        Blend.InverseSourceAlpha
    );

    public static readonly BlendState NonPremultiplied = new(
        Blend.SourceAlpha,
        Blend.SourceAlpha,
        Blend.InverseSourceAlpha,
        Blend.InverseSourceAlpha
    );

    public static readonly BlendState Opaque = new(
        Blend.One,
        Blend.One,
        Blend.Zero,
        Blend.Zero
    );

    private BlendFunction _alphaBlendFunc;

    private Blend _alphaDestBlend;
    private Blend _alphaSrcBlend;

    private BlendFunction _colorBlendFunc;
    private Blend _colorDestBlend;
    private Blend _colorSrcBlend;

    internal Bgfx.StateFlags State { get; private set; }
}