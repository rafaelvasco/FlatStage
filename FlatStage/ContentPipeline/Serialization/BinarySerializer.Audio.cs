using System.IO;

using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

public static partial class BinarySerializer
{
    private static void SerializeAudioData(Stream stream, AudioData audio)
    {
        IDefinitionData.ThrowIfInValid(audio, "BinarySerializer::SerializeAudioData");

#if DEBUG
        IDefinitionData.Debug(audio, "Writing Audio Data:");
#endif

        using var writer = new BinaryWriter(stream);

        writer.Write(ContentProperties.SerializationMagicString);
        writer.Write((byte)SerializationDataType.Audio);
        writer.Write(audio.Id!);
        writer.Write(audio.Data!.Length);
        writer.Write(audio.Data);
        writer.Write(audio.Type.ToString());
        writer.Write(audio.Format.ToString());
    }

    private static AudioData DeserializeAudioData(Stream stream)
    {
        using var reader = new BinaryReader(stream);

        var magic = reader.ReadString();
        var type = reader.ReadByte();

        if (magic != ContentProperties.SerializationMagicString || type != (byte)SerializationDataType.Audio)
        {
            throw new InvalidDataException("Invalid AudioData Binary File");
        }

        var id = reader.ReadString();
        var dataLength = reader.ReadInt32();
        var data = reader.ReadBytes(dataLength);
        var audioType = reader.ReadString();
        var audioFormat = reader.ReadString();

        var audioData = new AudioData()
        {
            Id = id,
            Data = data,
            Type = Audio.ParseAudioTypeFromString(audioType),
            Format = Audio.ParseAudioFormatFromString(audioFormat),
        };

#if DEBUG
        IDefinitionData.Debug(audioData, "Loaded Audio Data:");
#endif

        IDefinitionData.ThrowIfInValid(audioData, "BinarySerializer::DeserializeAudioData");

        return audioData;
    }
}