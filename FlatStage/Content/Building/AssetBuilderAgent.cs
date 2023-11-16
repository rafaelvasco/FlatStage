using System;

namespace FlatStage.Content;

internal interface IAssetBuilder
{
    void Build(string rootPath, AssetInfo assetInfo);
    AssetData BuildData(string rootPath, AssetInfo assetInfo);

}

internal abstract class AssetBuilderAgent<DataType, AssetInfoType> : IAssetBuilder
    where DataType : AssetData where AssetInfoType : AssetInfo
{
    protected string Name { get; }

    protected AssetBuilderAgent(string builderName)
    {
        Name = builderName;
    }

    void IAssetBuilder.Build(string rootPath, AssetInfo assetInfo)
    {
        Build(rootPath, (AssetInfoType)assetInfo);
    }

    public AssetData BuildData(string rootPath, AssetInfo assetInfo)
    {
        return BuildAssetData(rootPath, (AssetInfoType)assetInfo);
    }

    protected virtual void Build(string rootPath, AssetInfoType assetInfoType)
    {
        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"\nBuilding Asset: {assetInfoType.Id}\n");

        var assetData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = AssetDataIO.SaveAssetData(rootPath, assetData, assetInfoType);

        Console.WriteLine($"Asset {assetInfoType.Id} built successfully on path {assetOutPutPath}");
    }

    protected abstract DataType BuildAssetData(string rootPath, AssetInfoType assetInfoType);

}
