using System;
using System.IO;

using FlatStage.Graphics;

namespace FlatStage.ContentPipeline;

internal class TextureLoader : AssetLoader<Texture>
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

            return LoadFromStream(id, stream);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    protected override Texture LoadFromStream(string assetId, Stream stream)
    {
        var imageData = LoadAssetData<ImageData>(assetId, stream);

        var decodedImageData = ImageIO.LoadPNGFromMem(imageData.Data);

        var texture = GraphicsContext.CreateTexture(
            imageData.Id!,
            new TextureProps()
            {
                Data = decodedImageData.Data,
                Width = imageData.Width,
                Height = imageData.Height
            }
        );

        return texture;
    }
}