using FlatStage.Graphics;
using System.IO;

namespace FlatStage.ContentPipeline;
internal class ImageBuilder : AssetBuilderAgent<ImageData, ImageAssetInfo>
{
    public ImageBuilder() : base("Image")
    {
    }

    protected override ImageData BuildAssetData(string rootPath, ImageAssetInfo assetInfoType)
    {
        var (Data, Width, Height) = ImageIO.LoadPNG(Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.Path!));

        // Convert RgbaToBgra
        Blitter.Begin(Data, Width, Height);
        Blitter.ConvertRgbaToBgra(premultiplyAlpha: true);
        Blitter.End();

        ImageIO.SavePNGToMem(Data, Width, Height, out var finalImageData);

        var result = new ImageData
        {
            Id = assetInfoType.Id!,
            Data = finalImageData,
            Width = Width,
            Height = Height,
        };

        return result;
    }
}
