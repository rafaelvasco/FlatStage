using System;
using System.IO;

using FlatStage.Graphics;

namespace FlatStage.ContentPipeline;

internal class TextureLoader : AssetLoader<Texture, ImageData>
{
    public override Texture Load(string id, AssetsManifest manifest)
    {
        if (manifest.Images?.TryGetValue(id, out var imageAssetInfo) != true)
            throw new ApplicationException($"Could not find asset with Id: {id}");

        IDefinitionData.ThrowIfInValid(imageAssetInfo, "TextureLoader::Load");

        var assetFullBinPath =
            Path.Combine(ContentProperties.AssetsFolder, imageAssetInfo!.Id + ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            var imageData = Content.LoadAssetData<ImageData>(id, stream);

            return LoadFromAssetData(imageData);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    public override Texture LoadFromAssetData(ImageData assetData)
    {
        var (Data, _, _) = ImageIO.LoadPNGFromMem(assetData.Data);

        var texture = GraphicsContext.CreateTexture(
            assetData.Id!,
            new TextureProps()
            {
                Data = Data,
                Width = assetData.Width,
                Height = assetData.Height
            }
        );

        return texture;
    }
}
