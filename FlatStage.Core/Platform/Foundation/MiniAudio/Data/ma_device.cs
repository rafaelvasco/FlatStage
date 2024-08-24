using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_device
    {
        public ma_context* pContext;

        public ma_device_type type;


        public uint sampleRate;

        public ma_atomic_device_state state;


        public IntPtr onData;


        public IntPtr onNotification;


        public IntPtr onStop;

        public void* pUserData;


        public void* startStopLock;


        public void* wakeupEvent;


        public void* startEvent;


        public void* stopEvent;


        public void* thread;

        public ma_result workResult;


        public byte isOwnerOfContext;


        public byte noPreSilencedOutputBuffer;


        public byte noClip;


        public byte noDisableDenormals;


        public byte noFixedSizedCallback;

        public ma_atomic_float masterVolumeFactor;

        public ma_duplex_rb duplexRB;


        public _resampling_e__Struct resampling;


        public _playback_e__Struct playback;


        public _capture_e__Struct capture;


        public _Anonymous_e__Union Anonymous;

        public ref _Anonymous_e__Union._wasapi_e__Struct wasapi
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous_e__Union* pField = &Anonymous)
                {
                    return ref pField->wasapi;
                }
            }
        }

        public ref _Anonymous_e__Union._dsound_e__Struct dsound
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous_e__Union* pField = &Anonymous)
                {
                    return ref pField->dsound;
                }
            }
        }

        public ref _Anonymous_e__Union._winmm_e__Struct winmm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous_e__Union* pField = &Anonymous)
                {
                    return ref pField->winmm;
                }
            }
        }

        public ref _Anonymous_e__Union._jack_e__Struct jack
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous_e__Union* pField = &Anonymous)
                {
                    return ref pField->jack;
                }
            }
        }

        public ref _Anonymous_e__Union._null_device_e__Struct null_device
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous_e__Union* pField = &Anonymous)
                {
                    return ref pField->null_device;
                }
            }
        }

        public unsafe partial struct _resampling_e__Struct
        {
            public ma_resample_algorithm algorithm;

            public ma_resampling_backend_vtable* pBackendVTable;

            public void* pBackendUserData;


            public _linear_e__Struct linear;

            public partial struct _linear_e__Struct
            {

                public uint lpfOrder;
            }
        }

        public unsafe partial struct _playback_e__Struct
        {
            public ma_device_id* pID;

            public ma_device_id id;


            public fixed sbyte name[256];

            public ma_share_mode shareMode;

            public ma_format format;


            public uint channels;


            public fixed byte channelMap[254];

            public ma_format internalFormat;


            public uint internalChannels;


            public uint internalSampleRate;


            public fixed byte internalChannelMap[254];


            public uint internalPeriodSizeInFrames;


            public uint internalPeriods;

            public ma_channel_mix_mode channelMixMode;


            public uint calculateLFEFromSpatialChannels;

            public ma_data_converter converter;

            public void* pIntermediaryBuffer;


            public uint intermediaryBufferCap;


            public uint intermediaryBufferLen;

            public void* pInputCache;


            public ulong inputCacheCap;


            public ulong inputCacheConsumed;


            public ulong inputCacheRemaining;
        }

        public unsafe partial struct _capture_e__Struct
        {
            public ma_device_id* pID;

            public ma_device_id id;


            public fixed sbyte name[256];

            public ma_share_mode shareMode;

            public ma_format format;


            public uint channels;


            public fixed byte channelMap[254];

            public ma_format internalFormat;


            public uint internalChannels;


            public uint internalSampleRate;


            public fixed byte internalChannelMap[254];


            public uint internalPeriodSizeInFrames;


            public uint internalPeriods;

            public ma_channel_mix_mode channelMixMode;


            public uint calculateLFEFromSpatialChannels;

            public ma_data_converter converter;

            public void* pIntermediaryBuffer;


            public uint intermediaryBufferCap;


            public uint intermediaryBufferLen;
        }

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _Anonymous_e__Union
        {
            [FieldOffset(0)]

            public _wasapi_e__Struct wasapi;

            [FieldOffset(0)]

            public _dsound_e__Struct dsound;

            [FieldOffset(0)]

            public _winmm_e__Struct winmm;

            [FieldOffset(0)]

            public _jack_e__Struct jack;

            [FieldOffset(0)]

            public _null_device_e__Struct null_device;

            public unsafe partial struct _wasapi_e__Struct
            {

                public void* pAudioClientPlayback;


                public void* pAudioClientCapture;


                public void* pRenderClient;


                public void* pCaptureClient;


                public void* pDeviceEnumerator;

                public ma_IMMNotificationClient notificationClient;


                public void* hEventPlayback;


                public void* hEventCapture;


                public uint actualBufferSizeInFramesPlayback;


                public uint actualBufferSizeInFramesCapture;


                public uint originalPeriodSizeInFrames;


                public uint originalPeriodSizeInMilliseconds;


                public uint originalPeriods;

                public ma_performance_profile originalPerformanceProfile;


                public uint periodSizeInFramesPlayback;


                public uint periodSizeInFramesCapture;

                public void* pMappedBufferCapture;


                public uint mappedBufferCaptureCap;


                public uint mappedBufferCaptureLen;

                public void* pMappedBufferPlayback;


                public uint mappedBufferPlaybackCap;


                public uint mappedBufferPlaybackLen;

                public ma_atomic_bool32 isStartedCapture;

                public ma_atomic_bool32 isStartedPlayback;


                public uint loopbackProcessID;


                public byte loopbackProcessExclude;


                public byte noAutoConvertSRC;


                public byte noDefaultQualitySRC;


                public byte noHardwareOffloading;


                public byte allowCaptureAutoStreamRouting;


                public byte allowPlaybackAutoStreamRouting;


                public byte isDetachedPlayback;


                public byte isDetachedCapture;

                public ma_wasapi_usage usage;

                public void* hAvrtHandle;


                public void* rerouteLock;
            }

            public unsafe partial struct _dsound_e__Struct
            {

                public void* pPlayback;


                public void* pPlaybackPrimaryBuffer;


                public void* pPlaybackBuffer;


                public void* pCapture;


                public void* pCaptureBuffer;
            }

            public unsafe partial struct _winmm_e__Struct
            {

                public void* hDevicePlayback;


                public void* hDeviceCapture;


                public void* hEventPlayback;


                public void* hEventCapture;


                public uint fragmentSizeInFrames;


                public uint iNextHeaderPlayback;


                public uint iNextHeaderCapture;


                public uint headerFramesConsumedPlayback;


                public uint headerFramesConsumedCapture;


                public byte* pWAVEHDRPlayback;


                public byte* pWAVEHDRCapture;


                public byte* pIntermediaryBufferPlayback;


                public byte* pIntermediaryBufferCapture;


                public byte* _pHeapData;
            }

            public unsafe partial struct _jack_e__Struct
            {

                public void* pClient;


                public void** ppPortsPlayback;


                public void** ppPortsCapture;

                public float* pIntermediaryBufferPlayback;

                public float* pIntermediaryBufferCapture;
            }

            public unsafe partial struct _null_device_e__Struct
            {

                public void* deviceThread;


                public void* operationEvent;


                public void* operationCompletionEvent;


                public void* operationSemaphore;


                public uint operation;

                public ma_result operationResult;

                public ma_timer timer;

                public double priorRunTime;


                public uint currentPeriodFramesRemainingPlayback;


                public uint currentPeriodFramesRemainingCapture;


                public ulong lastProcessedFramePlayback;


                public ulong lastProcessedFrameCapture;

                public ma_atomic_bool32 isStarted;
            }
        }
    }
}
