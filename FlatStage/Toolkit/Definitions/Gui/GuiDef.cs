using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class GuiDef : IDefinitionData
{
    [JsonRequired]
    public required GuiContainerDef Main { get; init; }

    public bool IsValid()
    {
        return Main != null && Main.IsValid();
    }
}
