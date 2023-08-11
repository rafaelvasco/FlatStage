using System;

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

    public bool IsPlaying => AudioManager.GetPlaying(this);

    public AudioType Type { get; private set; }

    public float Volume
    {
        get => AudioManager.GetVolume(this);
        set => AudioManager.SetVolume(this, value);
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

        AudioManager.Play(this);
    }

    public void PlayEx(float pan, float pitch)
    {
        if (IsPlaying)
        {
            return;
        }

        AudioManager.PlayEx(this, pan, pitch);
    }

    public void Pause()
    {
        if (!IsPlaying)
        {
            return;
        }

        AudioManager.Stop(this, false);
    }

    public void Stop()
    {
        if (!IsPlaying)
        {
            return;
        }

        AudioManager.Stop(this, true);
    }

    protected override void Free()
    {
        AudioManager.DestroyAudio(this);
    }
}