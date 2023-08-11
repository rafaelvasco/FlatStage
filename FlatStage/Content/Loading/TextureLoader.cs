using System;
using System.IO;

namespace FlatStage;

internal class TextureLoader : AssetLoader<Texture2D>
{
    public override Texture2D Load(string id, AssetsManifest manifest)
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

    protected override Texture2D LoadFromStream(string assetid, Stream stream)
    {
        var imageData = LoadAssetData<ImageData>(assetid, stream);

        var texture = Graphics.CreateTexture(
            imageData.Id!,
            new Texture2DProps()
            {
                Data = imageData.Data,
                Width = imageData.Width,
                Height = imageData.Height,
                BytesPerPixel = imageData.BytesPerPixel
            }
        );

        return texture;
    }
}