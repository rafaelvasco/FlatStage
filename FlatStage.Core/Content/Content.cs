using System.Reflection;
using System.Text;

namespace FlatStage;

public static class Content
{
    public static readonly string RootPath = Path.Combine(AppContext.BaseDirectory, ContentProperties.AssetsFolder);

    private static readonly Dictionary<string, Asset> _loadedAssets = new();

    private static readonly List<Asset> _runtimeAssets = [];
    
    private static AssetsManifest _manifest = null!;

    private static readonly Dictionary<Type, IAssetLoader> _assetLoaders = new()
    {
        {
            typeof(Texture), new TextureLoader()
        },
        {
            typeof(TextureFont), new FontLoader()
        },
        {
            typeof(Sound), new AudioLoader()
        },
        {
            typeof(ShaderProgram), new ShaderLoader()
        }
    };


    internal static void Init()
    {
        var path = Path.Combine(RootPath, ContentProperties.AssetsManifestFile);
        _manifest = DefinitionIO.LoadDefinitionData<AssetsManifest>(path);
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

        var assetInfo = _manifest.GetAssetInfo(assetId);
        
        var asset = assetLoader.Load(RootPath, assetInfo);

        return (T)asset;
    }

    internal static string LoadEmbeddedRaw(string folderName, string fileName)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(folderName);
        path.Append('.');
        path.Append(fileName);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset on folder {folderName} with file name {fileName}");
        }

        using var reader = new StreamReader(fileStream);

        return reader.ReadToEnd();

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
