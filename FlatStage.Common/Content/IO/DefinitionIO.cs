using System.Text.Json;

namespace FlatStage;

public static class DefinitionIO
{
    public static T LoadDefinitionData<T>(string filePath) where T : IDefinitionData
    {
#if DEBUG
        Console.WriteLine($"Loading Definition File At Path: {filePath}");
#endif
        try
        {
            var jsonText = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<T>(jsonText, ContentProperties.JsonSettings);
            return data ?? throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            FlatException.Throw($"Failed to load definition file of type {typeof(T)}: {e.Message}", e);
        }

        throw new InvalidOperationException();
    }
}
