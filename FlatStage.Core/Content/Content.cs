namespace FlatStage;

public static class Content
{
    public static readonly string RootPath = Path.Combine(AppContext.BaseDirectory, ContentProperties.AssetsFolder);

    private static readonly Dictionary<string, Asset> _loadedAssets = new();

    private static readonly List<Asset> _runtimeAssets = [];

    private static readonly Dictionary<Type, IAssetLoader> _assetLoaders = new()
    {
        {
            typeof(Texture), new TextureLoader()
        },
        {
            typeof(TextureFont), new FontLoader()
        },
        {
            typeof(Audio), new AudioLoader()
        },
        {
            typeof(ShaderProgram), new ShaderLoader()
        }
    };


    internal static void Init()
    {
        //var path = Path.Combine(RootPath, ContentProperties.AssetsManifestFile);
        //AssetsManifest = LoadDefinitionData<AssetsManifest>(path);

//#if DEBUG
//        Console.WriteLine("Loaded Assets Manifest: \n");
//        Console.WriteLine(AssetsManifest);
//#endif
    }

    public static T Get<T>(string assetId) where T : Asset
    {
        return Get<T>(assetId, false);
    }

    internal static T Get<T>(string assetId, bool embeddedAsset) where T : Asset
    {
        if (_loadedAssets.TryGetValue(assetId, out var cachedAsset))
        {
            if (cachedAsset is T value)
            {
                return value;
            }
        }

        Asset asset = !embeddedAsset ? LoadInternal<T>(assetId) : LoadEmbedded<T>(assetId);

        RegisterAsset(assetId, asset);

        return (T)asset;
    }

    public static void LoadPak(string id)
    {
        var pakPath = Path.Combine(RootPath, $"{id}{ContentProperties.BinaryExt}");

        if (!File.Exists(pakPath))
        {
            throw new FileNotFoundException($"Could not find AssetPak: {id}");
        }

        using var stream = File.OpenRead(pakPath);

        var assetPak = AssetDataIO.LoadAssetData<AssetPak>(stream);

        if (assetPak.Images.Count > 0)
        {
            foreach (var (key, imageData) in assetPak.Images)
            {
                var texture = Texture.LoadFromData(imageData);
                RegisterAsset(key, texture);
            }
        }

        if (assetPak.Shaders.Count > 0)
        {
            foreach (var (key, shaderData) in assetPak.Shaders)
            {
                var shader = ShaderProgram.LoadFromData(shaderData);
                RegisterAsset(key, shader);
            }
        }

        if (assetPak.Fonts.Count > 0)
        {
            foreach (var (key, fontData) in assetPak.Fonts)
            {
                var font = TextureFont.LoadFromData(fontData);
                RegisterAsset(key, font);
            }
        }

        if (assetPak.Audios.Count > 0)
        {
            foreach (var (key, audioData) in assetPak.Audios)
            {
                var audio = Audio.LoadFromData(audioData);
                RegisterAsset(key, audio);
            }
        }
    }

    private static IAssetLoader GetLoader<T>() where T : Asset
    {
        return _assetLoaders[typeof(T)];
    }

    public static void Free(string assetId)
    {
        if (_loadedAssets.TryGetValue(assetId, out var asset))
        {
            asset.Dispose();
            _loadedAssets.Remove(assetId);
        }
    }

    internal static void Shutdown()
    {
        Console.WriteLine("Content Shutdown...");
        foreach (var (_, asset) in _loadedAssets)
        {
            Console.WriteLine($"Freeing asset {asset.Id}...");

            asset.Dispose();
        }

        foreach (var asset in _runtimeAssets)
        {
            Console.WriteLine($"Freeing Runtime {asset.GetType()}");
            asset.Dispose();
        }

        _loadedAssets.Clear();
        _runtimeAssets.Clear();
    }

    private static T LoadInternal<T>(string assetId) where T : Asset
    {
        var assetLoader = GetLoader<T>();

#if DEBUG
        Console.WriteLine($"\nLoading Asset: {assetId}");
#endif

        var asset = assetLoader.Load(RootPath, assetId);

        return (T)asset;
    }

    private static T LoadEmbedded<T>(string assetId) where T : Asset
    {
#if DEBUG
        Console.WriteLine($"\nLoading Embedded Asset: {assetId}");
#endif

        var assetLoader = GetLoader<T>();

        var asset = assetLoader.LoadEmbedded(assetId);

        return (T)asset;
    }

    private static void RegisterAsset(string id, Asset asset)
    {
        if (_loadedAssets == null)
        {
            throw new ApplicationException($"Content not initialized");
        }

        _loadedAssets.Add(id, asset);
    }

    internal static void RegisterAsset(Asset asset)
    {
        _runtimeAssets.Add(asset);
    }
}
