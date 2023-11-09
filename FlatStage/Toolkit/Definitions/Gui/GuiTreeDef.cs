using System.Text.Json.Serialization;

namespace FlatStage.Toolkit;

public class GuiTreeNodeDef
{
    [JsonRequired]
    public required string Id { get; init; }

    [JsonRequired]
    public required string Label { get; init; }

    public GuiTreeNodeDef[]? ChildNodes { get; init; }
}

public class GuiTreeDef : GuiControlDef
{
    [JsonRequired]
    public required GuiTreeNodeDef[] Nodes { get; init; }
}
