using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using FlatStage.ContentPipeline;
using FlatStage.Foundation.MiniAudio;

namespace FlatStage.Sound;

public static unsafe class AudioContext
{
    private const int ARRAY_SIZE = 256;

    private static ma_device* _audioDevice;
    private static ma_engine* _audioEngine;
    private static ma_sound* _songsGroup;
    private static ma_sound* _effectsGroup;

    private static ma_sound* _soundsArray;
    private static ma_decoder* _decodersArray;

    private static int _soundsIndex;
    private static int _audioHandle;

    private static readonly Dictionary<int, int> _audioSoundMap = new();

    private delegate void dataCallback(ma_device* pDevice, void* pOutput, void* pInput, uint frameCount);

    private static dataCallback _dataCallBackDelegate = null!;

    internal static void Init()
    {
        _dataCallBackDelegate = DataCallback;

        _audioDevice = (ma_device*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_device)));
        _audioEngine = (ma_engine*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_engine)));
        _songsGroup = (ma_sound*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_sound)));
        _effectsGroup = (ma_sound*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_sound)));

        _soundsArray = (ma_sound*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_sound)) * ARRAY_SIZE);
        _decodersArray = (ma_decoder*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ma_decoder)) * ARRAY_SIZE);

        ma_device_config deviceConfig = MiniAudio.ma_device_config_init(ma_device_type.ma_device_type_playback);

        deviceConfig.playback.format = ma_format.ma_format_f32;
        deviceConfig.dataCallback = Marshal.GetFunctionPointerForDelegate(_dataCallBackDelegate);

        if (MiniAudio.ma_device_init(null, &deviceConfig, _audioDevice) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Audio Device");
        }

        ma_engine_config engineConfig = MiniAudio.ma_engine_config_init();

        engineConfig.pDevice = _audioDevice;

        if (MiniAudio.ma_engine_init(&engineConfig, _audioEngine) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Audio Engine");
        }

        MiniAudio.ma_sound_group_init(_audioEngine, 0, null, _songsGroup);
        MiniAudio.ma_sound_group_init(_audioEngine, 0, null, _effectsGroup);
    }

    public static void StopAll()
    {
        MiniAudio.ma_engine_stop(_audioEngine);
    }

    public static void SetGlobalVolume(float volume)
    {
        volume = Calc.Clamp(volume, 0f, 1f);

        MiniAudio.ma_engine_set_volume(_audioEngine, volume);
    }

    internal static Audio CreateAudio(AudioData audioData)
    {
#if DEBUG

        Console.WriteLine(
            $"Creating Audio Source: \nID: {audioData.Id}\nData: {audioData.Data!.Length}\nType: {audioData.Type}");
#endif

        ma_decoder_config decoderConfig = MiniAudio.ma_decoder_config_init_default();

        Audio audio;

        var decoder = _decodersArray + _soundsIndex;
        var sound = _soundsArray + _soundsIndex;

        MiniAudio.ma_decoder_init_memory(Unsafe.AsPointer(ref audioData.Data![0]), (ulong)audioData.Data.Length,
                &decoderConfig,
                decoder);

        uint flags = (uint)ma_sound_flags.MA_SOUND_FLAG_DECODE;

        audio = new Audio(audioData.Id!, _audioHandle, audioData.Type);

        _audioSoundMap[_audioHandle] = _soundsIndex;

        ma_sound* targetGroup = audioData.Type switch
        {
            AudioType.Song => _songsGroup,
            AudioType.Effect => _effectsGroup,
            _ => null
        };

        MiniAudio.ma_sound_init_from_data_source(_audioEngine, decoder, flags,
            targetGroup, sound);

        _soundsIndex++;
        _audioHandle++;

        return audio;
    }

    internal static void Play(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            MiniAudio.ma_sound_start(sound);
            MiniAudio.ma_sound_seek_to_pcm_frame(sound, 0);
        }
    }

    internal static void PlayEx(Audio audio, float pan, float pitch)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            if (pan != 0)
            {
                MiniAudio.ma_sound_set_pan(sound, pan);
            }

            if (pitch > 0)
            {
                MiniAudio.ma_sound_set_pitch(sound, pitch);
            }

            MiniAudio.ma_sound_start(sound);
            MiniAudio.ma_sound_seek_to_pcm_frame(sound, 0);
        }
    }

    internal static void Stop(Audio audio, bool reset)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            MiniAudio.ma_sound_stop(sound);

            if (reset)
            {
                MiniAudio.ma_sound_seek_to_pcm_frame(sound, 0);
            }
        }
    }

    internal static bool GetPlaying(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            return MiniAudio.ma_sound_is_playing(sound) > 0;
        }

        return false;
    }

    internal static float GetVolume(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            return MiniAudio.ma_sound_get_volume(sound);
        }

        return 0;
    }

    internal static void SetVolume(Audio audio, float value)
    {
        value = Calc.Clamp(value, 0f, 1f);

        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            MiniAudio.ma_sound_set_volume(sound, value);
        }
    }

    internal static void DestroyAudio(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = _soundsArray + index;

            MiniAudio.ma_sound_uninit(sound);
            *sound = default;
            _audioSoundMap.Remove(audio.Handle);
        }
    }

    private static void DataCallback(ma_device* pDevice, void* pOutput, void* pInput, uint frameCount)
    {
        MiniAudio.ma_engine_read_pcm_frames(_audioEngine, pOutput, frameCount, null);
    }

    internal static void Shutdown()
    {
        MiniAudio.ma_engine_stop(_audioEngine);
        MiniAudio.ma_sound_group_uninit(_songsGroup);
        MiniAudio.ma_sound_group_uninit(_effectsGroup);

        for (int i = 0; i < _soundsIndex; ++i)
        {
            MiniAudio.ma_sound_uninit(_soundsArray + i);
            MiniAudio.ma_decoder_uninit(_decodersArray + i);
        }

        MiniAudio.ma_engine_uninit(_audioEngine);

        Marshal.FreeHGlobal((nint)_audioEngine);
        Marshal.FreeHGlobal((nint)_songsGroup);
        Marshal.FreeHGlobal((nint)_effectsGroup);
        Marshal.FreeHGlobal((nint)_soundsArray);
        Marshal.FreeHGlobal((nint)_decodersArray);
    }
}