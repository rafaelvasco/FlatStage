using System.IO;
using Tommy;

namespace FlatStage.ContentPipeline;

public static partial class DefinitionSerializer
{

    private static StageSettings DeSerializeStageSettings(string filePath)
    {
        using var reader = File.OpenText(filePath);

        TomlTable table = TOML.Parse(reader);

        var title = table["AppTitle"].AsString;
        var windowWidth = table["WindowWidth"].AsInteger;
        var windowHeight = table["WindowHeight"].AsInteger;

        var stageSettings = new StageSettings()
        {
            AppTitle = title,
            WindowWidth = windowWidth,
            WindowHeight = windowHeight
        };

        IDefinitionData.ThrowIfInValid(stageSettings, "DefinitionSerializer::DeSerializeStageSettings");

        return stageSettings;

    }

    private static void SerializeStageSettings(StageSettings settings, string filePath)
    {
        IDefinitionData.ThrowIfInValid(settings, "DefinitionSerializer::SerializeStageSettings");

        TomlTable toml = new()
        {
            ["AppTitle"] = settings.AppTitle!,
            ["WindowWidth"] = settings.WindowWidth,
            ["WindowHeight"] = settings.WindowHeight,
        };

        using var file = File.CreateText(filePath);

        toml.WriteTo(file);

        file.Flush();
    }
}