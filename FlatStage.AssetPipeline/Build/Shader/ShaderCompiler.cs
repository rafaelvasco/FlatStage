
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace FlatStage;

internal static partial class ShaderCompiler
{
    private static readonly string CompilerPath;

    static ShaderCompiler()
    {
        CompilerPath = Path.Combine(
            AppContext.BaseDirectory,
            OperatingSystem.IsWindows() ?
                "./ShaderCompiler/shaderc.exe" :
                "./ShaderCompiler/shaderc"
        );
    }

    private const string IncludePath = "ShaderCompiler";
    private const string SamplerRegexVar = "sampler";
    private const string SamplerRegex = @"SAMPLER2D\s*\(\s*(?<sampler>\w+)\s*\,\s*(?<index>\d+)\s*\)\s*\;";
    private const string ParamRegexVar = "param";
    private const string VecParamRegex = @"uniform\s+vec4\s+(?<param>\w+)\s*\;";

    private const string D3DCompileParams =
        "--platform windows -p $profile_5_0 -O 3 --type $type -f $path -o $output -i $include";

    private const string GlslCompileParams =
        "--platform linux --type $type -f $path -o $output -i $include";

    private const string MetalCompileParams =
        "--platform osx -p metal --type $type -f $path -o $output -i $include";

    public static ShaderCompileResult Compile(string vsSrcPath, string fsSrcPath, GraphicsBackend graphicsBackend)
    {
        var vsBuildResult = string.Empty;
        var fsBuildResult = string.Empty;

        var processInfo = new ProcessStartInfo
        {
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            FileName = CompilerPath
        };

        var compileParams = graphicsBackend switch
        {
            GraphicsBackend.Direct3D11 => D3DCompileParams,
            GraphicsBackend.OpenGL => GlslCompileParams,
            GraphicsBackend.Metal => MetalCompileParams,
            _ => throw new ApplicationException($"Unsupported Graphics Backend for shader compilation: {graphicsBackend}")
        };

        var vsArgs = new StringBuilder(compileParams);

        vsArgs.Replace("$path", vsSrcPath);
        vsArgs.Replace("$type", "vertex");

        if (graphicsBackend == GraphicsBackend.Direct3D11)
        {
            vsArgs.Replace("$profile", "vs");
        }

        var tempVsBinOutput = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(vsSrcPath) + ".bin");

        vsArgs.Replace("$output", tempVsBinOutput);
        vsArgs.Replace("$include", IncludePath);

        processInfo.Arguments = vsArgs.ToString();

        var procVs = Process.Start(processInfo);

        procVs?.WaitForExit();

        var outputVs = procVs?.ExitCode ?? -1;

        if (outputVs != 0 && outputVs != -1)
        {
            using var reader = procVs?.StandardOutput;
            vsBuildResult = reader?.ReadToEnd();
        }

        var fsArgs = new StringBuilder(compileParams);

        fsArgs.Replace("$path", fsSrcPath);
        fsArgs.Replace("$type", "fragment");

        if (graphicsBackend == GraphicsBackend.Direct3D11)
        {
            fsArgs.Replace("$profile", "ps");
        }

        var tempFsBinOutput = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fsSrcPath) + ".bin");
        fsArgs.Replace("$output", tempFsBinOutput);

        fsArgs.Replace("$include", IncludePath);

        processInfo.Arguments = fsArgs.ToString();

        var procFs = Process.Start(processInfo);

        procFs?.WaitForExit();

        var outputFs = procFs?.ExitCode ?? -1;

        if (outputFs != 0 && outputFs != -1)
        {
            using var reader = procFs?.StandardOutput;
            fsBuildResult = reader?.ReadToEnd();
        }

        bool vsBinGenerated = File.Exists(tempVsBinOutput);
        bool fsBinGenerated = File.Exists(tempFsBinOutput);

        if (vsBinGenerated && fsBinGenerated)
        {
            var vsBytes = File.ReadAllBytes(tempVsBinOutput);
            var fsBytes = File.ReadAllBytes(tempFsBinOutput);

            var fsStream = File.OpenRead(fsSrcPath);

            ParseUniforms(fsStream, out var samplers, out var @params);

            var result = new ShaderCompileResult(
                vsBytes,
                fsBytes,
                samplers,
                @params
            );

            File.Delete(tempVsBinOutput);
            File.Delete(tempFsBinOutput);

            return result;
        }

        if (vsBinGenerated)
        {
            File.Delete(tempVsBinOutput);
        }

        if (fsBinGenerated)
        {
            File.Delete(tempFsBinOutput);
        }

        throw new ApplicationException(
            $"Shader Compilation Error on {vsSrcPath} and {fsSrcPath}: VSResult: {vsBuildResult}, FSResult: {fsBuildResult}");
    }

    private static void ParseUniforms(FileStream fsStream, out string[] samplers, out string[] @params)
    {
        var samplerRegex = SamplerRegexGen();
        var paramRegex = VecParamRegexGen();

        var samplersList = new List<string>();
        var paramsList = new List<string>();

        using var reader = new StreamReader(fsStream);

        while (reader.ReadLine() is { } line)
        {
            Match samplerMatch = samplerRegex.Match(line);

            if (samplerMatch.Success)
            {
                string samplerName = samplerMatch.Groups[SamplerRegexVar].Value;
                samplersList.Add(samplerName);
            }
            else
            {
                Match paramMatch = paramRegex.Match(line);

                if (!paramMatch.Success) continue;

                string paramName = paramMatch.Groups[ParamRegexVar].Value;

                paramsList.Add(paramName);
            }
        }

        samplers = samplersList.Count > 0 ? samplersList.ToArray() : [];

        @params = paramsList.Count > 0 ? paramsList.ToArray() : [];
    }

    [GeneratedRegex(SamplerRegex)]
    private static partial Regex SamplerRegexGen();

    [GeneratedRegex(VecParamRegex)]
    private static partial Regex VecParamRegexGen();
}
