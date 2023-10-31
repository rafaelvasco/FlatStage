using System;
using System.IO;
using System.Reflection;
using System.Text;

using FlatStage.Graphics;

namespace FlatStage.Content;

internal class ShaderLoader : AssetLoader<ShaderProgram, ShaderData>
{
    public override ShaderProgram LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(ContentProperties.GetEmbeddedFolderNameFromAssetType<ShaderProgram>());
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.ShaderAppendStrings[GraphicsContext.GraphicsBackend]);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
        }

        var shaderData = Assets.LoadAssetData<ShaderData>(id, fileStream);

        return LoadFromAssetData(shaderData);
    }

    public override ShaderProgram Load(string id, AssetsManifest manifest)
    {
        if (manifest.Shaders?.TryGetValue(id, out var shaderAssetInfo) != true)
            throw new ApplicationException($"Could not find asset with Id: {id}");

        IDefinitionData.ThrowIfInValid(shaderAssetInfo!, "ShaderLoader::Load");

        var assetFullBinPath = Path.Combine(ContentProperties.AssetsFolder,
            shaderAssetInfo!.Id + ContentProperties.ShaderAppendStrings[GraphicsContext.GraphicsBackend] +
            ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            var shaderData = Assets.LoadAssetData<ShaderData>(id, stream);

            return LoadFromAssetData(shaderData);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    public override ShaderProgram LoadFromAssetData(ShaderData assetData)
    {
        var shader = GraphicsContext.CreateShader(assetData.Id!, new ShaderProgramProps()
        {
            VertexShader = assetData.VertexShader,
            FragmentShader = assetData.FragmentShader,
            Parameters = assetData.Params,
            Samplers = assetData.Samplers
        });

        return shader;
    }
}
