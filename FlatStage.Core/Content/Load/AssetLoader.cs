using System.Reflection;
using System.Text;

namespace FlatStage;

internal interface IAssetLoader
{
    Asset Load(string id);
    Asset LoadEmbedded(string id);
    Asset LoadFromAssetData(AssetData assetData);
}

internal abstract class AssetLoader<AssetType, AssetDataType> : IAssetLoader where AssetType : Asset where AssetDataType : AssetData
{
    Asset IAssetLoader.Load(string id)
    {
        return Load(id);
    }

    Asset IAssetLoader.LoadEmbedded(string id)
    {
        return LoadEmbedded(id);
    }

    public Asset LoadFromAssetData(AssetData assetData)
    {
        return LoadFromAssetData((assetData as AssetDataType ?? throw new InvalidOperationException()));
    }

    public abstract AssetType Load(string id);

    public virtual AssetType LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(ContentProperties.GetEmbeddedFolderNameFromAssetType<AssetType>());
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
        }

        var assetData = Content.LoadAssetData<AssetDataType>(id, fileStream);

        return LoadFromAssetData(assetData);
    }

    public abstract AssetType LoadFromAssetData(AssetDataType assetData);

}
