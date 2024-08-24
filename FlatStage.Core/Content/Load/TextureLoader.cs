namespace FlatStage;

internal class TextureLoader : AssetLoader<Texture, ImageData>
{
    public override Texture Load(string id)
    {
        var assetFullBinPath =
            Path.Combine(Content.RootPath, id + ContentProperties.BinaryExt);

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

        var texture = Graphics.CreateTexture(
            assetData.Id,
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
