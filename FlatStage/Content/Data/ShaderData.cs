namespace FlatStage;

public class ShaderData : AssetData
{
    public byte[]? VertexShader { get; init; }

    public byte[]? FragmentShader { get; init; }

    public string[]? Samplers { get; init; }

    public string[]? Params { get; init; }

    public override string ToString()
    {
        return
            $"\nId: {Id}\n[VertexShader: {VertexShader?.Length}\nFragmentShader: {FragmentShader?.Length}\nSamplers: {Samplers?.Length}\nParams: {Params?.Length}]";
    }

    public override bool IsValid()
    {
        return
            VertexShader is { Length: > 0 } &&
            FragmentShader is { Length: > 0 } &&
            Samplers != null &&
            Params != null;
    }
}