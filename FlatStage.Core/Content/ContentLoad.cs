using System.Reflection;
using System.Text;

namespace FlatStage;

internal interface IAssetLoader
{
    Asset Load(string rootPath, AssetInfo assetInfo);

    Asset LoadEmbedded(string assetId);
}

internal abstract class AssetLoader<T> : IAssetLoader where T : Asset
{
    public abstract string EmbeddedFolderName { get; }

    public Asset Load(Stream stream)
    {
        return LoadFromStream(stream);
    }

    public virtual Asset Load(string rootPath, AssetInfo info)
    {
        var assetFullPath = BuildFullPath(rootPath, info.Id);

        using var stream = File.OpenRead(assetFullPath);

        return LoadFromStream(stream);
    }

    public virtual Asset LoadEmbedded(string assetId)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(EmbeddedFolderName);
        path.Append('.');
        path.Append(assetId);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {assetId}");
        }

        return LoadFromStream(fileStream);
    }

    protected abstract T LoadFromStream(Stream stream);

    protected virtual string BuildFullPath(string rootPath, string assetId)
    {
        return Path.Combine(rootPath, assetId + ContentProperties.BinaryExt);
    }
}

internal class TextureLoader : AssetLoader<Texture>
{
    public override string EmbeddedFolderName => "Textures";

    protected override Texture LoadFromStream(Stream stream)
    {
        var assetData = AssetDataIO.LoadAssetData<ImageData>(stream);

        return Texture.LoadFromData(assetData);
    }
}

internal class ShaderLoader : AssetLoader<ShaderProgram>
{
    public override string EmbeddedFolderName => "Shaders";

    protected override ShaderProgram LoadFromStream(Stream stream)
    {
        var assetData = AssetDataIO.LoadAssetData<ShaderData>(stream);

        return ShaderProgram.LoadFromData(assetData);
    }

    protected override string BuildFullPath(string rootPath, string assetId)
    {
        var assetFullBinPath = Path.Combine(rootPath,
            assetId + ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend] +
            ContentProperties.BinaryExt);

        return assetFullBinPath;
    }

    public override Asset LoadEmbedded(string assetId)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(EmbeddedFolderName);
        path.Append('.');
        path.Append(assetId);
        path.Append(ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend]);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {assetId}");
        }

        return LoadFromStream(fileStream);
    }
}

internal class AudioLoader : AssetLoader<Sound>
{
    public override string EmbeddedFolderName => "Audios";

    public override Asset Load(string rootPath, AssetInfo info)
    {
        var fullPath = BuildFullPath(rootPath, info.Id);

        var streamFromDisk = (info as AudioAssetInfo)!.Type == AudioType.Song;
        
        return Sound.LoadFromFile(info.Id, fullPath, streamFromDisk);
    }

    protected override Sound LoadFromStream(Stream stream)
    {
        var assetData = AssetDataIO.LoadAssetData<SoundData>(stream);
        return Sound.LoadFromData(assetData);
    }
}

internal class FontLoader : AssetLoader<TextureFont>
{
    public override string EmbeddedFolderName => "Fonts";

    protected override TextureFont LoadFromStream(Stream stream)
    {
        var assetData = AssetDataIO.LoadAssetData<FontData>(stream);

        return TextureFont.LoadFromData(assetData);
    }
}
