namespace FlatStage.Graphics;

using System;

/// <summary>
/// Specifies various debug options.
/// </summary>
[Flags]
public enum DebugFeatures
{
    /// <summary>
    /// Don't enable any debugging features.
    /// </summary>
    None = 0,
    /// <summary>
    /// Display internal statistics.
    /// </summary>
    DisplayStatistics = 0x4,

    /// <summary>
    /// Display debug text.
    /// </summary>
    DisplayText = 0x8,
}

/// <summary>
/// Specifies debug text colors.
/// </summary>
public enum DebugColor
{
    /// <summary>
    /// Black.
    /// </summary>
    Black,

    /// <summary>
    /// Blue.
    /// </summary>
    Blue,

    /// <summary>
    /// Green.
    /// </summary>
    Green,

    /// <summary>
    /// Cyan.
    /// </summary>
    Cyan,

    /// <summary>
    /// Red.
    /// </summary>
    Red,

    /// <summary>
    /// Magenta.
    /// </summary>
    Magenta,

    /// <summary>
    /// Brown.
    /// </summary>
    Brown,

    /// <summary>
    /// Light gray.
    /// </summary>
    LightGray,

    /// <summary>
    /// Dark gray.
    /// </summary>
    DarkGray,

    /// <summary>
    /// Light blue.
    /// </summary>
    LightBlue,

    /// <summary>
    /// Light green.
    /// </summary>
    LightGreen,

    /// <summary>
    /// Light cyan.
    /// </summary>
    LightCyan,

    /// <summary>
    /// Light red.
    /// </summary>
    LightRed,

    /// <summary>
    /// Light magenta.
    /// </summary>
    LightMagenta,

    /// <summary>
    /// Yellow.
    /// </summary>
    Yellow,

    /// <summary>
    /// White.
    /// </summary>
    White
}