using FlatStage.Graphics;
using FlatStage.IO;
using System;
using System.IO;

namespace FlatStage.ContentPipeline;
internal class ShaderBuilder : AssetBuilderAgent<ShaderData, ShaderAssetInfo>
{

    public ShaderBuilder() : base("Shader")
    {
    }

    protected override string Build(string rootPath, ShaderAssetInfo assetInfoType)
    {
        IDefinitionData.ThrowIfInValid(assetInfoType, $"AssetBuilder::{Name}");

        Console.WriteLine($"\nBuilding Asset {assetInfoType.Id} for Graphics Api: {GraphicsContext.GraphicsBackend}...\n");

        var shaderData = BuildAssetData(rootPath, assetInfoType);

        var assetOutPutPath = AssetDataIO.SaveAssetData(rootPath, shaderData, assetInfoType, fileNameAppend: ContentProperties.ShaderAppendStrings[GraphicsContext.GraphicsBackend]);

        Console.WriteLine($"Shader {assetInfoType.Id} for Graphics Api {GraphicsContext.GraphicsBackend} built successfully on path {assetOutPutPath}");

        return assetOutPutPath;
    }

    protected override ShaderData BuildAssetData(string rootPath, ShaderAssetInfo assetInfoType)
    {
        var vsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.VsPath!);
        var fsFullPath = Path.Combine(rootPath, ContentProperties.AssetsFolder, assetInfoType.FsPath!);

        var compileResult = ShaderCompiler.Compile(vsFullPath, fsFullPath, GraphicsContext.GraphicsBackend);

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
