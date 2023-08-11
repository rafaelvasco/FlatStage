using FlatStage.Foundation.BGFX;

namespace FlatStage;

public enum StencilCompareFunction : uint
{
    /// <summary>
    /// Always passes the test.
    /// </summary>
    Always = Bgfx.StencilFlags.TestAlways,

    /// <summary>
    /// Never passes the test.
    /// </summary>
    Never = Bgfx.StencilFlags.TestNever,

    /// <summary>
    /// Passes the test when the new pixel value is less than current pixel value.
    /// </summary>
    Less = Bgfx.StencilFlags.TestLess,

    /// <summary>
    /// Passes the test when the new pixel value is less than or equal to current pixel value.
    /// </summary>
    LessEqual = Bgfx.StencilFlags.TestLequal,

    /// <summary>
    /// Passes the test when the new pixel value is equal to current pixel value.
    /// </summary>
    Equal = Bgfx.StencilFlags.TestEqual,

    /// <summary>
    /// Passes the test when the new pixel value is greater than or equal to current pixel value.
    /// </summary>
    GreaterEqual = Bgfx.StencilFlags.TestGequal,

    /// <summary>
    /// Passes the test when the new pixel value is greater than current pixel value.
    /// </summary>
    Greater = Bgfx.StencilFlags.TestGreater,

    /// <summary>
    /// Passes the test when the new pixel value does not equal to current pixel value.
    /// </summary>
    NotEqual = Bgfx.StencilFlags.TestNotequal
}