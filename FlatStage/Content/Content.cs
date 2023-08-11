using System;
using System.Collections.Generic;
using System.IO;
using FlatStage.Sound;

namespace FlatStage;

public static class Content
{
    private static AssetsManifest? AssetsManifest { get; set; }

    private static readonly Dictionary<string, Asset> Assets = new();
    private static readonly Dictionary<string, AssetData> AssetDatas = new();


    private static readonly Lazy<TextureLoader> _textureLoader = new();

    private static readonly Lazy<ShaderLoader> _shaderLoader = new();

    private static readonly Lazy<AudioLoader> _audioLoader = new();

    internal static void Init()
    {
        var path = Path.Combine(ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        AssetsManifest = LoadDefinitionData<AssetsManifest>(path);

#if DEBUG
        Console.WriteLine("Loaded Assets Manifest: \n");
        Console.WriteLine(AssetsManifest);
#endif
    }

    public static T Get<T>(string assetId, bool embeddedAsset = false) where T : Asset
    {
        if (Assets.TryGetValue(assetId, out var cachedAsset))
        {
            if (cachedAsset is T value)
            {
                return value;
            }
        }

        Asset asset = !embeddedAsset ? Load<T>(assetId) : LoadEmbedded<T>(assetId);

        RegisterAsset(assetId, asset);

        return (T)asset;
    }

    public static void Free(string assetId)
    {
        if (Assets.TryGetValue(assetId, out var asset))
        {
            Free(asset);
        }
    }

    private static void Free(Asset asset)
    {
        Assets.Remove(asset.Id);
        asset.Dispose();
    }

    public static T LoadDefinitionData<T>(string filePath) where T : class, IDefinitionData
    {
#if DEBUG
        Console.WriteLine($"Loading Definition File At Path: {filePath}");
#endif
        try
        {
            var data = DefinitionSerializer.DeSerializeDefinitionFile<T>(filePath);

            return data;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to load definition file of type {typeof(T)}: {e.Message}");
        }
    }

    internal static StageSettings LoadSettingsOrDefault()
    {
        if (File.Exists(ContentProperties.GameSettingsFile))
        {
            var settings = LoadDefinitionData<StageSettings>(ContentProperties.GameSettingsFile);
            return settings;
        }

        var newSettings = new StageSettings();

        DefinitionSerializer.SerializeDefinitionFile(ContentProperties.GameSettingsFile, newSettings);

        return newSettings;
    }

    internal static void Shutdown()
    {
        Console.WriteLine("Content Shutdown...");
        foreach (var (_, asset) in Assets)
        {
            Console.WriteLine($"Freeing asset {asset.Id}...");

            asset.Dispose();
        }

        Assets.Clear();
    }

    private static T Load<T>(string assetId) where T : Asset
    {
        var assetLoader = GetLoader<T>();

        if (assetLoader == null) throw new ApplicationException("Could not get Loader for asset");

#if DEBUG
        Console.WriteLine($"\nLoading Asset: {assetId}");
#endif

        var asset = assetLoader.Load(assetId, AssetsManifest!);

        return asset;
    }

    private static T LoadEmbedded<T>(string assetId) where T : Asset
    {
#if DEBUG
        Console.WriteLine($"\nLoading Embedded Asset: {assetId}");
#endif

        var assetLoader = GetLoader<T>();

        if (assetLoader == null) throw new ApplicationException("Could not get Loader for asset");

        var asset = assetLoader.LoadEmbedded(assetId);

        return asset;
    }


    private static AssetLoader<T>? GetLoader<T>() where T : Asset
    {
        if (typeof(T) == typeof(Texture2D))
        {
            return _textureLoader.Value as AssetLoader<T>;
        }

        if (typeof(T) == typeof(ShaderProgram))
        {
            return _shaderLoader.Value as AssetLoader<T>;
        }

        if (typeof(T) == typeof(Audio))
        {
            return _audioLoader.Value as AssetLoader<T>;
        }

        throw new ArgumentException($"Unsupported asset type: {typeof(T).FullName}");
    }

    internal static void RegisterAssetData(string id, AssetData data)
    {
        AssetDatas.Add(id, data);
    }

    private static void RegisterAsset(string id, Asset asset)
    {
        if (Assets == null)
        {
            throw new ApplicationException($"Content not initialized");
        }

        Assets.Add(id, asset);
    }
}