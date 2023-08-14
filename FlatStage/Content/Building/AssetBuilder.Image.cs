using System;
using System.IO;
using Stb;

namespace FlatStage;

internal static partial class AssetBuilder
{
    private static string BuildAndExportImage(string rootPath, ImageAssetInfo imageAssetInfo)
    {
        IDefinitionData.ThrowIfInValid(imageAssetInfo, "AssetBuilder::BuildAndExportImage");

        Console.WriteLine($"Building Texture: {imageAssetInfo.Id}");

        var imageData = BuildImageData(rootPath, imageAssetInfo);

        var assetDirectory = Path.GetDirectoryName(imageAssetInfo.Path) ?? "";

        var assetFullBinPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
            assetDirectory,
            imageAssetInfo.Id + ContentProperties.BinaryExt);

        using var stream = File.OpenWrite(assetFullBinPath);

        BinarySerializer.Serialize(stream, ref imageData);

        Console.WriteLine($"Texture {imageAssetInfo.Id} built successfully.");

        return assetFullBinPath;
    }

    private static ImageData BuildImageData(string rootPath, ImageAssetInfo imageAssetInfo)
    {
        using var stream = File.OpenRead(Path.Combine(rootPath, ContentProperties.AssetsFolder, imageAssetInfo.Path!));

        var stbImage = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

        Blitter.Begin(stbImage.Data, stbImage.Width, stbImage.Height);
        Blitter.ConvertRgbaToBgra(premultiplyAlpha: true);
        Blitter.End();

        var result = new ImageData
        {
            Id = imageAssetInfo.Id!,
            Data = stbImage.Data,
            Width = stbImage.Width,
            Height = stbImage.Height,
            BytesPerPixel = 4
        };

        return result;
    }
}