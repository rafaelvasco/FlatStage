using BGFX;

namespace FlatStage;

/// <summary>
/// Defines filtering types for texture sampler.
/// </summary>
public enum TextureFilter : uint
{
    /// <summary>
    /// Use linear filtering.
    /// </summary>
    Linear = Bgfx.SamplerFlags.None,

    /// <summary>
    /// Use point filtering.
    /// </summary>
    Point = Bgfx.SamplerFlags.Point
}
