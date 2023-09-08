using System;
using System.Collections.Generic;
using System.Text;
using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

public class AssetsManifest : IDefinitionData
{
    public Dictionary<string, ImageAssetInfo>? Images { get; internal set; }

    public Dictionary<string, ShaderAssetInfo>? Shaders { get; internal set; }

    public Dictionary<string, AudioAssetInfo>? Audios { get; internal set; }

    public Dictionary<string, FontAssetInfo>? Fonts { get; internal set; }

    private Dictionary<string, AssetInfo>? FlatMap;

    public AssetInfo GetAssetInfo(string id)
    {
        if (FlatMap == null)
        {
            BuildFlapMap();
        }

        if (FlatMap!.TryGetValue(id, out var assetInfo))
        {
            return assetInfo;
        }

        throw new Exception($"Could not find Asset: {id}");
    }

    private void BuildFlapMap()
    {
        FlatMap = new Dictionary<string, AssetInfo>();

        if (Images != null)
        {
            foreach (var (key, imageInfo) in Images)
            {
                FlatMap[key] = imageInfo;
            }
        }

        if (Shaders != null)
        {
            foreach (var (key, shaderInfo) in Shaders)
            {
                FlatMap[key] = shaderInfo;
            }
        }

        if (Audios != null)
        {
            foreach (var (key, audioInfo) in Audios)
            {
                FlatMap[key] = audioInfo;
            }
        }

        if (Fonts != null)
        {
            foreach (var (key, fontInfo) in Fonts)
            {
                FlatMap[key] = fontInfo;
            }
        }
    }

    internal bool IsEmpty => Images == null && Shaders == null && Audios == null && Fonts == null;

    public override string ToString()
    {
        var debugString = new StringBuilder();

        debugString.AppendLine("Images:");

        if (Images != null)
        {
            foreach (var image in Images)
            {
                debugString.AppendLine(image.ToString());
            }
        }
        else
        {
            debugString.AppendLine("None");
        }

        debugString.AppendLine("Shaders:");

        if (Shaders != null)
        {
            foreach (var shader in Shaders)
            {
                debugString.AppendLine(shader.ToString());
            }
        }
        else
        {
            debugString.AppendLine("None");
        }

        debugString.AppendLine("Audios:");

        if (Audios != null)
        {
            foreach (var audio in Audios)
            {
                debugString.AppendLine(audio.ToString());
            }
        }
        else
        {
            debugString.AppendLine("None");
        }

        debugString.AppendLine("Fonts:");

        if (Fonts != null)
        {
            foreach (var font in Fonts)
            {
                debugString.AppendLine(font.ToString());
            }
        }
        else
        {
            debugString.AppendLine("None");
        }

        return debugString.ToString();
    }

    public bool IsValid()
    {
        return !IsEmpty;
    }
}

public abstract class AssetInfo : IDefinitionData
{
    public required string Id { get; init; }

    public string? TargetAssetPack { get; init; }

    public abstract string PrimaryPath { get; }

    public abstract bool IsValid();
}

public class ImageAssetInfo : AssetInfo
{
    public required string Path { get; init; }

    public override string PrimaryPath => Path;

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Path);
    }

    public override string ToString()
    {
        return $"[Id: {Id}, Path: {Path}]";
    }
}

public class ShaderAssetInfo : AssetInfo
{
    public required string VsPath { get; init; }

    public required string FsPath { get; init; }

    public override string PrimaryPath => VsPath;

    public override string ToString()
    {
        return $"[Id: {Id}, VsPath: {VsPath}, FsPath: {FsPath}]";
    }

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(VsPath) && !string.IsNullOrEmpty(FsPath);
    }
}

public class AudioAssetInfo : AssetInfo
{
    public required string Path { get; init; }

    public AudioType Type { get; set; }

    public override string PrimaryPath => Path;

    public override string ToString()
    {
        return $"[Id: {Id}, Path: {Path}, Type: {Type}]";
    }

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Path);
    }
}

public class FontAssetInfo : AssetInfo
{
    public required string Path { get; init; }

    public string? CharRange { get; internal set; }

    public int FontSize { get; init; }

    public override string PrimaryPath => Path;

    public override bool IsValid()
    {
        return Path.Length > 0 && FontSize > 0;
    }
}
