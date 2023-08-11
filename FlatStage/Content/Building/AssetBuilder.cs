using System;
using System.Collections.Generic;
using System.IO;

namespace FlatStage;

internal static partial class AssetBuilder
{
    public static void BuildAssets(string rootFolder)
    {
        var manifestPath =
            Path.Combine(rootFolder, ContentProperties.AssetsFolder, ContentProperties.AssetsManifestFile);
        var assetsManifest = Content.LoadDefinitionData<AssetsManifest>(manifestPath);

        var builtBinariesPaths = new List<string>();

        void Cleanup()
        {
            foreach (var path in builtBinariesPaths)
            {
                File.Delete(path);
            }
        }

        Console.WriteLine($"Building Assets:\n");

        if (assetsManifest.Images != null)
        {
            foreach (var (_, imageAssetInfo) in assetsManifest.Images)
            {
                try
                {
                    var exportedBinPath = BuildAndExportImage(rootFolder, imageAssetInfo);
                    builtBinariesPaths.Add(exportedBinPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build texture {imageAssetInfo.Id}: {e.Message}");
                    Cleanup();
                    return;
                }
            }
        }

        if (assetsManifest.Shaders != null)
        {
            foreach (var (_, shaderAssetInfo) in assetsManifest.Shaders)
            {
                try
                {
                    var exportedBinShaderD3DPath =
                        BuildAndExportShader(rootFolder, shaderAssetInfo, GraphicsBackend.Direct3D11);
                    var exportedBinShaderGlPath =
                        BuildAndExportShader(rootFolder, shaderAssetInfo, GraphicsBackend.OpenGl);

                    builtBinariesPaths.Add(exportedBinShaderD3DPath);
                    builtBinariesPaths.Add(exportedBinShaderGlPath);
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
            foreach (var (_, audioAssetInfo) in assetsManifest.Audios)
            {
                try
                {
                    var exportedBinPath = BuildAndExportAudio(rootFolder, audioAssetInfo);
                    builtBinariesPaths.Add(exportedBinPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to build audio {audioAssetInfo.Id}: {e.Message}");
                    Cleanup();
                    return;
                }
            }
        }

        Console.WriteLine(!assetsManifest.IsEmpty ? "All assets built successfully!" : "Nothing to build, exiting.");
    }
}