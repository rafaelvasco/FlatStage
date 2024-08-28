namespace FlatStage;

internal readonly struct ShaderCompileResult(byte[] vs, byte[] fs, string[] samplers, string[] @params)
{
    public readonly byte[] VsBytes = vs;

    public readonly byte[] FsBytes = fs;

    public readonly string[] Samplers = samplers;

    public readonly string[] Params = @params;
}
