namespace FlatStage.ContentPipeline;

internal abstract class AssetData : IDefinitionData
{
    public required string Id { get; set; }

    public abstract bool IsValid();
}
