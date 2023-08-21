using System.IO;
using System;
using FlatStage.IO;

namespace FlatStage.ContentPipeline;
internal class FontBuilder : AssetBuilderAgent<FontData, FontAssetInfo>
{
    private const int DEFAULT_BITMAP_FONT_IMAGE_SIZE = 1024;

    private static readonly CharRange _defaultCharRange = CharRange.BasicLatin;

    private readonly FontCompiler _fontCompiler;

    internal FontBuilder() : base("Font")
    {
        _fontCompiler = new FontCompiler();
    }

    protected override string Build(string rootPath, FontAssetInfo assetInfoType)
    {
        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"Building asset: {assetInfoType.Id}");

        var fontData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref fontData, assetInfoType);

#if DEBUG

        var debugFontImagePath = Path.Combine(Path.GetDirectoryName(assetOutPutPath)!, Path.GetFileNameWithoutExtension(assetOutPutPath) + ".png");

        File.WriteAllBytes(debugFontImagePath, fontData.ImageData.Data);

#endif

        Console.WriteLine($"Asset {assetInfoType.Id} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
    }

    protected override FontData BuildAssetData(string rootPath, FontAssetInfo assetInfoType)
    {
        var fontFileData = File.ReadAllBytes(Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.Path));

        CharRange range;

        if (assetInfoType.CharRange != null)
        {
            CharRange? rangeFromName = CharRange.FromName(assetInfoType.CharRange);

            if (rangeFromName == null)
            {
                throw new Exception($"Unsupported CharRange: {assetInfoType.CharRange}");
            }

            range = rangeFromName.Value;
        }
        else
        {
            range = _defaultCharRange;
        }

        var fontData = _fontCompiler.Build(assetInfoType.Id, fontFileData, DEFAULT_BITMAP_FONT_IMAGE_SIZE, assetInfoType.FontSize, range);

        return fontData;
    }
}
