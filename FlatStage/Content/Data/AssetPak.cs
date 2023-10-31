using MemoryPack;
using System.Collections.Generic;

namespace FlatStage.Content;

[MemoryPackable]
internal partial class AssetPak : AssetData
{
    public Dictionary<string, ImageData> Images { get; set; }

    public Dictionary<string, ShaderData> Shaders { get; set; }

    public Dictionary<string, AudioData> Audios { get; set; }

    public Dictionary<string, FontData> Fonts { get; set; }

    public AssetPak()
    {
        Images = new Dictionary<string, ImageData>();
        Shaders = new Dictionary<string, ShaderData>();
        Audios = new Dictionary<string, AudioData>();
        Fonts = new Dictionary<string, FontData>();
    }

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id) && Images != null && Shaders != null && Audios != null && Fonts != null;
    }
}
