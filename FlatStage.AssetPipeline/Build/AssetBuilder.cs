namespace FlatStage;

public static class AssetBuilder
{
    private static readonly Dictionary<Type, IAssetBuilder> _builders = new()
    {
        {
            typeof(ImageAssetInfo), new ImageBuilder()
        },
        {
            typeof(AudioAssetInfo), new AudioBuilder()
        },
        {
            typeof(ShaderAssetInfo), new ShaderBuilder()
        },
        {
            typeof(FontAssetInfo), new FontBuilder()
        },
    };

    public static void BuildAsset(string rootFolder, string assetId)
    {
        var manifestPath =
            Path.Combine(rootFolder, ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        var assetsManifest = DefinitionIO.LoadDefinitionData<AssetsManifest>(manifestPath);

        try
        {
            var assetInfo = assetsManifest.GetAssetInfo(assetId);

            BuildAsset(rootFolder, assetInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to build asset {assetId}: {e.Message}");

            Cleanup(rootFolder);
        }
    }

    private static void BuildAsset<T>(string rootFolder, T assetInfo) where T : AssetInfo
    {
        _builders[typeof(T)].Build(rootFolder, assetInfo);
    }

    private static AssetDataType BuildAsset<AssetInfoType, AssetDataType>(string rootFolder, AssetInfoType assetInfo) where AssetInfoType : AssetInfo where AssetDataType : AssetData
    {
        return (AssetDataType)_builders[typeof(AssetInfoType)].BuildData(rootFolder, assetInfo);
    }

    public static void BuildAssets(string rootFolder)
    {
        foreach (var (_, value) in _builders)
        {
            value.Init();
        }

        var manifestPath =
            Path.Combine(rootFolder, ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);

        var assetsManifest = DefinitionIO.LoadDefinitionData<AssetsManifest>(manifestPath);

        if (assetsManifest.IsEmpty)
        {
            Console.WriteLine("\nNothing to Build. Exiting\n");
            return;
        }

        Console.WriteLine("\nBuilding Assets:\n");

        try
        {
            var images =
                assetsManifest.Images?.Values;
            var shaders =
                assetsManifest.Shaders?.Values;
            var fonts =
                assetsManifest.Fonts?.Values;
            var audios =
                assetsManifest.Audios?.Values;

            if (
                (images == null || images.Count == 0) &&
                (shaders == null || shaders.Count == 0) &&
                (fonts == null || fonts.Count == 0) &&
                (audios == null || audios.Count == 0))
            {
                return;
            }

            if (images != null)
            {
                Console.WriteLine("\nBuilding Images: \n");
                foreach (var imageAssetInfo in images)
                {
                    try
                    {
                        BuildAsset(rootFolder, imageAssetInfo);
                    }
                    catch (Exception e)
                    {
                        FlatException.Throw($"Failed to build image {imageAssetInfo.Id}: {e.Message}");
                        return;
                    }
                }
            }

            if (shaders != null)
            {
                Console.WriteLine("\nBuilding Shaders: \n");
                foreach (var shaderAssetInfo in shaders)
                {
                    try
                    {
                        BuildAsset(rootFolder, shaderAssetInfo);
                    }
                    catch (Exception e)
                    {
                        FlatException.Throw($"Failed to build shader {shaderAssetInfo.Id}: {e.Message}");
                        return;
                    }
                }
            }

            if (audios != null)
            {
                Console.WriteLine("\nBuilding Audios: \n");
                foreach (var audioAssetInfo in audios)
                {
                    try
                    {
                        BuildAsset(rootFolder, audioAssetInfo);
                    }
                    catch (Exception e)
                    {
                        FlatException.Throw($"Failed to build audio {audioAssetInfo.Id}: {e.Message}");
                        return;
                    }
                }
            }

            if (fonts != null)
            {
                Console.WriteLine("\nBuilding Fonts: \n");
                foreach (var fontAssetInfo in fonts)
                {
                    try
                    {
                        BuildAsset(rootFolder, fontAssetInfo);
                    }
                    catch (Exception e)
                    {
                        FlatException.Throw($"Failed to build font {fontAssetInfo.Id}: {e.Message}");
                        return;
                    }
                }
            }

            Console.WriteLine("\nAll assets built successfully.\n");
        }
        catch (FlatException)
        {
            Console.WriteLine("Build Failed.");
            //Cleanup(rootFolder);
            throw;
        }

        foreach (var (_, value) in _builders)
        {
            value.Cleanup();
        }
    }

    private static void Cleanup(string assetsFolder)
    {
        var assetFiles = Directory.GetFiles(
            assetsFolder,
            $"*{ContentProperties.BinaryExt}",
            SearchOption.AllDirectories);

        foreach (var file in assetFiles)
        {
            File.Delete(file);
        }
    }
}
