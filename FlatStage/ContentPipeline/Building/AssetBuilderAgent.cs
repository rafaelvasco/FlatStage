using FlatStage.IO;
using System;

namespace FlatStage.ContentPipeline;

internal interface IAssetBuilder
{
    string Build(string rootPath, AssetInfo assetInfo);
}

internal abstract class AssetBuilderAgent<DataType, AssetInfoType> : IAssetBuilder
    where DataType : AssetData where AssetInfoType : AssetInfo
{
    protected string Name { get; private set; }

    protected AssetBuilderAgent(string builderName)
    {
        Name = builderName;
    }

    string IAssetBuilder.Build(string rootPath, AssetInfo assetInfo)
    {
        return Build(rootPath, (AssetInfoType)assetInfo);
    }

    protected virtual string Build(string rootPath, AssetInfoType assetInfoType)
    {
        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"Building Asset: {assetInfoType.Id}");

        var assetData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref assetData, assetInfoType);

        Console.WriteLine($"Asset {assetInfoType.Id} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
    }

    protected abstract DataType BuildAssetData(string rootPath, AssetInfoType assetInfoType);
}
