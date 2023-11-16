using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlatStage.Content;

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
        var assetsManifest = Assets.LoadDefinitionData<AssetsManifest>(manifestPath);

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
        var manifestPath =
            Path.Combine(rootFolder, ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        var assetsManifest = Assets.LoadDefinitionData<AssetsManifest>(manifestPath);

        if (assetsManifest.IsEmpty)
        {
            Console.WriteLine("\nNothing to Build. Exiting\n");
            return;
        }

        //Cleanup(rootFolder);

        Console.WriteLine("\nBuilding Assets:\n");

        try
        {
            var nonPackedImages = assetsManifest.Images?.Values.Where(obj => string.IsNullOrEmpty(obj.TargetAssetPack));
            var nonPackedShaders = assetsManifest.Shaders?.Values.Where(obj => string.IsNullOrEmpty(obj.TargetAssetPack));
            var nonPackedFonts = assetsManifest.Fonts?.Values.Where(obj => string.IsNullOrEmpty(obj.TargetAssetPack));
            var nonPackedAudios = assetsManifest.Audios?.Values.Where(obj => string.IsNullOrEmpty(obj.TargetAssetPack));

            BuildDirect(
                nonPackedImages,
                nonPackedShaders,
                nonPackedFonts,
                nonPackedAudios
            );

            var packedImages = assetsManifest.Images?.Values.Where(obj => !string.IsNullOrEmpty(obj.TargetAssetPack));
            var packedShaders = assetsManifest.Shaders?.Values.Where(obj => !string.IsNullOrEmpty(obj.TargetAssetPack));
            var packedFonts = assetsManifest.Fonts?.Values.Where(obj => !string.IsNullOrEmpty(obj.TargetAssetPack));
            var packedAudios = assetsManifest.Audios?.Values.Where(obj => !string.IsNullOrEmpty(obj.TargetAssetPack));

            BuildPacked(
                packedImages,
                packedShaders,
                packedFonts,
                packedAudios
            );

            Console.WriteLine("\nAll assets built successfully.\n");
        }
        catch (FlatException)
        {
            Console.WriteLine("Build Failed.");
            //Cleanup(rootFolder);
            throw;
        }

        return;

        void BuildDirect(
            IEnumerable<ImageAssetInfo>? images,
            IEnumerable<ShaderAssetInfo>? shaders,
            IEnumerable<FontAssetInfo>? fonts,
            IEnumerable<AudioAssetInfo>? audios
        )
        {
            if (
                (images == null || !images.Any()) &&
                (shaders == null || !shaders.Any()) &&
                (fonts == null || !fonts.Any()) &&
                (audios == null || !audios.Any()))
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
        }

        void BuildPacked(
            IEnumerable<ImageAssetInfo>? images,
            IEnumerable<ShaderAssetInfo>? shaders,
            IEnumerable<FontAssetInfo>? fonts,
            IEnumerable<AudioAssetInfo>? audios
        )
        {
            if (
              (images == null || !images.Any()) &&
              (shaders == null || !shaders.Any()) &&
              (fonts == null || !fonts.Any()) &&
              (audios == null || !audios.Any()))
            {
                return;
            }

            var assetPaks = new Dictionary<string, AssetPak>();

            AssetPak GetTargetAssetPak(AssetInfo info)
            {
                if (!assetPaks!.ContainsKey(info.TargetAssetPack!))
                {
                    var assetPak = new AssetPak() { Id = info.TargetAssetPack! };
                    assetPaks.Add(info.TargetAssetPack!, assetPak);
                }

                return assetPaks![info.TargetAssetPack!];
            }

            if (images != null)
            {
                Console.WriteLine("\nBuilding Packed Images: \n");
                foreach (var imageAssetInfo in images)
                {
                    try
                    {
                        var assetData = BuildAsset<ImageAssetInfo, ImageData>(rootFolder, imageAssetInfo);
                        var targetAssetPak = GetTargetAssetPak(imageAssetInfo);
                        targetAssetPak.Images.Add(imageAssetInfo.Id, assetData);
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
                Console.WriteLine("\nBuilding Packed Shaders: \n");
                foreach (var shaderAssetInfo in shaders)
                {
                    try
                    {
                        var assetData = BuildAsset<ShaderAssetInfo, ShaderData>(rootFolder, shaderAssetInfo);
                        var targetAssetPak = GetTargetAssetPak(shaderAssetInfo);
                        targetAssetPak.Shaders.Add(shaderAssetInfo.Id, assetData);
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
                Console.WriteLine("\nBuilding Packed Audios: \n");
                foreach (var audioAssetInfo in audios)
                {
                    try
                    {
                        var assetData = BuildAsset<AudioAssetInfo, AudioData>(rootFolder, audioAssetInfo);
                        var targetAssetPak = GetTargetAssetPak(audioAssetInfo);
                        targetAssetPak.Audios.Add(audioAssetInfo.Id, assetData);
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
                Console.WriteLine("\nBuilding Packed Fonts: \n");
                foreach (var fontAssetInfo in fonts)
                {
                    try
                    {
                        var assetData = BuildAsset<FontAssetInfo, FontData>(rootFolder, fontAssetInfo);
                        var targetAssetPak = GetTargetAssetPak(fontAssetInfo);
                        targetAssetPak.Fonts.Add(fontAssetInfo.Id, assetData);
                    }
                    catch (Exception e)
                    {
                        FlatException.Throw($"Failed to build font {fontAssetInfo.Id}: {e.Message}");
                        return;
                    }
                }
            }

            foreach (var (pakName, pak) in assetPaks)
            {
                string path = Path.Combine(rootFolder, ContentProperties.AssetsFolder, $"{pakName}{ContentProperties.BinaryExt}");

                AssetDataIO.SaveAssetData(path, pak);
            }
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
