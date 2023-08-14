using System;
using System.IO;

namespace FlatStage;

internal static partial class AssetBuilder
{
    private static string BuildAndExportShader(string rootPath, ShaderAssetInfo shaderAssetInfo,
        GraphicsBackend graphicsBackend)
    {
        IDefinitionData.ThrowIfInValid(shaderAssetInfo, "AssetBuilder::BuildAndExportShader");

        Console.WriteLine($"Building Shader {shaderAssetInfo.Id} for Graphics Api: {graphicsBackend}...");

        var shaderData = BuildShaderData(rootPath, shaderAssetInfo, graphicsBackend);

        var assetDirectory = Path.GetDirectoryName(shaderAssetInfo.VsPath) ?? "";

        var assetFullBinPath = Path.Combine(rootPath, ContentProperties.AssetsFolder,
            assetDirectory,
            shaderAssetInfo.Id + ContentProperties.ShaderAppendStrings[graphicsBackend] + ContentProperties.BinaryExt);

        using var stream = File.OpenWrite(assetFullBinPath);

        BinarySerializer.Serialize(stream, ref shaderData);

        Console.WriteLine($"Shader {shaderAssetInfo.Id} for Graphics Api {graphicsBackend} built successfully.");

        return assetFullBinPath;
    }

    private static ShaderData BuildShaderData(string rootPath, ShaderAssetInfo shaderAssetInfo,
        GraphicsBackend graphicsBackend)
    {
        var vsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, shaderAssetInfo.VsPath!);
        var fsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, shaderAssetInfo.FsPath!);

        var compileResult = ShaderCompiler.Compile(vsFullPath, fsFullPath, graphicsBackend);

        var data = new ShaderData
        {
            Id = shaderAssetInfo.Id!,
            VertexShader = compileResult.VsBytes,
            FragmentShader = compileResult.FsBytes,
            Params = compileResult.Params,
            Samplers = compileResult.Samplers
        };

        return data;
    }
}