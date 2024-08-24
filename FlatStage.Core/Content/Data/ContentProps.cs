using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlatStage;

public static class ContentProperties
{
    public const string AssetsFolder = "Assets";
    public const string AssetsManifestFile = "assets.json";
    public const string GameProjectFile = "project.json";
    public const string BinaryExt = ".fsb";
    public const string SerializationMagicString = "FLATB";
    public const string EmbeddedAssetsNamespace = "FlatStage.Engine.Assets";
    public const string DefaultAssetsPackName = "MainAssets";

    private static readonly Dictionary<Type, string> EmbeddedFolders = new()
    {
        { typeof(Texture), "Textures" },
        { typeof(ShaderProgram), "Shaders" },
        { typeof(TextureFont), "Fonts" },
    };

    public static JsonSerializerOptions JsonSettings { get; } = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new ColorJsonConverter(),
        }
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
                GraphicsBackend.OpenGL, "_GL"
            },
            {
                GraphicsBackend.Metal, "_MT"
            },
            {
                GraphicsBackend.Vulkan, "_VK"
            },
        };

}
