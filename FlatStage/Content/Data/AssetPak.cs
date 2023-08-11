using System.Collections.Generic;

namespace FlatStage;

internal class AssetPak
{
    public string Name { get; set; }

    public Dictionary<string, ImageData> Images { get; set; }

    public Dictionary<string, ShaderData> Shaders { get; set; }

    public Dictionary<string, AudioData> Audios { get; set; }

    public AssetPak(string name)
    {
        Name = name;
        Images = new Dictionary<string, ImageData>();
        Shaders = new Dictionary<string, ShaderData>();
        Audios = new Dictionary<string, AudioData>();
    }
}