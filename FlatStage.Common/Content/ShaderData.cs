using MemoryPack;

namespace FlatStage;

[MemoryPackable]
public partial class ShaderData : AssetData
{
    public required byte[] VertexShader { get; init; }

    public required byte[] FragmentShader { get; init; }

    public required string[] Samplers { get; init; }

    public required string[] Params { get; init; }

    public override string ToString()
    {
        return
            $"\nId: {Id}\n[VertexShader: {VertexShader.Length}\nFragmentShader: {FragmentShader.Length}\nSamplers: {Samplers.Length}\nParams: {Params.Length}]";
    }

    public override bool IsValid()
    {
        return
            VertexShader is { Length: > 0 } &&
            FragmentShader is { Length: > 0 };
    }
}
