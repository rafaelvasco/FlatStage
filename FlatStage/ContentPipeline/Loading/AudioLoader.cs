using System;
using System.IO;

using FlatStage.Sound;

namespace FlatStage.ContentPipeline;

internal class AudioLoader : AssetLoader<Audio>
{
    public override Audio Load(string id, AssetsManifest manifest)
    {
        if (manifest.Audios?.TryGetValue(id, out var audioAssetInfo) != true)
            throw new ApplicationException($"Could not find asset with Id: {id}");

        IDefinitionData.ThrowIfInValid(audioAssetInfo, "AudioLoader::Load");

        var assetFullBinPath =
            Path.Combine(ContentProperties.AssetsFolder, audioAssetInfo!.Id + ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            return LoadFromStream(id, stream);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    protected override Audio LoadFromStream(string id, Stream stream)
    {
        var audioData = LoadAssetData<AudioData>(id, stream);

        var audio = AudioContext.CreateAudio(audioData);

        return audio;
    }
}