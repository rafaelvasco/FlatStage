using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlatStage;
public class Vec2JsonConverter : JsonConverter<Vec2>
{
    public override Vec2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var vecStr = reader.GetString();

        if (vecStr == null)
        {
            return Vec2.Zero;
        }

        if (vecStr[0] != '@')
        {
            var components = vecStr.Trim().Split(',');

            if (components.Length == 2)
            {
                try
                {
                    var value = new Vec2(float.Parse(components[0].Trim(), CultureInfo.InvariantCulture), float.Parse(components[1].Trim(), CultureInfo.InvariantCulture));
                    return value;
                }
                catch (Exception e)
                {
                    FlatException.Throw("Could not parse Vec2 value", e);
                }
            }
            else
            {
                try
                {
                    var value = new Vec2(float.Parse(components[0].Trim(), CultureInfo.InvariantCulture));
                    return value;
                }
                catch (Exception e)
                {
                    FlatException.Throw("Could not parse Vec2 value", e);
                }
            }
        }
        else
        {
            switch (vecStr)
            {
                case "@Half": return Vec2.Half;
                case "@Zero": return Vec2.Zero;
                case "@One": return Vec2.One;
            }
        }

        FlatException.Throw("Could not parse Vec2 value");
        return Vec2.Zero;
    }

    public override void Write(Utf8JsonWriter writer, Vec2 value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
