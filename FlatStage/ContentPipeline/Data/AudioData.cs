using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

internal class AudioData : AssetData
{
    public required byte[] Data { get; init; }

    public AudioType Type { get; init; }

    public override string ToString()
    {
        return $"Id: {Id}\nData: {Data.Length}\nType: {Type}";
    }

    public override bool IsValid()
    {
        return Data is { Length: > 0 };
    }
}