using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

/// <summary>
/// Defines a culling mode for faces in rasterization process.
/// </summary>
public enum CullMode : ulong
{
    /// <summary>
    /// Do not cull faces.
    /// </summary>
    None = 0,

    /// <summary>
    /// Cull faces with clockwise order.
    /// </summary>
    CullClockwiseFace = Bgfx.StateFlags.CullCw,

    /// <summary>
    /// Cull faces with counter clockwise order.
    /// </summary>
    CullCounterClockwiseFace = Bgfx.StateFlags.CullCcw
}