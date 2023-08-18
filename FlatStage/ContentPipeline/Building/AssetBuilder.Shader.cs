using FlatStage.Graphics;
using FlatStage.IO;
using System;
using System.IO;

namespace FlatStage.ContentPipeline;

internal static partial class AssetBuilder
{
    private static string BuildAndExportShader(string rootPath, ShaderAssetInfo shaderAssetInfo,
        GraphicsBackend graphicsBackend)
    {
        IDefinitionData.ThrowIfInValid(shaderAssetInfo, "AssetBuilder::BuildAndExportShader");

        Console.WriteLine($"Building Shader {shaderAssetInfo.Id} for Graphics Api: {graphicsBackend}...");

        var shaderData = BuildShaderData(rootPath, shaderAssetInfo, graphicsBackend);

        var assetOutPutPath = BinaryIO.SaveAssetData(rootPath, ref shaderData, shaderAssetInfo, fileNameAppend: ContentProperties.ShaderAppendStrings[graphicsBackend]);

        Console.WriteLine($"Shader {shaderAssetInfo.Id} for Graphics Api {graphicsBackend} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
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