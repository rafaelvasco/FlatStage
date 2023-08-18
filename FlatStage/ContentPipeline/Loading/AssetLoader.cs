using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlatStage.ContentPipeline;

internal abstract class AssetLoader<T> where T : Asset
{
    public abstract T Load(string id, AssetsManifest manifest);

    public virtual T LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(ContentProperties.GetEmbeddedFolderNameFromAssetType<T>());
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
        }

        return LoadFromStream(id, fileStream);
    }

    protected abstract T LoadFromStream(string assetId, Stream stream);

    protected static T2 LoadAssetData<T2>(string assetId, Stream stream) where T2 : AssetData
    {
        var data = BinarySerializer.Deserialize<T2>(stream);
        Content.RegisterAssetData(assetId, data);
        return data;
    }
}