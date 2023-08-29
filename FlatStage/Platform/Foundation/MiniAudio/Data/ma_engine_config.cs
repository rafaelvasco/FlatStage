using System;

namespace FlatStage.Foundation.MiniAudio;

public unsafe partial struct ma_engine_config
{
    public ma_resource_manager* pResourceManager;

    public void* pContext;

    public void* pDevice;

    public void* pPlaybackDeviceID;

    public void* dataCallback;

    public void* notificationCallback;

    public void* pLog;

    public uint listenerCount;

    public uint channels;

    public uint sampleRate;

    public uint periodSizeInFrames;

    public uint periodSizeInMilliseconds;

    public uint gainSmoothTimeInFrames;

    public uint gainSmoothTimeInMilliseconds;

    public uint defaultVolumeSmoothTimeInPCMFrames;

    public ma_allocation_callbacks allocationCallbacks;

    public uint noAutoStart;

    public uint noDevice;

    public ma_mono_expansion_mode monoExpansionMode;

    public void* pResourceManagerVFS;

    public IntPtr onProcess;

    public void* pProcessUserData;
}
