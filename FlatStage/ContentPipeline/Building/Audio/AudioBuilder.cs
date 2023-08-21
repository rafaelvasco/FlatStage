using System.IO;

namespace FlatStage.ContentPipeline;
internal class AudioBuilder : AssetBuilderAgent<AudioData, AudioAssetInfo>
{
    public AudioBuilder() : base("Audio")
    {
    }

    protected override AudioData BuildAssetData(string rootPath, AudioAssetInfo assetInfoType)
    {
        var audioExt = Path.GetExtension(assetInfoType.Path);

        var assetFilePath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.Path!);

        var fileData = File.ReadAllBytes(assetFilePath);

        if (audioExt == ".ogg"/* && fileData.Length < 1024*/)
        {
            var wavFromOgg = OggCompiler.Build(fileData);

            var result = new AudioData()
            {
                Id = assetInfoType.Id!,
                Data = wavFromOgg,
                Type = assetInfoType.Type
            };

            return result;
        }
        else
        {
            var result = new AudioData()
            {
                Id = assetInfoType.Id!,
                Data = fileData,
                Type = assetInfoType.Type
            };

            return result;
        }
    }
}
