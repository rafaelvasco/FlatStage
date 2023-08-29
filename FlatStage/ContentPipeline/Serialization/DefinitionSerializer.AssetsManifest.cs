using System.Collections.Generic;
using System.IO;

using Tommy;
using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

public static partial class DefinitionSerializer
{
    private static AssetsManifest DeSerializeAssetsManifest(string filePath)
    {
        using var reader = File.OpenText(filePath);

        TomlTable table = TOML.Parse(reader);

        var shadersNodeArray = table["shaders"].AsArray;
        var imagesNodeArray = table["images"].AsArray;
        var audiosNodeArray = table["audios"].AsArray;
        var fontsNodeArray = table["fonts"].AsArray;

        var manifest = new AssetsManifest();

        if (shadersNodeArray != null)
        {
            manifest.Shaders = new Dictionary<string, ShaderAssetInfo>();

            foreach (var item in shadersNodeArray.Children)
            {
                var id = item["id"];
                var vsPath = item["vsPath"];
                var fsPath = item["fsPath"];

                var shaderAssetInfo = new ShaderAssetInfo()
                {
                    Id = id,
                    VsPath = vsPath,
                    FsPath = fsPath
                };

                IDefinitionData.ThrowIfInValid(shaderAssetInfo,
                    "DefinitionSerializer::DeSerializeAssetsManifest::Shader");

                manifest.Shaders.Add(id, shaderAssetInfo);
            }
        }

        if (imagesNodeArray != null)
        {
            manifest.Images = new Dictionary<string, ImageAssetInfo>();

            foreach (var item in imagesNodeArray.Children)
            {
                var id = item["id"];
                var path = item["path"];

                var imageAssetInfo = new ImageAssetInfo()
                {
                    Id = id,
                    Path = path
                };

                IDefinitionData.ThrowIfInValid(imageAssetInfo,
                    "DefinitionSerializer::DeSerializeAssetsManifest::Image");

                manifest.Images.Add(id, imageAssetInfo);
            }
        }

        if (audiosNodeArray != null)
        {
            manifest.Audios = new Dictionary<string, AudioAssetInfo>();

            foreach (var item in audiosNodeArray.Children)
            {
                var id = item["id"];
                var path = item["path"];
                var type = item["type"];

                var audioAsssetInfo = new AudioAssetInfo()
                {
                    Id = id,
                    Path = path,
                    Type = Audio.ParseAudioTypeFromString(type)
                };

                IDefinitionData.ThrowIfInValid(audioAsssetInfo,
                    "DefinitionSerializer::DeSerializeAssetsManifest::Audio");

                manifest.Audios.Add(id, audioAsssetInfo);
            }
        }

        if (fontsNodeArray != null)
        {
            manifest.Fonts = new Dictionary<string, FontAssetInfo>();

            foreach (var item in fontsNodeArray.Children)
            {
                var id = item["id"];
                var path = item["path"];
                var fontSize = item["fontSize"];
                string charRange = item["charRange"].AsString;

                var fontAssetInfo = new FontAssetInfo()
                {
                    Id = id,
                    FontSize = fontSize.IsInteger ? fontSize : int.Parse(fontSize),
                    Path = path,
                    CharRange = charRange!
                };

                IDefinitionData.ThrowIfInValid(fontAssetInfo,
                       "DefinitionSerializer::DeSerializeAssetsManifest::Font");

                manifest.Fonts.Add(id, fontAssetInfo);
            }

        }

        IDefinitionData.ThrowIfInValid(manifest, "DefinitionSerializer::DeSerializeAssetsManifest::Manifest");

        return manifest;
    }

    private static void SerializeAssetsManifest(AssetsManifest manifest, string filePath)
    {
        IDefinitionData.ThrowIfInValid(manifest, "DefinitionSerializer::SerializeAssetsManifest");

        TomlTable toml = new()
        {
            ["shaders"] = new TomlArray
            {
                IsTableArray = true
            },
            ["images"] = new TomlArray
            {
                IsTableArray = true
            },
            ["audios"] = new TomlArray()
            {
                IsTableArray = true
            }
            ["fonts"] = new TomlArray()
            {
                IsTableArray = true
            }
        };

        if (manifest.Shaders?.Count > 0)
        {
            var shadersNode = toml["shaders"].AsArray;

            foreach (var (_, shader) in manifest.Shaders!)
            {
                IDefinitionData.ThrowIfInValid(shader, "DefinitionSerializer::SerializeAssetsManifest::Shader");

                shadersNode!.Add(new TomlTable()
                {
                    ["id"] = shader.Id!,
                    ["vsPath"] = shader.VsPath!,
                    ["fsPath"] = shader.FsPath!
                });
            }
        }

        if (manifest.Images?.Count > 0)
        {
            var imagesNode = toml["images"].AsArray;

            foreach (var (_, image) in manifest.Images!)
            {
                IDefinitionData.ThrowIfInValid(image, "DefinitionSerializer::SerializeAssetsManifest::Image");

                imagesNode!.Add(new TomlTable()
                {
                    ["id"] = image.Id!,
                    ["path"] = image.Path!,
                });
            }
        }

        if (manifest.Audios?.Count > 0)
        {
            var audiosNode = toml["audios"].AsArray;

            foreach (var (_, audio) in manifest.Audios)
            {
                IDefinitionData.ThrowIfInValid(audio, "DefinitionSerializer::SerializeAssetsManifest::Audio");

                audiosNode!.Add(new TomlTable()
                {
                    ["id"] = audio.Id!,
                    ["path"] = audio.Path!,
                    ["type"] = audio.Type.ToString()
                });
            }
        }

        if (manifest.Fonts?.Count > 0)
        {
            var fontsNode = toml["fonts"].AsArray;

            foreach (var (_, font) in manifest.Fonts)
            {
                IDefinitionData.ThrowIfInValid(font, "DefinitionSerializer::SerializeAssetsManifest:Font");

                var fontNode = new TomlTable()
                {
                    ["id"] = font.Id!,
                    ["path"] = font.Path!,
                    ["fontSize"] = font.FontSize
                };

                if (font.CharRange != null)
                {
                    fontNode.Add("charRange", font.CharRange);
                }

                fontsNode!.Add(fontNode);
            }
        }

        File.WriteAllText(filePath, toml.ToInlineToml());
    }
}