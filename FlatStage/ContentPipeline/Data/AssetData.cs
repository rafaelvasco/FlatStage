namespace FlatStage.ContentPipeline;

public abstract class AssetData : IDefinitionData
{
    public required string Id { get; init; }

    public abstract bool IsValid();
}