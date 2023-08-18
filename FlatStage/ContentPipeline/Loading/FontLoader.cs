using FlatStage.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlatStage.ContentPipeline;
internal class FontLoader : AssetLoader<TextureFont>
{
    public override TextureFont Load(string id, AssetsManifest manifest)
    {
        if (manifest.Fonts?.TryGetValue(id, out var fontAssetInfo) != true)
            throw new ApplicationException($"Could not find asset with Id: {id}");

        IDefinitionData.ThrowIfInValid(fontAssetInfo, "FontLoader::Load");

        var assetFullBinPath =
            Path.Combine(ContentProperties.AssetsFolder, fontAssetInfo!.Id + ContentProperties.BinaryExt);

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

    protected override TextureFont LoadFromStream(string assetId, Stream stream)
    {
        var fontData = LoadAssetData<FontData>(assetId, stream);

        var decodedImageData = ImageIO.LoadPNGFromMem(fontData.ImageData.Data);

        var texture = GraphicsContext.CreateTexture(
            assetId + "_Texture",
            new TextureProps()
            {
                Data = decodedImageData.Data,
                Width = fontData.Width,
                Height = fontData.Height
            }
        );

        var glyphBounds = new List<Rect>();
        var cropping = new List<Rect>();
        var chars = new List<char>();
        var kerning = new List<Vec3>();

        var orderedKeys = fontData.Glyphs.Keys.OrderBy(a => a);

        foreach (var key in orderedKeys)
        {
            var character = fontData.Glyphs[key];

            var bounds = new Rect(character.X, character.Y,
                character.Width,
                character.Height);

            glyphBounds.Add(bounds);
            cropping.Add(new Rect(character.XOffset, character.YOffset, bounds.Width, bounds.Height));

            chars.Add((char)key);

            kerning.Add(new Vec3(0, bounds.Width, character.XAdvance - bounds.Width));
        }

        var textureFont = new TextureFont(
            assetId,
            texture,
            glyphBounds,
            cropping,
            chars,
            20,
            0,
            kerning,
            ' '
        );

        return textureFont;
    }
}
