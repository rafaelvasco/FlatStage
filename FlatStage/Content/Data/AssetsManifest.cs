using System.Collections.Generic;
using System.Text;
using FlatStage.Sound;

namespace FlatStage;

public class AssetsManifest : IDefinitionData
{
    public Dictionary<string, ImageAssetInfo>? Images { get; internal set; }

    public Dictionary<string, ShaderAssetInfo>? Shaders { get; internal set; }

    public Dictionary<string, AudioAssetInfo>? Audios { get; internal set; }

    internal bool IsEmpty => Images == null && Shaders == null && Audios == null;

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

        return debugString.ToString();
    }

    public bool IsValid()
    {
        return !IsEmpty;
    }
}

public abstract class AssetInfo : IDefinitionData
{
    public string? Id { get; init; }

    public abstract bool IsValid();
}

public class ImageAssetInfo : AssetInfo
{
    public string? Path { get; init; }

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
    public string? VsPath { get; init; }

    public string? FsPath { get; init; }

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
    public string? Path { get; set; }

    public AudioType Type { get; set; }

    public override string ToString()
    {
        return $"[Id: {Id}, Path: {Path}, Type: {Type}]";
    }

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Path);
    }
}