using System;
using System.Collections.Generic;
using System.IO;

using FlatStage.Graphics;
using FlatStage.IO;
using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

public static class Content
{
    private static AssetsManifest AssetsManifest { get; set; } = null!;

    private static readonly Dictionary<string, Asset> Assets = new();
    private static readonly Dictionary<string, AssetData> AssetDatas = new();

    private static readonly Dictionary<Type, IAssetLoader> _loaders = new()
    {
        { typeof(Texture), new TextureLoader() },
        { typeof(TextureFont), new FontLoader() },
        { typeof(ShaderProgram), new ShaderLoader() },
        { typeof(Audio), new AudioLoader() },
    };

    internal static void Init()
    {
        var path = Path.Combine(ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        AssetsManifest = LoadDefinitionData<AssetsManifest>(path);

#if DEBUG
        Console.WriteLine("Loaded Assets Manifest: \n");
        Console.WriteLine(AssetsManifest);
#endif
    }

    public static T Get<T>(string assetId) where T : Asset
    {
        return Get<T>(assetId, false);
    }

    internal static T Get<T>(string assetId, bool embeddedAsset = false) where T : Asset
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

    public static void LoadPak(string id)
    {
        var pakPath = Path.Combine(ContentProperties.AssetsFolder, $"{id}{ContentProperties.BinaryExt}");

        if (!File.Exists(pakPath))
        {
            throw new FileNotFoundException($"Could not find AssetPak: {id}");
        }

        using var stream = File.OpenRead(pakPath);

        var assetPak = LoadAssetData<AssetPak>(id, stream);

        if (assetPak.Images.Count > 0)
        {
            var loader = GetLoader<Texture>();

            foreach (var (key, imageData) in assetPak.Images)
            {
                var texture = loader.LoadFromAssetData(imageData);
                RegisterAsset(key, texture);
            }
        }

        if (assetPak.Shaders.Count > 0)
        {
            var loader = GetLoader<ShaderProgram>();

            foreach (var (key, shaderData) in assetPak.Shaders)
            {
                var shader = loader.LoadFromAssetData(shaderData);
                RegisterAsset(key, shader);
            }
        }

        if (assetPak.Fonts.Count > 0)
        {
            var loader = GetLoader<TextureFont>();

            foreach (var (key, fontData) in assetPak.Fonts)
            {
                var font = loader.LoadFromAssetData(fontData);
                RegisterAsset(key, font);
            }
        }

        if (assetPak.Audios.Count > 0)
        {
            var loader = GetLoader<Audio>();

            foreach (var (key, audioData) in assetPak.Audios)
            {
                var audio = loader.LoadFromAssetData(audioData);
                RegisterAsset(key, audio);
            }
        }
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

    private static AssetType Load<AssetType>(string assetId) where AssetType : Asset
    {
        var assetLoader = GetLoader<AssetType>();

        if (assetLoader == null) throw new ApplicationException("Could not get Loader for asset");

#if DEBUG
        Console.WriteLine($"\nLoading Asset: {assetId}");
#endif

        var asset = assetLoader.Load(assetId, AssetsManifest!);

        return (AssetType)asset;
    }

    private static T LoadEmbedded<T>(string assetId) where T : Asset
    {
#if DEBUG
        Console.WriteLine($"\nLoading Embedded Asset: {assetId}");
#endif

        var assetLoader = GetLoader<T>();

        if (assetLoader == null) throw new ApplicationException("Could not get Loader for asset");

        var asset = assetLoader.LoadEmbedded(assetId);

        return (T)asset;
    }

    private static IAssetLoader GetLoader<AssetType>() where AssetType : Asset
    {
        if (_loaders.TryGetValue(typeof(AssetType), out var loader))
        {
            return loader;
        }

        throw new ArgumentException($"Unsupported asset type: {typeof(AssetType).FullName}");
    }

    internal static AssetDataType LoadAssetData<AssetDataType>(string assetId, Stream stream) where AssetDataType : AssetData
    {
        var data = AssetDataIO.LoadAssetData<AssetDataType>(stream);

        RegisterAssetData(assetId, data);
        return data;
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
