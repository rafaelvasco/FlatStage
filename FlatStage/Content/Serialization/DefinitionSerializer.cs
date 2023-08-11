using System;

namespace FlatStage;

public static partial class DefinitionSerializer
{
    public static T DeSerializeDefinitionFile<T>(string filePath) where T : class, IDefinitionData
    {
        if (typeof(T) == typeof(StageSettings))
        {
            var stageSettings = DeSerializeStageSettings(filePath);
            return (stageSettings as T)!;
        }

        if (typeof(T) == typeof(AssetsManifest))
        {
            var assetsManifest = DeSerializeAssetsManifest(filePath);
            return (assetsManifest as T)!;
        }

        throw new ArgumentException("Invalid Type For Definition File", nameof(T));
    }

    public static void SerializeDefinitionFile<T>(string filePath, T definition) where T : class, IDefinitionData
    {
        if (typeof(T) == typeof(StageSettings))
        {
            SerializeStageSettings((definition as StageSettings)!, filePath);
        }
        else if (typeof(T) == typeof(AssetsManifest))
        {
            SerializeAssetsManifest((definition as AssetsManifest)!, filePath);
        }
        else
        {
            throw new ArgumentException("Invalid Type For Definition File", nameof(T));    
        }
    }
}