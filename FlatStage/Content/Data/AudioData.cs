using FlatStage.Sound;
using MemoryPack;

namespace FlatStage.Content;

[MemoryPackable]
internal partial class AudioData : AssetData
{
    public required byte[] Data { get; init; }

    public required AudioType Type { get; init; }

    public required AudioFormat Format { get; init; }

    public override string ToString()
    {
        return $"Id: {Id}\nData: {Data.Length}\nType: {Type}";
    }

    public override bool IsValid()
    {
        return Data is { Length: > 0 };
    }
}
