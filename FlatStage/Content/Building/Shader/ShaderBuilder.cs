using FlatStage.Graphics;
using System;
using System.IO;

namespace FlatStage.Content;
internal class ShaderBuilder : AssetBuilderAgent<ShaderData, ShaderAssetInfo>
{

    public ShaderBuilder() : base("Shader")
    {
    }

    private static GraphicsBackend GetBackendBuild()
    {
        if (OperatingSystem.IsWindows())
        {
            return GraphicsBackend.Direct3D11;
        }

        if (OperatingSystem.IsMacOS())
        {
            return GraphicsBackend.Metal;
        }

        if (OperatingSystem.IsLinux())
        {
            return GraphicsBackend.OpenGL;
        }

        return GraphicsBackend.Unknown;
    }

    protected override void Build(string rootPath, ShaderAssetInfo assetInfoType)
    {
        var graphicsBackend = GetBackendBuild();

        if (graphicsBackend == GraphicsBackend.Unknown)
        {
            FlatException.Throw("Unsupported Platform. Can't build shaders for it.");
            return;
        }

        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"\nBuilding Asset {assetInfoType.Id} for Graphics Api: {graphicsBackend}...\n");

        var shaderData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = AssetDataIO.SaveAssetData(rootPath, shaderData, assetInfoType, fileNameAppend: ContentProperties.ShaderAppendStrings[graphicsBackend]);

        Console.WriteLine($"Shader {assetInfoType.Id} for Graphics Api {graphicsBackend} built successfully on path {assetOutPutPath}");
    }

    protected override ShaderData BuildAssetData(string rootPath, ShaderAssetInfo assetInfoType)
    {
        var graphicsBackend = GetBackendBuild();

        var vsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.VsPath!);
        var fsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.FsPath!);

        var compileResult = ShaderCompiler.Compile(vsFullPath, fsFullPath, graphicsBackend);

        var data = new ShaderData
        {
            Id = assetInfoType.Id!,
            VertexShader = compileResult.VsBytes,
            FragmentShader = compileResult.FsBytes,
            Params = compileResult.Params,
            Samplers = compileResult.Samplers
        };

        return data;
    }
}
