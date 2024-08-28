using MemoryPack;
using MemoryPack.Compression;

namespace FlatStage;
public static class AssetDataIO
{
    public static T LoadAssetData<T>(Stream stream) where T : AssetData
    {
        using var binaryReader = new BinaryReader(stream);

        var buffer = binaryReader.ReadBytes((int)stream.Length);

        using var decompressor = new BrotliDecompressor();

        var decompressedBuffer = decompressor.Decompress(buffer);

        var data = MemoryPackSerializer.Deserialize<T>(decompressedBuffer) ?? throw new Exception("Could not load AssetData.");

        return data;
    }

    public static string SaveAssetData<T>(string rootPath, T data, AssetInfo assetInfo, string? fileNameAppend = null) where T : AssetData
    {
        var assetDirectory = Path.GetDirectoryName(assetInfo.PrimaryPath) ?? string.Empty;

        var assetFullBinPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
           assetDirectory,
           assetInfo.Id + (fileNameAppend ?? string.Empty) + ContentProperties.BinaryExt);

        SaveAssetData(assetFullBinPath, data);

        return assetFullBinPath;
    }

    public static void SaveAssetData<T>(string targetPath, T data) where T : AssetData
    {
        using var stream = File.Open(targetPath, FileMode.OpenOrCreate);

        using var compressor = new BrotliCompressor();

        MemoryPackSerializer.Serialize(in compressor, in data);

        using var writer = new BinaryWriter(stream);

        writer.Write(compressor.ToArray());

        writer.Close();
    }

    public static void SaveAssetPak(string rootPath, AssetPak pak)
    {
        var fullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
         pak.Id + ContentProperties.BinaryExt);

        SaveAssetData(fullPath, pak);
    }
}
