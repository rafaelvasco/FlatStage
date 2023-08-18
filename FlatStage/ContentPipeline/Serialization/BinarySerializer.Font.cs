using System.Collections.Generic;
using System.IO;

namespace FlatStage.ContentPipeline;
public static partial class BinarySerializer
{
    private static void SerializeFontData(Stream stream, FontData fontData)
    {
        IDefinitionData.ThrowIfInValid(fontData, "BinarySerializer::SerializeFontData");

#if DEBUG
        IDefinitionData.Debug(fontData, "Writing Font Data:");
#endif

        using var writer = new BinaryWriter(stream);

        writer.Write(ContentProperties.SerializationMagicString);
        writer.Write((byte)SerializationDataType.Font);
        writer.Write(fontData.Id);
        writer.Write(fontData.ImageData.Data.Length);
        writer.Write(fontData.ImageData.Data);
        writer.Write(fontData.Width);
        writer.Write(fontData.Height);

        writer.Write(fontData.Glyphs.Count);
        foreach (var (glyphKey, glyphValue) in fontData.Glyphs)
        {
            writer.Write(glyphKey);
            writer.Write(glyphValue.X);
            writer.Write(glyphValue.Y);
            writer.Write(glyphValue.Width);
            writer.Write(glyphValue.Height);
            writer.Write(glyphValue.XOffset);
            writer.Write(glyphValue.YOffset);
            writer.Write(glyphValue.XAdvance);
        }
    }

    private static FontData DeserializeFontData(Stream stream)
    {
        using var reader = new BinaryReader(stream);

        var magic = reader.ReadString();
        var type = reader.ReadByte();

        if (magic != ContentProperties.SerializationMagicString || type != (byte)SerializationDataType.Font)
        {
            throw new InvalidDataException("Invalid FontData Binary File");
        }

        var id = reader.ReadString();
        var imageDataLength = reader.ReadInt32();
        var imageData = reader.ReadBytes(imageDataLength);
        var width = reader.ReadInt32();
        var height = reader.ReadInt32();

        var glyphs = new Dictionary<int, GlyphInfo>();

        var countGlyphs = reader.ReadInt32();

        for (int i = 0; i < countGlyphs; ++i)
        {
            var glyphKey = reader.ReadInt32();
            var glyphX = reader.ReadInt32();
            var glyphY = reader.ReadInt32();
            var glyphWidth = reader.ReadInt32();
            var glyphHeight = reader.ReadInt32();
            var glyphXOffset = reader.ReadInt32();
            var glyphYOffset = reader.ReadInt32();
            var glyphXAdvance = reader.ReadInt32();

            glyphs[glyphKey] = new GlyphInfo()
            {
                X = glyphX,
                Y = glyphY,
                Width = glyphWidth,
                Height = glyphHeight,
                XOffset = glyphXOffset,
                YOffset = glyphYOffset,
                XAdvance = glyphXAdvance,
            };
        }

        var fontData = new FontData()
        {
            Id = id,
            Glyphs = glyphs,
            Width = width,
            Height = height,
            ImageData = new ImageData { Id = id + "_ImageData", Data = imageData, Width = width, Height = height }
        };

#if DEBUG
        IDefinitionData.Debug(fontData, "Loaded Font Data:");
#endif

        IDefinitionData.ThrowIfInValid(fontData, "BinarySerializer::DeserializeImageData");

        return fontData;
    }
}
