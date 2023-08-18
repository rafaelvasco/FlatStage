using System;

using FlatStage.ContentPipeline;

namespace FlatStage.Sound;

public enum AudioType
{
    Song,
    Effect
}

public enum AudioParameter
{
    Pitch
}

public class Audio : Asset
{
    internal int Handle { get; private set; }

    public bool IsPlaying => AudioContext.GetPlaying(this);

    public AudioType Type { get; private set; }

    public float Volume
    {
        get => AudioContext.GetVolume(this);
        set => AudioContext.SetVolume(this, value);
    }

    public static AudioType ParseAudioTypeFromString(string value)
    {
        if (Enum.TryParse<AudioType>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        throw new InvalidCastException($"$Could not parse AudioType of type: {value}");
    }

    internal Audio(string id, int handle, AudioType type) : base(id)
    {
        Handle = handle;
        Type = type;
    }

    public void Play()
    {
        if (IsPlaying)
        {
            return;
        }

        AudioContext.Play(this);
    }

    public void PlayEx(float pan, float pitch)
    {
        if (IsPlaying)
        {
            return;
        }

        AudioContext.PlayEx(this, pan, pitch);
    }

    public void Pause()
    {
        if (!IsPlaying)
        {
            return;
        }

        AudioContext.Stop(this, false);
    }

    public void Stop()
    {
        if (!IsPlaying)
        {
            return;
        }

        AudioContext.Stop(this, true);
    }

    protected override void Free()
    {
        AudioContext.DestroyAudio(this);
    }
}