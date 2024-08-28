using MemoryPack;

namespace FlatStage;

[MemoryPackable]
public partial class AssetPak : AssetData
{
    public Dictionary<string, ImageData> Images { get; set; } = new();

    public Dictionary<string, ShaderData> Shaders { get; set; } = new();

    public Dictionary<string, AudioData> Audios { get; set; } = new();

    public Dictionary<string, FontData> Fonts { get; set; } = new();

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(Id);
    }
}
