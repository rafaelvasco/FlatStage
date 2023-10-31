using FlatStage.Toolkit;
using System.Text.Json.Serialization;

namespace FlatStage.Engine.Toolkit.Definitions.Gui;

public class GuiMenuItemDef
{
    [JsonRequired]
    public required string Id { get; init; }

    [JsonRequired]
    public required string Label { get; init; }

    public GuiMenuItemDef[]? SubMenuItems { get; init; }
}

public class GuiMenuBarDef : GuiControlDef
{
    [JsonRequired]
    public required GuiMenuItemDef[] MenuItems { get; init; }
}
