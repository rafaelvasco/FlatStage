using MemoryPack;

namespace FlatStage;

[MemoryPackable]
public partial class SoundData : AssetData
{
    public required byte[] Data { get; init; }

    public required AudioType Type { get; init; }

    public override string ToString()
    {
        return $"Id: {Id}\nData: {Data.Length}\nType: {Type}";
    }

    public override bool IsValid()
    {
        return Data is { Length: > 0 };
    }
}
