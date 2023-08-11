using System;
using System.IO;

namespace FlatStage;

internal static partial class AssetBuilder
{
    private static string BuildAndExportAudio(string rootPath, AudioAssetInfo audioAssetInfo)
    {
        IDefinitionData.ThrowIfInValid(audioAssetInfo, "AssetBuilder::BuildAndExportAudio");

        Console.WriteLine($"Building Audio: {audioAssetInfo.Id}");

        var imageData = BuildAudioData(rootPath, audioAssetInfo);

        var assetFullBinPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
            audioAssetInfo.Id + ContentProperties.BinaryExt);

        using var stream = File.OpenWrite(assetFullBinPath);

        BinarySerializer.Serialize(stream, ref imageData);

        Console.WriteLine($"Audio {audioAssetInfo.Id} built successfully.");

        return assetFullBinPath;
    }

    private static AudioData BuildAudioData(string rootPath, AudioAssetInfo audioAssetInfo)
    {
        var assetFilePath = Path.Combine(rootPath, ContentProperties.AssetsFolder, audioAssetInfo.Path!);

        var fileData = File.ReadAllBytes(assetFilePath);

        var result = new AudioData()
        {
            Id = audioAssetInfo.Id!,
            Data = fileData,
            Type = audioAssetInfo.Type
        };

        return result;
    }
}