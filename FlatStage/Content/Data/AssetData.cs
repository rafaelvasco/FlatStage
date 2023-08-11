namespace FlatStage;

public abstract class AssetData : IDefinitionData
{
    public string? Id { get; init; }

    public abstract bool IsValid();
}