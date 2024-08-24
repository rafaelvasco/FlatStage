using System.Reflection;
using System.Text;

namespace FlatStage;

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
        path.Append(ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend]);
        path.Append(ContentProperties.BinaryExt);

        using var fileStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(path.ToString());

        if (fileStream == null)
        {
            throw new ApplicationException($"Could not load embedded asset {id} on path {path}");
        }

        var shaderData = Content.LoadAssetData<ShaderData>(id, fileStream);

        return LoadFromAssetData(shaderData);
    }

    public override ShaderProgram Load(string id)
    {
        var assetFullBinPath = Path.Combine(Content.RootPath,
            id + ContentProperties.ShaderAppendStrings[Graphics.GraphicsBackend] +
            ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            var shaderData = Content.LoadAssetData<ShaderData>(id, stream);

            return LoadFromAssetData(shaderData);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    public override ShaderProgram LoadFromAssetData(ShaderData assetData)
    {
        var shader = Graphics.CreateShader(assetData.Id, new ShaderProgramProps()
        {
            VertexShader = assetData.VertexShader,
            FragmentShader = assetData.FragmentShader,
            Parameters = assetData.Params,
            Samplers = assetData.Samplers
        });

        return shader;
    }
}
