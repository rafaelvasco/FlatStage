using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

/// <summary>
/// The comparison function used for depth, stencil, and alpha tests.
/// </summary>
public enum CompareFunction : ulong
{
    /// <summary>
    /// Always passes the test.
    /// </summary>
    Always = Bgfx.StateFlags.DepthTestAlways,

    /// <summary>
    /// Never passes the test.
    /// </summary>
    Never = Bgfx.StateFlags.DepthTestNever,

    /// <summary>
    /// Passes the test when the new pixel value is less than current pixel value.
    /// </summary>
    Less = Bgfx.StateFlags.DepthTestLess,

    /// <summary>
    /// Passes the test when the new pixel value is less than or equal to current pixel value.
    /// </summary>
    LessEqual = Bgfx.StateFlags.DepthTestLequal,

    /// <summary>
    /// Passes the test when the new pixel value is equal to current pixel value.
    /// </summary>
    Equal = Bgfx.StateFlags.DepthTestEqual,

    /// <summary>
    /// Passes the test when the new pixel value is greater than or equal to current pixel value.
    /// </summary>
    GreaterEqual = Bgfx.StateFlags.DepthTestGequal,

    /// <summary>
    /// Passes the test when the new pixel value is greater than current pixel value.
    /// </summary>
    Greater = Bgfx.StateFlags.DepthTestGreater,

    /// <summary>
    /// Passes the test when the new pixel value does not equal to current pixel value.
    /// </summary>
    NotEqual = Bgfx.StateFlags.DepthTestNotequal
}