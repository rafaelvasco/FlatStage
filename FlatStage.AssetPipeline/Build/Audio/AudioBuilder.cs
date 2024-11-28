namespace FlatStage;
internal class AudioBuilder() : AssetBuilderAgent<SoundData, AudioAssetInfo>("Audio")
{
    protected override SoundData BuildAssetData(string rootPath, AudioAssetInfo assetInfoType)
    {
        var assetFilePath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.Path);

        var fileData = File.ReadAllBytes(assetFilePath);

        var result = new SoundData()
        {
            Id = assetInfoType.Id,
            Data = fileData,
            Type = assetInfoType.Type,
        };

        return result;
    }
}
