namespace FlatStage;
internal class AudioBuilder() : AssetBuilderAgent<AudioData, AudioAssetInfo>("Audio")
{
    private const string WavExt = ".wav";
    private const string OggExt = ".ogg";

    protected override AudioData BuildAssetData(string rootPath, AudioAssetInfo assetInfoType)
    {
        var assetFilePath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.Path);

        var fileData = File.ReadAllBytes(assetFilePath);

        var ext = Path.GetExtension(assetFilePath);

        AudioFormat format;

        switch (ext)
        {
            case WavExt: format = AudioFormat.Wav; break;
            case OggExt: format = AudioFormat.Ogg; break;
            default: throw new Exception($"Unsupported Audio Extension: {ext}");
        }

        var result = new AudioData()
        {
            Id = assetInfoType.Id,
            Data = fileData,
            Type = assetInfoType.Type,
            Format = format,
        };

        return result;
    }
}
