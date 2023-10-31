using MemoryPack;
using System.Collections.Generic;

namespace FlatStage.Content;

[MemoryPackable]
internal partial class GameSaveData : AssetData
{
    public required Dictionary<string, string> StringValues { get; internal set; }

    public required Dictionary<string, int> IntValues { get; internal set; }

    public required Dictionary<string, float> FloatValues { get; internal set; }

    public override bool IsValid()
    {
        return true;
    }

}
