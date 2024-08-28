namespace MINIAUDIO
{
    public unsafe partial struct ma_device_config
    {
        public ma_device_type deviceType;


        public uint sampleRate;


        public uint periodSizeInFrames;


        public uint periodSizeInMilliseconds;


        public uint periods;

        public ma_performance_profile performanceProfile;


        public byte noPreSilencedOutputBuffer;


        public byte noClip;


        public byte noDisableDenormals;


        public byte noFixedSizedCallback;


        public IntPtr dataCallback;


        public IntPtr notificationCallback;


        public IntPtr stopCallback;

        public void* pUserData;

        public ma_resampler_config resampling;


        public _playback_e__Struct playback;


        public _capture_e__Struct capture;


        public _wasapi_e__Struct wasapi;


        public _alsa_e__Struct alsa;


        public _pulse_e__Struct pulse;


        public _coreaudio_e__Struct coreaudio;


        public _opensl_e__Struct opensl;


        public _aaudio_e__Struct aaudio;

        public unsafe partial struct _playback_e__Struct
        {

            public ma_device_id* pDeviceID;

            public ma_format format;


            public uint channels;


            public byte* pChannelMap;

            public ma_channel_mix_mode channelMixMode;


            public uint calculateLFEFromSpatialChannels;

            public ma_share_mode shareMode;
        }

        public unsafe partial struct _capture_e__Struct
        {

            public ma_device_id* pDeviceID;

            public ma_format format;


            public uint channels;


            public byte* pChannelMap;

            public ma_channel_mix_mode channelMixMode;


            public uint calculateLFEFromSpatialChannels;

            public ma_share_mode shareMode;
        }

        public partial struct _wasapi_e__Struct
        {
            public ma_wasapi_usage usage;


            public byte noAutoConvertSRC;


            public byte noDefaultQualitySRC;


            public byte noAutoStreamRouting;


            public byte noHardwareOffloading;


            public uint loopbackProcessID;


            public byte loopbackProcessExclude;
        }

        public partial struct _alsa_e__Struct
        {

            public uint noMMap;


            public uint noAutoFormat;


            public uint noAutoChannels;


            public uint noAutoResample;
        }

        public unsafe partial struct _pulse_e__Struct
        {

            public sbyte* pStreamNamePlayback;


            public sbyte* pStreamNameCapture;
        }

        public partial struct _coreaudio_e__Struct
        {

            public uint allowNominalSampleRateChange;
        }

        public partial struct _opensl_e__Struct
        {
            public ma_opensl_stream_type streamType;

            public ma_opensl_recording_preset recordingPreset;


            public uint enableCompatibilityWorkarounds;
        }

        public partial struct _aaudio_e__Struct
        {
            public ma_aaudio_usage usage;

            public ma_aaudio_content_type contentType;

            public ma_aaudio_input_preset inputPreset;

            public ma_aaudio_allowed_capture_policy allowedCapturePolicy;


            public uint noAutoStartAfterReroute;


            public uint enableCompatibilityWorkarounds;
        }
    }
}
