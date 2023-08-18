using System;
using System.IO;

using FlatStage.Graphics;
using FlatStage.IO;

namespace FlatStage.ContentPipeline;

internal static partial class AssetBuilder
{
    private static string BuildAndExportImage(string rootPath, ImageAssetInfo imageAssetInfo)
    {
        IDefinitionData.ThrowIfInValid(imageAssetInfo, "AssetBuilder::BuildAndExportImage");

        Console.WriteLine($"Building Texture: {imageAssetInfo.Id}");

        var imageData = BuildImageData(rootPath, imageAssetInfo);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref imageData, imageAssetInfo);

        Console.WriteLine($"Texture {imageAssetInfo.Id} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
    }

    private static ImageData BuildImageData(string rootPath, ImageAssetInfo imageAssetInfo)
    {
        var (Data, Width, Height) = ImageIO.LoadPNG(Path.Combine(rootPath, ContentProperties.AssetsFolder, imageAssetInfo.Path!));

        // Convert RgbaToBgra
        Blitter.Begin(Data, Width, Height);
        Blitter.ConvertRgbaToBgra(premultiplyAlpha: true);
        Blitter.End();

        ImageIO.SavePNGToMem(Data, Width, Height, out var finalImageData);

        var result = new ImageData
        {
            Id = imageAssetInfo.Id!,
            Data = finalImageData,
            Width = Width,
            Height = Height,
        };

        return result;
    }
}