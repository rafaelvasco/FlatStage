using System;
using System.IO;

using FlatStage.Sound;

namespace FlatStage.Content;

internal class AudioLoader : AssetLoader<Audio, AudioData>
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

            var audioData = Assets.LoadAssetData<AudioData>(id, stream);

            return LoadFromAssetData(audioData);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Failed to open asset bin file: {e.Message}");
        }
    }

    public override Audio LoadFromAssetData(AudioData assetData)
    {
        var audio = AudioContext.CreateAudio(assetData);

        return audio;
    }
}
