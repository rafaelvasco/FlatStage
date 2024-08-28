using System.Reflection;
using System.Text;

namespace FlatStage;

internal interface IAssetLoader
{
    Asset Load(Stream stream);
    Asset Load(string rootPath, string assetId);

    Asset LoadEmbedded(string id);
}

internal abstract class AssetLoader<T> : IAssetLoader where T : Asset
{
    public abstract string EmbeddedFolderName { get; }

    public Asset Load(Stream stream)
    {
        return LoadFromStream(stream);
    }

    public Asset Load(string rootPath, string id)
    {
        var assetFullPath = BuildFullPath(rootPath, id);

        using var stream = File.OpenRead(assetFullPath);

        return LoadFromStream(stream);
    }

    public virtual Asset LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(EmbeddedFolderName);
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
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

    public override Asset LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(EmbeddedFolderName);
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend]);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
        }

        return LoadFromStream(fileStream);
    }
}

internal class AudioLoader : AssetLoader<Audio>
{
    public override string EmbeddedFolderName => "Audios";

    protected override Audio LoadFromStream(Stream stream)
    {
        var assetData = AssetDataIO.LoadAssetData<AudioData>(stream);

        return Audio.LoadFromData(assetData);
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
