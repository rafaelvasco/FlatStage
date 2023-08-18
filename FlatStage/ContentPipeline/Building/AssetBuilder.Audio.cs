using FlatStage.IO;
using System;
using System.IO;

namespace FlatStage.ContentPipeline;

internal static partial class AssetBuilder
{
    private static string BuildAndExportAudio(string rootPath, AudioAssetInfo audioAssetInfo)
    {
        IDefinitionData.ThrowIfInValid(audioAssetInfo, "AssetBuilder::BuildAndExportAudio");

        Console.WriteLine($"Building Audio: {audioAssetInfo.Id}");

        var audioData = BuildAudioData(rootPath, audioAssetInfo);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref audioData, audioAssetInfo);

        Console.WriteLine($"Audio {audioAssetInfo.Id} built successfully on path  {assetOutPutPath}");

        return assetOutPutPath;
    }

    private static AudioData BuildAudioData(string rootPath, AudioAssetInfo audioAssetInfo)
    {
        var audioExt = Path.GetExtension(audioAssetInfo.Path);

        var assetFilePath = Path.Combine(rootPath, ContentProperties.AssetsFolder, audioAssetInfo.Path!);

        var fileData = File.ReadAllBytes(assetFilePath);

        if (audioExt == ".ogg"/* && fileData.Length < 1024*/)
        {
            var wavFromOgg = OggCompiler.Build(fileData);

            var result = new AudioData()
            {
                Id = audioAssetInfo.Id!,
                Data = wavFromOgg,
                Type = audioAssetInfo.Type
            };

            return result;
        }
        else
        {
            var result = new AudioData()
            {
                Id = audioAssetInfo.Id!,
                Data = fileData,
                Type = audioAssetInfo.Type
            };

            return result;
        }
    }
}