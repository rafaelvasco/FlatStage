using System;
using System.IO;

namespace FlatStage;

public enum SerializationDataType : byte
{
    Image = 0,
    Shader = 1,
    Audio
}

public static partial class BinarySerializer
{
    public static void Serialize<T>(Stream stream, ref T value) where T : AssetData
    {
        if (typeof(T) == typeof(ImageData))
        {
            SerializeImageData(stream, (value as ImageData)!);
        }
        else if (typeof(T) == typeof(ShaderData))
        {
            SerializeShaderData(stream, (value as ShaderData)!);
        }
        else if (typeof(T) == typeof(AudioData))
        {
            SerializeAudioData(stream, (value as AudioData)!);
        }
        else
        {
            throw new ArgumentException("Invalid Serialization Data Type", nameof(T));
        }
    }

    public static T Deserialize<T>(Stream stream) where T : AssetData
    {
        if (typeof(T) == typeof(ImageData))
        {
            if (DeserializeImageData(stream) is not T imageData)
            {
                throw new ApplicationException("Could not Deserialize ImageData.");
            }

            return imageData;
        }

        if (typeof(T) == typeof(ShaderData))
        {
            if (DeserializeShaderData(stream) is not T shaderData)
            {
                throw new ApplicationException("Could not Deserialize ShaderData.");
            }

            return shaderData;
        }

        if (typeof(T) == typeof(AudioData))
        {
            if (DeserializeAudioData(stream) is not T audioData)
            {
                throw new ApplicationException("Could not Deserialize AudioData");
            }

            return audioData;
        }

        throw new ArgumentException("Invalid Deserialization Data Type", nameof(T));
    }
}