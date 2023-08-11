using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlatStage;

internal class ShaderLoader : AssetLoader<ShaderProgram>
{
    public override ShaderProgram LoadEmbedded(string id)
    {
        var path = new StringBuilder();

        path.Append(ContentProperties.EmbeddedAssetsNamespace);
        path.Append('.');
        path.Append(ContentProperties.GetEmbeddedFolderNameFromAssetType<ShaderProgram>());
        path.Append('.');
        path.Append(id);
        path.Append(ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend]);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset: {id}");
        }

        return LoadFromStream(id, fileStream);
    }

    public override ShaderProgram Load(string id, AssetsManifest manifest)
    {
        if (manifest.Shaders?.TryGetValue(id, out var shaderAssetInfo) != true)
            throw new ApplicationException($"Could not find asset with Id: {id}");

        IDefinitionData.ThrowIfInValid(shaderAssetInfo!, "ShaderLoader::Load");

        var assetFullBinPath = Path.Combine(ContentProperties.AssetsFolder,
            shaderAssetInfo!.Id + ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend] +
            ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            return LoadFromStream(id, stream);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    protected override ShaderProgram LoadFromStream(string id, Stream stream)
    {
        var shaderData = LoadAssetData<ShaderData>(id, stream);

        var shader = Graphics.CreateShader(shaderData.Id!, new ShaderProgramProps()
        {
            VertexShader = shaderData.VertexShader,
            FragmentShader = shaderData.FragmentShader,
            Parameters = shaderData.Params,
            Samplers = shaderData.Samplers
        });

        return shader;
    }
}