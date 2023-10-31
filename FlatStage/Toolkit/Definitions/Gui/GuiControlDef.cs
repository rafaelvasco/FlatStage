using FlatStage.Engine.Toolkit.Definitions.Gui;
using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

[JsonDerivedType(typeof(GuiButtonDef), typeDiscriminator: "button")]
[JsonDerivedType(typeof(GuiTextDef), typeDiscriminator: "text")]
[JsonDerivedType(typeof(GuiCheckBoxDef), typeDiscriminator: "checkbox")]
[JsonDerivedType(typeof(GuiSliderDef), typeDiscriminator: "slider")]
[JsonDerivedType(typeof(GuiTextboxDef), typeDiscriminator: "textbox")]
[JsonDerivedType(typeof(GuiMenuBarDef), typeDiscriminator: "menubar")]
[JsonDerivedType(typeof(GuiContainerDef), typeDiscriminator: "container")]
[JsonDerivedType(typeof(GuiLayoutDef), typeDiscriminator: "layout")]
[JsonDerivedType(typeof(GuiPanelDef), typeDiscriminator: "panel")]
[JsonDerivedType(typeof(GuiWindowDef), typeDiscriminator: "window")]
public abstract class GuiControlDef : IDefinitionData
{
    [JsonRequired]
    public required string Id { get; init; }

    public int X { get; init; }

    public int Y { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public bool FixedSize { get; init; }

    public GuiAnchoring Anchor { get; init; } = GuiAnchoring.None;

    public virtual bool IsValid()
    {
        return !string.IsNullOrEmpty(Id);
    }
}
