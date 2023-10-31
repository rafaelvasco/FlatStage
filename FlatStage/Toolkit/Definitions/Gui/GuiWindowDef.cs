using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;
public class GuiWindowDef : GuiContainerDef
{
    [JsonRequired]
    public required string Title { get; init; }

    public GuiText.HorizontalAlignment TitleHorizontalAlignment { get; set; } = GuiText.HorizontalAlignment.Left;

    public GuiText.VerticalAlignment TitleVerticalAlignment { get; set; } = GuiText.VerticalAlignment.Center;
}
