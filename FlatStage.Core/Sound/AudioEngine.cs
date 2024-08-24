using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MINIAUDIO;

namespace FlatStage;

internal unsafe class AudioEngine
{
    private static int _audioHandle;

    private const int SOUNDS_ARRAY_SIZE = 128;
    private const int CUSTOM_SOUND_GROUPS_ARRAY_SIZE = 16;

    private readonly void* _audioEngine; // ma_engine*
    private readonly void* _songsGroup; // ma_sound*
    private readonly void* _effectsGroup; // ma_sound*
    private void* _customGroups = null; // ma_sound*

    private void* _soundsArray; // ma_sound*
    private void* _audioBuffersArray; // ma_audio_buffer

    private int _soundsIndex;
    private int _customSoundGroupIndex;
    private int _maxSoundsIndex = SOUNDS_ARRAY_SIZE - 1;

    private readonly FastDictionary<int, int> _audioSoundMap = new();
    private readonly FastDictionary<string, int> _customSoundGroupsMap = new();

    public AudioEngine()
    {
        _audioEngine = MiniAudio.ma_ext_alloc<ma_engine>();
        _songsGroup = MiniAudio.ma_ext_alloc<ma_sound>();
        _effectsGroup = MiniAudio.ma_ext_alloc<ma_sound>();
        _soundsArray = MiniAudio.ma_ext_alloc<ma_sound>(SOUNDS_ARRAY_SIZE);
        _audioBuffersArray = MiniAudio.ma_ext_alloc<ma_audio_buffer>(SOUNDS_ARRAY_SIZE);

        ma_engine_config engineConfig = MiniAudio.ma_engine_config_init();

        if (MiniAudio.ma_engine_init(&engineConfig, _audioEngine) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Audio Engine");
        }

        if (MiniAudio.ma_sound_group_init(_audioEngine, 0, null, _songsGroup) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Audio Engine Songs Group");
        }

        if (MiniAudio.ma_sound_group_init(_audioEngine, 0, null, _effectsGroup) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Audio Engine Effects Group");
        }
    }

    public void CreateSoundGroup(string name)
    {
        if (_customGroups == null)
        {
            _customGroups = MiniAudio.ma_ext_alloc<ma_sound>(CUSTOM_SOUND_GROUPS_ARRAY_SIZE);
        }

        void* soundGroup = Unsafe.Add<ma_sound>(_customGroups, _customSoundGroupIndex);

        if (MiniAudio.ma_sound_group_init(_audioEngine, 0, null, soundGroup) != ma_result.MA_SUCCESS)
        {
            throw new ApplicationException("Failed to initialize Custom Sound Group");
        }

        _customSoundGroupsMap[name] = _customSoundGroupIndex++;
    }

    public void StopAll()
    {
        MiniAudio.ma_engine_stop(_audioEngine);
    }

    public void SetGlobalVolume(float volume)
    {
        volume = MathUtils.Clamp(volume, 0f, 1f);

        MiniAudio.ma_engine_set_volume(_audioEngine, volume);
    }

    public Audio CreateAudio(AudioData audioData, string? customSoundGroup = null)
    {
#if DEBUG

        Console.WriteLine(
            $"Creating Audio: \nID: {audioData.Id}\nData: {audioData.Data.Length}\nType: {audioData.Type}");
#endif

        var targetGroup = audioData.Type switch
        {
            AudioType.Song => _songsGroup,
            AudioType.Effect => _effectsGroup,
            _ => null
        };

        if (customSoundGroup != null)
        {
            if (!_customSoundGroupsMap.ContainsKey(customSoundGroup))
            {
                CreateSoundGroup(customSoundGroup);
            }

            var index = _customSoundGroupsMap[customSoundGroup];

            targetGroup = Unsafe.Add<ma_sound>(_customGroups, index);
        }

        CheckBuffersOverflow();

        void* sound = Unsafe.Add<ma_sound>(_soundsArray, _soundsIndex);
        void* audioBuffer = Unsafe.Add<ma_audio_buffer>(_audioBuffersArray, _soundsIndex);

        PopulateAudioBuffer(audioData, audioBuffer);

        _audioSoundMap[_audioHandle] = _soundsIndex;

        var audio = BuildAudio(audioData, audioBuffer, sound, targetGroup);

        _soundsIndex++;
        _audioHandle++;

        return audio;
    }

    private void CheckBuffersOverflow()
    {
        if (_soundsIndex > _maxSoundsIndex)
        {
            _maxSoundsIndex *= 2;

            _soundsArray = MiniAudio.ma_ext_realloc<ma_sound>(_soundsArray, _maxSoundsIndex);
            _audioBuffersArray = MiniAudio.ma_ext_realloc<ma_audio_buffer>(_audioBuffersArray, _maxSoundsIndex);
        }
    }

    private Audio BuildAudio(AudioData data, void* targetAudioBuffer, void* targetSound, void* targetSoundGroup)
    {
        uint flags = (uint)ma_sound_flags.MA_SOUND_FLAG_DECODE;

        MiniAudio.ma_sound_init_from_data_source(_audioEngine, targetAudioBuffer, flags,
            targetSoundGroup, targetSound);

        var audio = new Audio(data.Id, _audioHandle, data.Type);

        return audio;
    }

    private void PopulateAudioBuffer(AudioData data, void* audioBuffer)
    {
        var audioData = data.Data;

        if (data.Format == AudioFormat.Ogg)
        {
            audioData = OggDecoder.Decode(audioData);
        }

        ma_decoder_config decoderConfig = MiniAudio.ma_decoder_config_init_default();

        decoderConfig.format = ma_format.ma_format_f32;
        decoderConfig.channels = 2;

        void* decoder = NativeMemory.AlignedAlloc((nuint)Marshal.SizeOf<ma_decoder>(), 32);

        MiniAudio.ma_decoder_init_memory(Unsafe.AsPointer(ref audioData[0]), (ulong)audioData.Length,
                &decoderConfig,
        decoder);

        ulong audioPcmFramesLength;

        MiniAudio.ma_data_source_get_length_in_pcm_frames(decoder, &audioPcmFramesLength);

        int totalBufferLength = (int)audioPcmFramesLength * 2;

        var audioLoadBuffer = NativeMemory.Alloc((nuint)(totalBufferLength * sizeof(float)));

        MiniAudio.ma_data_source_read_pcm_frames(decoder, audioLoadBuffer, audioPcmFramesLength, null);

        MiniAudio.ma_decoder_uninit(decoder);

        NativeMemory.AlignedFree(decoder);

        ma_audio_buffer_config config = MiniAudio.ma_audio_buffer_config_init(
           ma_format.ma_format_f32,
           2,
           audioPcmFramesLength,
           audioLoadBuffer,
           null
        );

        MiniAudio.ma_audio_buffer_init_copy(&config, audioBuffer);

        NativeMemory.Free(audioLoadBuffer);
    }

    public void Play(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            InternalPlay(_soundsArray, index);
        }
    }

    public void Play(string soundGroup)
    {
        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            InternalPlay(_customGroups, index);
            return;
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            InternalPlay(_effectsGroup, 0);
            return;
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            InternalPlay(_songsGroup, 0);
        }
    }

    private void InternalPlay(void* soundPtr, int index)
    {
        var sound = Unsafe.Add<ma_sound>(soundPtr, index);

        MiniAudio.ma_sound_start(sound);
        MiniAudio.ma_sound_seek_to_pcm_frame(sound, 0);
    }

    public void PlayWithPanPitch(Audio audio, float pan, float pitch)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            InternalPlayEx(_soundsArray, index, pan, pitch);
        }
    }

    public void PlayWithPanPitch(string soundGroup, float pan, float pitch)
    {
        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            InternalPlayEx(_customGroups, index, pan, pitch);
            return;
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            InternalPlayEx(_effectsGroup, 0, pan, pitch);
            return;
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            InternalPlayEx(_songsGroup, 0, pan, pitch);
        }
    }

    private void InternalPlayEx(void* soundPtr, int index, float pan, float pitch)
    {
        var sound = Unsafe.Add<ma_sound>(soundPtr, index);
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

    public void Stop(Audio audio, bool reset)
    {

        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            InternalStop(_soundsArray, index, reset);
        }
    }

    public void Stop(string soundGroup, bool reset)
    {
        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            InternalStop(_customGroups, index, reset);
            return;
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            InternalStop(_effectsGroup, 0, reset);
            return;
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            InternalStop(_songsGroup, 0, reset);
        }
    }

    private void InternalStop(void* sountPtr, int index, bool reset)
    {
        var sound = Unsafe.Add<ma_sound>(sountPtr, index);
        MiniAudio.ma_sound_stop(sound);

        if (reset)
        {
            MiniAudio.ma_sound_seek_to_pcm_frame(sound, 0);
        }
    }

    public bool GetPlaying(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            return InternalGetPlaying(_soundsArray, index);
        }

        return false;
    }

    public bool GetPlaying(string soundGroup)
    {
        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            return InternalGetPlaying(_customGroups, index);
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            return InternalGetPlaying(_effectsGroup, 0);
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            return InternalGetPlaying(_songsGroup, 0);
        }

        return false;
    }

    private bool InternalGetPlaying(void* soundPtr, int index)
    {
        var sound = Unsafe.Add<ma_sound>(soundPtr, index);
        return MiniAudio.ma_sound_is_playing(sound) > 0;
    }

    public float GetVolume(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            return InternalGetVolume(_soundsArray, index);
        }

        return 0f;
    }

    public float GetVolume(string soundGroup)
    {
        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            return InternalGetVolume(_customGroups, index);
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            return InternalGetVolume(_effectsGroup, 0);
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            return InternalGetVolume(_songsGroup, 0);
        }

        return 0.0f;
    }

    private float InternalGetVolume(void* soundPtr, int index)
    {
        var sound = Unsafe.Add<ma_sound>(soundPtr, index);
        return MiniAudio.ma_sound_get_volume(sound);
    }

    public void SetVolume(Audio audio, float value)
    {
        value = MathUtils.Clamp(value, 0f, 1f);

        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            InternalSetVolume(_soundsArray, index, value);
        }
    }

    public void SetVolume(string soundGroup, float value)
    {
        value = MathUtils.Clamp(value, 0f, 1f);

        if (_customSoundGroupsMap.TryGetValue(soundGroup, out var index))
        {
            InternalSetVolume(_customGroups, index, value);
            return;
        }

        if (soundGroup == AudioContext.DefaultEffectsGroup)
        {
            InternalSetVolume(_effectsGroup, 0, value);
            return;
        }

        if (soundGroup == AudioContext.DefaultSongsGroup)
        {
            InternalSetVolume(_songsGroup, 0, value);
        }
    }

    private void InternalSetVolume(void* soundPtr, int index, float value)
    {
        var sound = Unsafe.Add<ma_sound>(soundPtr, index);
        MiniAudio.ma_sound_set_volume(sound, value);
    }

    public void DestroyAudio(Audio audio)
    {
        if (_audioSoundMap.TryGetValue(audio.Handle, out var index))
        {
            var sound = Unsafe.Add<ma_sound>(_soundsArray, index);

            MiniAudio.ma_sound_uninit(sound);
            _audioSoundMap.Remove(audio.Handle);
        }
    }

    public void Shutdown()
    {

        MiniAudio.ma_engine_stop(_audioEngine);
        MiniAudio.ma_sound_group_uninit(_songsGroup);
        MiniAudio.ma_sound_group_uninit(_effectsGroup);

        for (int i = 0; i < _soundsIndex; ++i)
        {
            var audioBuffer = Unsafe.Add<ma_audio_buffer>(_audioBuffersArray, i);

            MiniAudio.ma_audio_buffer_uninit(audioBuffer);
        }

        if (_customGroups != null)
        {
            foreach (var (_, index) in _customSoundGroupsMap)
            {
                var soundGroup = Unsafe.Add<ma_sound>(_customGroups, index);

                MiniAudio.ma_sound_group_uninit(soundGroup);
            }
        }

        MiniAudio.ma_engine_uninit(_audioEngine);

        MiniAudio.ma_ext_free(_audioEngine);
        MiniAudio.ma_ext_free(_songsGroup);
        MiniAudio.ma_ext_free(_effectsGroup);
        MiniAudio.ma_ext_free(_soundsArray);
        MiniAudio.ma_ext_free(_audioBuffersArray);

        if (_customGroups != null)
        {
            MiniAudio.ma_ext_free(_customGroups);
        }
    }
}
