namespace FlatStage;

internal class AudioLoader : AssetLoader<Audio, AudioData>
{
    public override Audio Load(string id)
    {
        var assetFullBinPath =
            Path.Combine(Content.RootPath, id + ContentProperties.BinaryExt);

        try
        {
            using var stream = File.OpenRead(assetFullBinPath);

            var audioData = Content.LoadAssetData<AudioData>(id, stream);

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
