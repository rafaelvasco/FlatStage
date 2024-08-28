using MemoryPack;

namespace FlatStage;

[MemoryPackable]
public partial class GameSaveData : AssetData
{
    public required Dictionary<string, string> StringValues { get; set; }

    public required Dictionary<string, int> IntValues { get; set; }

    public required Dictionary<string, float> FloatValues { get; set; }

    public override bool IsValid()
    {
        return true;
    }

}
