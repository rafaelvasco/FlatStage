namespace FlatStage;

internal readonly struct ShaderCompileResult
{
    public readonly byte[] VsBytes;

    public readonly byte[] FsBytes;

    public readonly string[] Samplers;

    public readonly string[] Params;

    public ShaderCompileResult(byte[] vs, byte[] fs, string[] samplers, string[] @params)
    {
        VsBytes = vs;
        FsBytes = fs;
        Samplers = samplers;
        Params = @params;
    }
}