using System.IO;
using System;
using FlatStage.IO;

namespace FlatStage.ContentPipeline;
internal static partial class AssetBuilder
{
    private const int DEFAULT_BITMAP_FONT_IMAGE_SIZE = 1024;

    private static readonly CharRange _defaultCharRange = CharRange.BasicLatin;

    private static string BuildAndExportFont(string rootPath, FontAssetInfo fontAssetInfo, FontCompiler fontCompiler)
    {
        IDefinitionData.ThrowIfInValid(fontAssetInfo, "AssetBuilder::BuildAndExportFont");

        Console.WriteLine($"Building Font: {fontAssetInfo.Id}");

        var fontData = BuildFontData(rootPath, fontAssetInfo, fontCompiler);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref fontData, fontAssetInfo);

#if DEBUG

        var debugFontImagePath = Path.Combine(Path.GetDirectoryName(assetOutPutPath)!, Path.GetFileNameWithoutExtension(assetOutPutPath) + ".png");

        File.WriteAllBytes(debugFontImagePath, fontData.ImageData.Data);

#endif

        Console.WriteLine($"Font {fontAssetInfo.Id} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
    }

    private static FontData BuildFontData(string rootPath, FontAssetInfo fontAssetInfo, FontCompiler fontCompiler)
    {
        var fontFileData = File.ReadAllBytes(Path.Combine(rootPath, ContentProperties.AssetsFolder, fontAssetInfo.Path));

        CharRange range;

        if (fontAssetInfo.CharRange != null)
        {
            CharRange? rangeFromName = CharRange.FromName(fontAssetInfo.CharRange);

            if (rangeFromName == null)
            {
                throw new Exception($"Unsupported CharRange: {fontAssetInfo.CharRange}");
            }

            range = rangeFromName.Value;
        }
        else
        {
            range = _defaultCharRange;
        }

        var fontData = fontCompiler.Build(fontAssetInfo.Id, fontFileData, DEFAULT_BITMAP_FONT_IMAGE_SIZE, fontAssetInfo.FontSize, range);

        return fontData;
    }
}
