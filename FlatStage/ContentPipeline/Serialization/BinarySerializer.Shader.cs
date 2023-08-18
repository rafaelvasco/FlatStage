using System;
using System.IO;

namespace FlatStage.ContentPipeline;

public static partial class BinarySerializer
{
    private static void SerializeShaderData(Stream stream, ShaderData shader)
    {
        IDefinitionData.ThrowIfInValid(shader, "BinarySerializer::SerializeShaderData");

#if DEBUG
        IDefinitionData.Debug(shader, "Writing Shader Data:");
#endif

        using var writer = new BinaryWriter(stream);

        writer.Write(ContentProperties.SerializationMagicString);
        writer.Write((byte)SerializationDataType.Shader);
        writer.Write(shader.Id!);
        writer.Write(shader.VertexShader!.Length);
        writer.Write(shader.VertexShader!);
        writer.Write(shader.FragmentShader!.Length);
        writer.Write(shader.FragmentShader!);

        if (shader.Samplers is { Length: > 0 })
        {
            writer.Write(string.Join(';', shader.Samplers));
        }
        else
        {
            writer.Write(string.Empty);
        }

        if (shader.Params != null && shader.Params.Length > 0)
        {
            writer.Write(string.Join(';', shader.Params!));
        }
        else
        {
            writer.Write(string.Empty);
        }
    }

    private static ShaderData DeserializeShaderData(Stream stream)
    {
        using var reader = new BinaryReader(stream);

        var magic = reader.ReadString();
        var type = reader.ReadByte();

        if (magic != ContentProperties.SerializationMagicString || type != (byte)SerializationDataType.Shader)
        {
            throw new InvalidDataException("Invalid ShaderData Binary File");
        }

        var id = reader.ReadString();
        var vertexShaderDataLength = reader.ReadInt32();
        var vertexShaderData = reader.ReadBytes(vertexShaderDataLength);
        var fragShaderDataLength = reader.ReadInt32();
        var fragShaderData = reader.ReadBytes(fragShaderDataLength);

        var samplersString = reader.ReadString();
        var paramsString = reader.ReadString();

        var samplers = samplersString.Split(';');
        var parameters = paramsString.Split(';');

        var shaderData = new ShaderData()
        {
            Id = id,
            VertexShader = vertexShaderData,
            FragmentShader = fragShaderData,
            Samplers = !string.IsNullOrEmpty(samplers[0]) ? samplers : Array.Empty<string>(),
            Params = !string.IsNullOrEmpty(parameters[0]) ? parameters : Array.Empty<string>()
        };

        IDefinitionData.ThrowIfInValid(shaderData, "BinarySerializer::DeserializeShaderData");

#if DEBUG
        IDefinitionData.Debug(shaderData, "Loaded Shader Data:");
#endif

        return shaderData;
    }
}