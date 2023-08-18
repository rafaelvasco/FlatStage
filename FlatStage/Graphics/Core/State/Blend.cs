using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

public enum Blend : ulong
{
    /// <summary>
    /// Each component of the color is multiplied by {1, 1, 1, 1}.
    /// </summary>
    One = Bgfx.StateFlags.BlendOne,

    /// <summary>
    /// Each component of the color is multiplied by {0, 0, 0, 0}.
    /// </summary>
    Zero = Bgfx.StateFlags.BlendZero,

    /// <summary>
    /// Each component of the color is multiplied by the source color.
    /// {Rs, Gs, Bs, As}, where Rs, Gs, Bs, As are color source values.
    /// </summary>
    SourceColor = Bgfx.StateFlags.BlendSrcColor,

    /// <summary>
    /// Each component of the color is multiplied by the inverse of the source color.
    /// {1 - Rs, 1 - Gs, 1 - Bs, 1 - As}, where Rs, Gs, Bs, As are color source values.
    /// </summary>
    InverseSourceColor = Bgfx.StateFlags.BlendInvSrcColor,

    /// <summary>
    /// Each component of the color is multiplied by the alpha value of the source.
    /// {As, As, As, As}, where As is the source alpha value.
    /// </summary>
    SourceAlpha = Bgfx.StateFlags.BlendSrcAlpha,

    /// <summary>
    /// Each component of the color is multiplied by the inverse of the alpha value of the source.
    /// {1 - As, 1 - As, 1 - As, 1 - As}, where As is the source alpha value.
    /// </summary>
    InverseSourceAlpha = Bgfx.StateFlags.BlendInvSrcAlpha,

    /// <summary>
    /// Each component color is multiplied by the destination color.
    /// {Rd, Gd, Bd, Ad}, where Rd, Gd, Bd, Ad are color destination values.
    /// </summary>
    DestinationColor = Bgfx.StateFlags.BlendDstColor,

    /// <summary>
    /// Each component of the color is multiplied by the inversed destination color.
    /// {1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad}, where Rd, Gd, Bd, Ad are color destination values.
    /// </summary>
    InverseDestinationColor = Bgfx.StateFlags.BlendInvDstColor,

    /// <summary>
    /// Each component of the color is multiplied by the alpha value of the destination.
    /// {Ad, Ad, Ad, Ad}, where Ad is the destination alpha value.
    /// </summary>
    DestinationAlpha = Bgfx.StateFlags.BlendDstAlpha,

    /// <summary>
    /// Each component of the color is multiplied by the inversed alpha value of the destination.
    /// {1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad}, where Ad is the destination alpha value.
    /// </summary>
    InverseDestinationAlpha = Bgfx.StateFlags.BlendInvDstAlpha,

    /// <summary>
    /// Each component of the color is multiplied by a constant in the <see cref="Bgfx.StateFlags.BlendFactor"/>.
    /// </summary>
    BlendFactor = Bgfx.StateFlags.BlendFactor,

    /// <summary>
    /// Each component of the color is multiplied by a inversed constant in the <see cref="Bgfx.StateFlags.BlendFactor"/>.
    /// </summary>
    InverseBlendFactor = Bgfx.StateFlags.BlendInvFactor,

    /// <summary>
    /// Each component of the color is multiplied by either the alpha of the source color, or the inverse of the alpha of the source color, whichever is greater.
    /// {f, f, f, 1}, where f = min(As, 1 - As), where As is the source alpha value.
    /// </summary>
    SourceAlphaSaturation = Bgfx.StateFlags.BlendSrcAlphaSat,
}