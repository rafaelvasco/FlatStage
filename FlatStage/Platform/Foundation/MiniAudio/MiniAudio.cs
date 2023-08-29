using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio;

public static unsafe partial class MiniAudio
{
    private const string DllName = "miniaudio";

    private const int MEM_ALIGNMENT_SIZE = 32;

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_decoder_config ma_decoder_config_init(ma_format outputFormat, uint outputChannels, uint outputSampleRate);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_decoder_init_memory(void* data, ulong dataLen, ma_decoder_config* pConfig, void* pDecoder);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_decoder_uninit(void* pDecoder);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ma_decoder_read_pcm_frames(void* decoder, void* framesOut, ulong frameCount);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_decoder_seek_to_pcm_frame(void* decoder, ulong frame);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ma_decoder_get_length_in_pcm_frames(void* decoder);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_device_init(void* context, void* config, void* device);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_device_uninit(void* device);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_device_start(void* device);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_device_stop(void* device);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_device_config ma_device_config_init(ma_device_type deviceType);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_engine_config ma_engine_config_init();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_engine_init(void* engineConfig, void* engine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_sound_group_init(void* engine, uint flags, void* parentGroup, void* group);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_engine_read_pcm_frames(void* engine, void* pFramesOut, ulong frameCount, ulong* pFramesRead);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_engine_stop(void* engine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_engine_set_volume(void* engine, float volume);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_decoder_config ma_decoder_config_init_default();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_sound_init_from_data_source(void* engine, void* dataSource, uint flags, void* soundGroup, void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_sound_start(void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_sound_seek_to_pcm_frame(void* sound, ulong frameIndex);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_sound_set_pan(void* sound, float pan);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_sound_set_pitch(void* sound, float pitch);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_sound_stop(void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern uint ma_sound_is_playing(void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern float ma_sound_get_volume(void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_sound_set_volume(void* sound, float volume);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_sound_uninit(void* sound);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_sound_group_uninit(void* group);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_engine_uninit(void* engine);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_data_source_read_pcm_frames(void* pDataSource, void* pFramesOut, ulong frameCount, ulong* pFramesRead);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_data_source_get_length_in_pcm_frames(void* pDataSource, ulong* pLength);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_audio_buffer_config ma_audio_buffer_config_init(ma_format format, uint channels, ulong sizeInFrames, void* pData, ma_allocation_callbacks* pAllocationCallbacks);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_audio_buffer_alloc_and_init(void* config, void** audioBuffer);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_audio_buffer_uninit_and_free(void* audioBuffer);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern ma_result ma_audio_buffer_init_copy(void* pConfig, void* pAudioBuffer);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ma_audio_buffer_uninit(void* pAudioBuffer);

    public static void* ma_ext_alloc<T>(int count = 1) where T : struct
    {
        void* memory = NativeMemory.AlignedAlloc((nuint)(Marshal.SizeOf<T>() * count), MEM_ALIGNMENT_SIZE);
        return memory;
    }

    public static void* ma_ext_realloc<T>(void* memory, int count) where T : struct
    {
        void* newMemory = NativeMemory.AlignedRealloc(memory, (nuint)(Marshal.SizeOf<T>() * count), MEM_ALIGNMENT_SIZE);
        return newMemory;
    }

    public static void ma_ext_free(void* memoryPtr)
    {
        NativeMemory.AlignedFree(memoryPtr);
    }
}
