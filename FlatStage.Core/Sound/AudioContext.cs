namespace FlatStage;

public static class AudioContext
{
    public const string DefaultEffectsGroup = "Effects";
    public const string DefaultSongsGroup = "Songs";

    private static AudioEngine _engine = null!;

    internal static void Init()
    {
        _engine = new AudioEngine();
    }

    public static void StopAll()
    {
        _engine.StopAll();
    }

    public static void SetGlobalVolume(float volume)
    {
        _engine.SetGlobalVolume(volume);
    }

    internal static Audio CreateAudio(AudioData audioData, string? customAudioGroup = null)
    {
        return _engine.CreateAudio(audioData, customAudioGroup);
    }

    public static void Play(Audio audio)
    {
        _engine.Play(audio);
    }

    public static void Play(string soundGroup)
    {
        _engine.Play(soundGroup);
    }

    public static void PlayWithPanPitch(Audio audio, float pan, float pitch)
    {
        _engine.PlayWithPanPitch(audio, pan, pitch);
    }

    public static void PlayWithPanPitch(string soundGroup, float pan, float pitch)
    {
        _engine.PlayWithPanPitch(soundGroup, pan, pitch);
    }

    public static void Stop(Audio audio, bool reset)
    {
        _engine.Stop(audio, reset);
    }

    public static void Stop(string soundGroup, bool reset)
    {
        _engine.Stop(soundGroup, reset);
    }

    public static bool GetPlaying(Audio audio)
    {
        return _engine.GetPlaying(audio);
    }

    public static bool GetPlaying(string soundGroup)
    {
        return _engine.GetPlaying(soundGroup);
    }

    public static float GetVolume(Audio audio)
    {
        return _engine.GetVolume(audio);
    }

    public static float GetVolume(string soundGroup)
    {
        return _engine.GetVolume(soundGroup);
    }

    public static void SetVolume(Audio audio, float value)
    {
        _engine.SetVolume(audio, value);
    }

    public static void SetVolume(string soundGroup, float value)
    {
        _engine.SetVolume(soundGroup, value);
    }

    internal static void DestroyAudio(Audio audio)
    {
        _engine.DestroyAudio(audio);
    }

    internal static void Shutdown()
    {
        _engine.Shutdown();
    }
}
