using FlatStage.ContentPipeline;
using System.IO;

namespace FlatStage.IO;
internal static class BinaryIO
{
    public static string SaveAssetData<T>(string rootPath, ref T data, AssetInfo assetInfo, string? fileNameAppend = null) where T : AssetData
    {
        var assetDirectory = Path.GetDirectoryName(assetInfo.PrimaryPath) ?? string.Empty;

        var assetFullBinPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
           assetDirectory,
           assetInfo.Id + (fileNameAppend ?? string.Empty) + ContentProperties.BinaryExt);

        if (File.Exists(assetFullBinPath))
        {
            File.Delete(assetFullBinPath);
        }

        using var stream = File.Open(assetFullBinPath, FileMode.Create);

        BinarySerializer.Serialize(stream, ref data);

        return assetFullBinPath;
    }
}
