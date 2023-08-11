using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlatStage.Foundation.MiniAudio;

namespace FlatStage.Sound;

public static unsafe class AudioManager
{
    private static ma_device _audioDevice;
    private static ma_engine _audioEngine;
    private static ma_sound _songsGroup;
    private static ma_sound _effectsGroup;

    private static readonly ma_sound[] _sounds = new ma_sound[256];
    private static readonly ma_decoder[] _soundDecoders = new ma_decoder[256];

    private static int _soundsIndex;
    private static int _audioHandle;

    private static readonly Dictionary<int, int> _audioSoundMap = new();

    private delegate void dataCallback(ma_device* pDevice, void* pOutput, void* pInput, uint frameCount);

    internal static void Init()
    {
        ma_device_config deviceConfig = Miniaudio.ma_device_config_init(ma_device_type.ma_device_type_playback);

        deviceConfig.playback.format = ma_format.ma_format_f32;
        deviceConfig.dataCallback = Marshal.GetFunctionPointerForDelegate(new dataCallback(DataCallback));

        fixed (ma_device* device = &_audioDevice)
        fixed (ma_engine* engine = &_audioEngine)
        fixed (ma_sound* songsGroup = &_songsGroup)
        fixed (ma_sound* effectsGroup = &_effectsGroup)
        {
            if (Miniaudio.ma_device_init(null, &deviceConfig, device) != ma_result.MA_SUCCESS)
            {
                throw new ApplicationException("Failed to initialize Audio Device");
            }

            ma_engine_config engineConfig = Miniaudio.ma_engine_config_init();

            engineConfig.pDevice = device;
            engineConfig.noAutoStart = 1;

            if (Miniaudio.ma_engine_init(&engineConfig, engine) != ma_result.MA_SUCCESS)
            {
                throw new ApplicationException("Failed to initialize Audio Engine");
            }

            Miniaudio.ma_sound_group_init(engine, 0, null, songsGroup);
            Miniaudio.ma_sound_group_init(engine, 0, null, effectsGroup);

            Miniaudio.ma_engine_start(engine);
        }
    }

    public static void StopAll()
    {
        fixed (ma_engine* engine = &_audioEngine)
        {
            Miniaudio.ma_engine_stop(engine);
        }
    }

    public static void SetGlobalVolume(float volume)
    {
        volume = Calc.Clamp(volume, 0f, 1f);

        fixed (ma_engine* engine = &_audioEngine)
        {
            Miniaudio.ma_engine_set_volume(engine, volume);
        }
    }

    internal static Audio CreateAudio(AudioData audioData)
    {
#if DEBUG

        Console.WriteLine(
            $"Creating Audio Source: \nID: {audioData.Id}\nData: {audioData.Data!.Length}\nType: {audioData.Type}");
#endif

        ma_decoder_config decoderConfig = Miniaudio.ma_decoder_config_init_default();

        Audio audio;

        fixed (ma_engine* engine = &_audioEngine)
        fixed (ma_sound* songsGroup = &_songsGroup)
        fixed (ma_sound* effectsGroup = &_effectsGroup)
        fixed (ma_sound* targetSound = &_sounds[_soundsIndex])
        fixed (ma_decoder* soundDecoder = &_soundDecoders[_soundsIndex])
        {
            Miniaudio.ma_decoder_init_memory(Unsafe.AsPointer(ref audioData.Data![0]), (ulong)audioData.Data.Length,
                &decoderConfig,
                soundDecoder);

            uint flags = 0;

            switch (audioData.Type)
            {
                case AudioType.Song:
                    flags |= (uint)ma_sound_flags.MA_SOUND_FLAG_STREAM;
                    break;
                case AudioType.Effect:
                    flags |= (uint)ma_sound_flags.MA_SOUND_FLAG_DECODE;
                    break;
            }

            audio = new Audio(audioData.Id!, _audioHandle, audioData.Type);

            _audioSoundMap[_audioHandle] = _soundsIndex;

            ma_sound* targetGroup = audioData.Type switch
            {
                AudioType.Song => songsGroup,
                AudioType.Effect => effectsGroup,
                _ => null
            };

            Miniaudio.ma_sound_init_from_data_source(engine, soundDecoder, (uint)flags,
                targetGroup, targetSound);

            _soundsIndex++;
            _audioHandle++;
        }

        return audio;
    }


    internal static void Play(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                Miniaudio.ma_sound_start(sound);
            }
        }
    }

    internal static void PlayEx(Audio audio, float pan, float pitch)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                if (pan != 0)
                {
                    Miniaudio.ma_sound_set_pan(sound, pan);
                }

                if (pitch > 0)
                {
                    Miniaudio.ma_sound_set_pitch(sound, pitch);
                }

                Miniaudio.ma_sound_start(sound);
            }
        }
    }

    internal static void Stop(Audio audio, bool reset)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                Miniaudio.ma_sound_stop(sound);

                if (reset)
                {
                    Miniaudio.ma_sound_seek_to_pcm_frame(sound, 0);
                }
            }
        }
    }

    internal static bool GetPlaying(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                return Miniaudio.ma_sound_is_playing(sound) > 0;
            }
        }

        return false;
    }

    internal static float GetVolume(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                return Miniaudio.ma_sound_get_volume(sound);
            }
        }

        return 0;
    }

    internal static void SetVolume(Audio audio, float value)
    {
        value = Calc.Clamp(value, 0f, 1f);

        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                Miniaudio.ma_sound_set_volume(sound, value);
            }
        }
    }

    internal static void DestroyAudio(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            fixed (ma_sound* sound = &_sounds[index])
            {
                Miniaudio.ma_sound_uninit(sound);
                *sound = default;
                _audioSoundMap.Remove(audio.Handle);
            }
        }
    }

    private static void DataCallback(ma_device* pDevice, void* pOutput, void* pInput, uint frameCount)
    {
        fixed (ma_engine* engine = &_audioEngine)
        {
            Miniaudio.ma_engine_read_pcm_frames(engine, pOutput, frameCount, null);
        }
    }


    internal static void Shutdown()
    {
        fixed (ma_engine* engine = &_audioEngine)
        {
            Miniaudio.ma_engine_stop(engine);
        }

        fixed (ma_sound* songsGroup = &_songsGroup)
        fixed (ma_sound* effectsGroup = &_effectsGroup)
        {
            Miniaudio.ma_sound_group_uninit(songsGroup);
            Miniaudio.ma_sound_group_uninit(effectsGroup);
        }

        for (int i = 0; i < _soundsIndex; ++i)
        {
            fixed (ma_sound* sound = &_sounds[i])
            fixed (ma_decoder* decoder = &_soundDecoders[i])
            {
                Miniaudio.ma_sound_uninit(sound);
                Miniaudio.ma_decoder_uninit(decoder);
            }
        }

        fixed (ma_engine* engine = &_audioEngine)
        fixed (ma_device* device = &_audioDevice)
        {
            Miniaudio.ma_engine_uninit(engine);
            Miniaudio.ma_device_uninit(device);
        }
    }
}