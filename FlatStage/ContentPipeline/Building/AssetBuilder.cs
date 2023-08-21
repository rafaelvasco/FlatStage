using System;
using System.Collections.Generic;
using System.IO;

namespace FlatStage.ContentPipeline;

internal static partial class AssetBuilder
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
        var assetsManifest = Content.LoadDefinitionData<AssetsManifest>(manifestPath);

        String builtPath = string.Empty;

        try
        {
            var assetInfo = assetsManifest.GetAssetInfo(assetId);

            builtPath = BuildAsset(rootFolder, assetInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to build asset {assetId}: {e.Message}");

            if (!string.IsNullOrEmpty(builtPath))
            {
                File.Delete(builtPath);
            }
        }
    }

    private static string BuildAsset(string rootFolder, AssetInfo assetInfo)
    {
        return _builders[assetInfo.GetType()].Build(rootFolder, assetInfo);
    }

    public static void BuildAssets(string rootFolder)
    {
        var manifestPath =
            Path.Combine(rootFolder, ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        var assetsManifest = Content.LoadDefinitionData<AssetsManifest>(manifestPath);

        Console.WriteLine("\nBuilding Assets:\n");

        List<string> _builtFilePaths = new();

        void Cleanup()
        {
            foreach (var path in _builtFilePaths)
            {
                File.Delete(path);
            }
        }

        if (assetsManifest.Images != null)
        {
            Console.WriteLine("Building Images: \n");
            foreach (var (_, imageAssetInfo) in assetsManifest.Images)
            {
                try
                {
                    var path = BuildAsset(rootFolder, imageAssetInfo);
                    _builtFilePaths.Add(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build image {imageAssetInfo.Id}: {e.Message}");
                    Cleanup();
                }

            }
        }

        if (assetsManifest.Shaders != null)
        {
            Console.WriteLine("Building Shaders: \n");
            foreach (var (_, shaderAssetInfo) in assetsManifest.Shaders)
            {
                try
                {
                    var path = BuildAsset(rootFolder, shaderAssetInfo);
                    _builtFilePaths.Add(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build shader {shaderAssetInfo.Id}: {e.Message}");
                    Cleanup();
                    return;
                }
            }
        }

        if (assetsManifest.Audios != null)
        {
            Console.WriteLine("Building Audios: \n");
            foreach (var (_, audioAssetInfo) in assetsManifest.Audios)
            {
                try
                {
                    var path = BuildAsset(rootFolder, audioAssetInfo);
                    _builtFilePaths.Add(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build audio {audioAssetInfo.Id}: {e.Message}");
                    Cleanup();
                    return;
                }
            }
        }

        if (assetsManifest.Fonts != null)
        {
            Console.WriteLine("Building Fonts: \n");

            foreach (var (_, fontAssetInfo) in assetsManifest.Fonts)
            {
                try
                {
                    var path = BuildAsset(rootFolder, fontAssetInfo);
                    _builtFilePaths.Add(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build font {fontAssetInfo.Id}: {e.Message}");
                    Cleanup();
                    return;
                }
            }
        }

        Console.WriteLine(!assetsManifest.IsEmpty ? "All assets built successfully!" : "Nothing to build, exiting.");
    }
}