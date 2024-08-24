using BGFX;

namespace FlatStage;

/// <summary>
/// Defines a function for color blending.
/// </summary>
public enum BlendFunction : ulong
{
    /// <summary>
    /// The function will add destination to the source. (srcColor * srcBlend) + (destColor * destBlend)
    /// </summary>
    Add = Bgfx.StateFlags.BlendEquationAdd,

    /// <summary>
    /// The function will subtract destination from source. (srcColor * srcBlend) - (destColor * destBlend)
    /// </summary>
    Subtract = Bgfx.StateFlags.BlendEquationSub,

    /// <summary>
    /// The function will subtract source from destination. (destColor * destBlend) - (srcColor * srcBlend)
    /// </summary>
    ReverseSubtract = Bgfx.StateFlags.BlendEquationRevsub,

    /// <summary>
    /// The function will extract minimum of the source and destination. min((srcColor * srcBlend),(destColor * destBlend))
    /// </summary>
    Max = Bgfx.StateFlags.BlendEquationMax,

    /// <summary>
    /// The function will extract maximum of the source and destination. max((srcColor * srcBlend),(destColor * destBlend))
    /// </summary>
    Min = Bgfx.StateFlags.BlendEquationMin,
}
