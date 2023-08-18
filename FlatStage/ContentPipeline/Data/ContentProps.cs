using System;
using System.Collections.Generic;

using FlatStage.Graphics;

namespace FlatStage.ContentPipeline;

internal static class ContentProperties
{
    public const string AssetsFolder = "Assets";
    public const string GameSettingsFile = "settings.toml";
    public const string AssetsManifestFile = "assets.toml";
    public const string BinaryExt = ".fsb";
    public const string SerializationMagicString = "FLATB";
    public const string EmbeddedAssetsNamespace = "FlatStage.Assets";

    private static readonly Dictionary<Type, string> EmbeddedFolders = new()
    {
        { typeof(Texture), "Textures" },
        { typeof(ShaderProgram), "Shaders" },
    };

    public static string GetEmbeddedFolderNameFromAssetType<T>() where T : Asset
    {
        return EmbeddedFolders[typeof(T)];
    }

    public static readonly Dictionary<GraphicsBackend, string> ShaderAppendStrings =
        new()
        {
            {
                GraphicsBackend.Direct3D11, "_D3D"
            },
            {
                GraphicsBackend.Direct3D12, "_D3D12"
            },
            {
                GraphicsBackend.OpenGl, "_GL"
            },
            {
                GraphicsBackend.Metal, "_MT"
            },
            {
                GraphicsBackend.Vulkan, "_VK"
            },
        };

}