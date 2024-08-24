using System;

namespace FlatStage.Toolkit;

[Flags]
public enum GuiControlState
{
    Idle = 1 << 0,
    Hover = 1 << 1,
    Active = 1 << 2,
    Checked = 1 << 3,
    Focused = 1 << 4,
    Disabled = 1 << 5
}
