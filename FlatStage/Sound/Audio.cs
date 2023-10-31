using FlatStage.Content;
using System;

namespace FlatStage.Sound;

public enum AudioParameter
{
    Pitch
}

public enum AudioType
{
    Effect,
    Song
}

public enum AudioFormat
{
    Wav,
    Ogg
}

public class Audio : Asset
{
    internal int Handle { get; private set; }

    public bool IsPlaying => AudioContext.GetPlaying(this);

    public AudioType Type { get; }

    public float Volume
    {
        get => AudioContext.GetVolume(this);
        set => AudioContext.SetVolume(this, value);
    }

    internal Audio(string id, int handle, AudioType type) : base(id)
    {
        Handle = handle;
        Type = type;
    }

    public void Play()
    {
        AudioContext.Play(this);
    }

    public void PlayWithPanPitch(float pan, float pitch)
    {
        AudioContext.PlayWithPanPitch(this, pan, pitch);
    }

    public void Pause()
    {
        AudioContext.Stop(this, reset: false);
    }

    public void Stop()
    {
        AudioContext.Stop(this, reset: true);
    }

    protected override void Free()
    {
        AudioContext.DestroyAudio(this);
    }

    internal static AudioType ParseAudioTypeFromString(string audioType)
    {
        if (Enum.TryParse<AudioType>(audioType, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new InvalidCastException($"$Could not parse AudioType of type: {audioType}");
    }

    internal static AudioFormat ParseAudioFormatFromString(string audioFormat)
    {
        if (Enum.TryParse<AudioFormat>(audioFormat, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new InvalidCastException($"$Could not parse AudioFormat of type: {audioFormat}");
    }
}