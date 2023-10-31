using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;
public class GuiTextDef : GuiControlDef
{
    [JsonRequired]
    public required string Text { get; init; }

    public GuiText.HorizontalAlignment HorizontalAlign { get; set; }

    public GuiText.VerticalAlignment VerticalAlign { get; set; }
}
