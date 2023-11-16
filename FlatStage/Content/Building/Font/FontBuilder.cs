using System.IO;
using System;

namespace FlatStage.Content;
internal class FontBuilder : AssetBuilderAgent<FontData, FontAssetInfo>
{
    private const int DEFAULT_BITMAP_FONT_IMAGE_SIZE = 256;

    private static readonly CharRange _defaultCharRange = CharRange.BasicLatin;

    private readonly FontCompiler _fontCompiler;

    internal FontBuilder() : base("Font")
    {
        _fontCompiler = new FontCompiler();
    }

    protected override void Build(string rootPath, FontAssetInfo assetInfoType)
    {
        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"Building asset: {assetInfoType.Id}");

        var fontData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = AssetDataIO.SaveAssetData(rootPath, fontData, assetInfoType);

        //#if DEBUG

        //        var debugFontImagePath = Path.Combine(Path.GetDirectoryName(assetOutPutPath)!, Path.GetFileNameWithoutExtension(assetOutPutPath) + ".png");
        //        var debugFontTextPath = Path.Combine(Path.GetDirectoryName(assetOutPutPath)!, Path.GetFileNameWithoutExtension(assetOutPutPath) + ".txt");

        //        File.WriteAllBytes(debugFontImagePath, fontData.ImageData.Data);

        //        StringBuilder glyphRegions = new();

        //        foreach (var (glyphKey, glyph) in fontData.Glyphs)
        //        {
        //            glyphRegions.AppendLine($"Index: {glyphKey}\nGlyph:\n{glyph.ToString()}");
        //        }

        //        File.WriteAllText(debugFontTextPath, glyphRegions.ToString());

        //#endif

        Console.WriteLine($"Asset {assetInfoType.Id} built successfully on path {assetOutPutPath}");

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

        var fontData = _fontCompiler.Build(assetInfoType.Id, fontFileData, DEFAULT_BITMAP_FONT_IMAGE_SIZE, assetInfoType.FontSize, range, padding: 1);

        return fontData;
    }
}
