using System.IO;

namespace FlatStage.ContentPipeline;

public static partial class BinarySerializer
{
    private static void SerializeImageData(Stream stream, ImageData image)
    {
        IDefinitionData.ThrowIfInValid(image, "BinarySerializer::SerializeImageData");

#if DEBUG
        IDefinitionData.Debug(image, "Writing Image Data:");
#endif

        using var writer = new BinaryWriter(stream);

        writer.Write(ContentProperties.SerializationMagicString);
        writer.Write((byte)SerializationDataType.Image);
        writer.Write(image.Id!);
        writer.Write(image.Data!.Length);
        writer.Write(image.Data!);
        writer.Write(image.Width);
        writer.Write(image.Height);
    }

    private static ImageData DeserializeImageData(Stream stream)
    {
        using var reader = new BinaryReader(stream);

        var magic = reader.ReadString();
        var type = reader.ReadByte();

        if (magic != ContentProperties.SerializationMagicString || type != (byte)SerializationDataType.Image)
        {
            throw new InvalidDataException("Invalid ImageData Binary File");
        }

        var id = reader.ReadString();
        var dataLength = reader.ReadInt32();
        var data = reader.ReadBytes(dataLength);
        var width = reader.ReadInt32();
        var height = reader.ReadInt32();

        var imageData = new ImageData()
        {
            Id = id,
            Data = data,
            Width = width,
            Height = height,
        };

#if DEBUG
        IDefinitionData.Debug(imageData, "Loaded Image Data:");
#endif

        IDefinitionData.ThrowIfInValid(imageData, "BinarySerializer::DeserializeImageData");

        return imageData;
    }
}