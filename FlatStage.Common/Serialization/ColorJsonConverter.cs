using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlatStage;
public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Span<char> colorCode = stackalloc char[reader.ValueSpan.Length];
        for (int i = 0; i < reader.ValueSpan.Length; ++i)
        {
            colorCode[i] = (char)reader.ValueSpan[i];
        }

        return Color.FromHex(colorCode);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        Span<char> hexStr = stackalloc char[8];
        var len = value.ToHex(hexStr);
        FlatException.Assert(len == 8);
        writer.WriteStringValue(hexStr);
    }
}
