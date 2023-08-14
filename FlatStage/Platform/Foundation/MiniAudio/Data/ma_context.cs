using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlatStage.Foundation.MiniAudio
{
    public unsafe partial struct ma_context
    {
        public ma_backend_callbacks callbacks;

        public ma_backend backend;

        public ma_log* pLog;

        public ma_log log;

        public ma_thread_priority threadPriority;

        [NativeTypeName("size_t")]
        public ulong threadStackSize;

        public void* pUserData;

        public ma_allocation_callbacks allocationCallbacks;

        [NativeTypeName("ma_mutex")]
        public void* deviceEnumLock;

        [NativeTypeName("ma_mutex")]
        public void* deviceInfoLock;

        [NativeTypeName("ma_uint32")]
        public uint deviceInfoCapacity;

        [NativeTypeName("ma_uint32")]
        public uint playbackDeviceInfoCount;

        [NativeTypeName("ma_uint32")]
        public uint captureDeviceInfoCount;

        public ma_device_info* pDeviceInfos;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7310_C5")]
        public _Anonymous1_e__Union Anonymous1;

        [NativeTypeName("__AnonymousRecord_miniaudio_L7664_C5")]
        public _Anonymous2_e__Union Anonymous2;

        public ref _Anonymous1_e__Union._wasapi_e__Struct wasapi
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous1_e__Union* pField = &Anonymous1)
                {
                    return ref pField->wasapi;
                }
            }
        }

        public ref _Anonymous1_e__Union._dsound_e__Struct dsound
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous1_e__Union* pField = &Anonymous1)
                {
                    return ref pField->dsound;
                }
            }
        }

        public ref _Anonymous1_e__Union._winmm_e__Struct winmm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous1_e__Union* pField = &Anonymous1)
                {
                    return ref pField->winmm;
                }
            }
        }

        public ref _Anonymous1_e__Union._jack_e__Struct jack
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous1_e__Union* pField = &Anonymous1)
                {
                    return ref pField->jack;
                }
            }
        }

        public ref _Anonymous1_e__Union._null_backend_e__Struct null_backend
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous1_e__Union* pField = &Anonymous1)
                {
                    return ref pField->null_backend;
                }
            }
        }

        public ref _Anonymous2_e__Union._win32_e__Struct win32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous2_e__Union* pField = &Anonymous2)
                {
                    return ref pField->win32;
                }
            }
        }

        public ref int _unused
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (_Anonymous2_e__Union* pField = &Anonymous2)
                {
                    return ref pField->_unused;
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _Anonymous1_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7313_C9")]
            public _wasapi_e__Struct wasapi;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7329_C9")]
            public _dsound_e__Struct dsound;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7339_C9")]
            public _winmm_e__Struct winmm;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7509_C9")]
            public _jack_e__Struct jack;

            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7657_C9")]
            public _null_backend_e__Struct null_backend;

            public unsafe partial struct _wasapi_e__Struct
            {
                [NativeTypeName("ma_thread")]
                public void* commandThread;

                [NativeTypeName("ma_mutex")]
                public void* commandLock;

                [NativeTypeName("ma_semaphore")]
                public void* commandSem;

                [NativeTypeName("ma_uint32")]
                public uint commandIndex;

                [NativeTypeName("ma_uint32")]
                public uint commandCount;

                [NativeTypeName("ma_context_command__wasapi[4]")]
                public _commands_e__FixedBuffer commands;

                [NativeTypeName("ma_handle")]
                public void* hAvrt;

                [NativeTypeName("ma_proc")]
                public void* AvSetMmThreadCharacteristicsA;

                [NativeTypeName("ma_proc")]
                public void* AvRevertMmThreadcharacteristics;

                [NativeTypeName("ma_handle")]
                public void* hMMDevapi;

                [NativeTypeName("ma_proc")]
                public void* ActivateAudioInterfaceAsync;

                public partial struct _commands_e__FixedBuffer
                {
                    public ma_context_command__wasapi e0;
                    public ma_context_command__wasapi e1;
                    public ma_context_command__wasapi e2;
                    public ma_context_command__wasapi e3;

                    public unsafe ref ma_context_command__wasapi this[int index]
                    {
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        get
                        {
                            fixed (ma_context_command__wasapi* pThis = &e0)
                            {
                                return ref pThis[index];
                            }
                        }
                    }
                }
            }

            public unsafe partial struct _dsound_e__Struct
            {
                [NativeTypeName("ma_handle")]
                public void* hDSoundDLL;

                [NativeTypeName("ma_proc")]
                public void* DirectSoundCreate;

                [NativeTypeName("ma_proc")]
                public void* DirectSoundEnumerateA;

                [NativeTypeName("ma_proc")]
                public void* DirectSoundCaptureCreate;

                [NativeTypeName("ma_proc")]
                public void* DirectSoundCaptureEnumerateA;
            }

            public unsafe partial struct _winmm_e__Struct
            {
                [NativeTypeName("ma_handle")]
                public void* hWinMM;

                [NativeTypeName("ma_proc")]
                public void* waveOutGetNumDevs;

                [NativeTypeName("ma_proc")]
                public void* waveOutGetDevCapsA;

                [NativeTypeName("ma_proc")]
                public void* waveOutOpen;

                [NativeTypeName("ma_proc")]
                public void* waveOutClose;

                [NativeTypeName("ma_proc")]
                public void* waveOutPrepareHeader;

                [NativeTypeName("ma_proc")]
                public void* waveOutUnprepareHeader;

                [NativeTypeName("ma_proc")]
                public void* waveOutWrite;

                [NativeTypeName("ma_proc")]
                public void* waveOutReset;

                [NativeTypeName("ma_proc")]
                public void* waveInGetNumDevs;

                [NativeTypeName("ma_proc")]
                public void* waveInGetDevCapsA;

                [NativeTypeName("ma_proc")]
                public void* waveInOpen;

                [NativeTypeName("ma_proc")]
                public void* waveInClose;

                [NativeTypeName("ma_proc")]
                public void* waveInPrepareHeader;

                [NativeTypeName("ma_proc")]
                public void* waveInUnprepareHeader;

                [NativeTypeName("ma_proc")]
                public void* waveInAddBuffer;

                [NativeTypeName("ma_proc")]
                public void* waveInStart;

                [NativeTypeName("ma_proc")]
                public void* waveInReset;
            }

            public unsafe partial struct _jack_e__Struct
            {
                [NativeTypeName("ma_handle")]
                public void* jackSO;

                [NativeTypeName("ma_proc")]
                public void* jack_client_open;

                [NativeTypeName("ma_proc")]
                public void* jack_client_close;

                [NativeTypeName("ma_proc")]
                public void* jack_client_name_size;

                [NativeTypeName("ma_proc")]
                public void* jack_set_process_callback;

                [NativeTypeName("ma_proc")]
                public void* jack_set_buffer_size_callback;

                [NativeTypeName("ma_proc")]
                public void* jack_on_shutdown;

                [NativeTypeName("ma_proc")]
                public void* jack_get_sample_rate;

                [NativeTypeName("ma_proc")]
                public void* jack_get_buffer_size;

                [NativeTypeName("ma_proc")]
                public void* jack_get_ports;

                [NativeTypeName("ma_proc")]
                public void* jack_activate;

                [NativeTypeName("ma_proc")]
                public void* jack_deactivate;

                [NativeTypeName("ma_proc")]
                public void* jack_connect;

                [NativeTypeName("ma_proc")]
                public void* jack_port_register;

                [NativeTypeName("ma_proc")]
                public void* jack_port_name;

                [NativeTypeName("ma_proc")]
                public void* jack_port_get_buffer;

                [NativeTypeName("ma_proc")]
                public void* jack_free;

                [NativeTypeName("char *")]
                public sbyte* pClientName;

                [NativeTypeName("ma_bool32")]
                public uint tryStartServer;
            }

            public partial struct _null_backend_e__Struct
            {
                public int _unused;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _Anonymous2_e__Union
        {
            [FieldOffset(0)]
            [NativeTypeName("__AnonymousRecord_miniaudio_L7667_C9")]
            public _win32_e__Struct win32;

            [FieldOffset(0)]
            public int _unused;

            public unsafe partial struct _win32_e__Struct
            {
                [NativeTypeName("ma_handle")]
                public void* hOle32DLL;

                [NativeTypeName("ma_proc")]
                public void* CoInitialize;

                [NativeTypeName("ma_proc")]
                public void* CoInitializeEx;

                [NativeTypeName("ma_proc")]
                public void* CoUninitialize;

                [NativeTypeName("ma_proc")]
                public void* CoCreateInstance;

                [NativeTypeName("ma_proc")]
                public void* CoTaskMemFree;

                [NativeTypeName("ma_proc")]
                public void* PropVariantClear;

                [NativeTypeName("ma_proc")]
                public void* StringFromGUID2;

                [NativeTypeName("ma_handle")]
                public void* hUser32DLL;

                [NativeTypeName("ma_proc")]
                public void* GetForegroundWindow;

                [NativeTypeName("ma_proc")]
                public void* GetDesktopWindow;

                [NativeTypeName("ma_handle")]
                public void* hAdvapi32DLL;

                [NativeTypeName("ma_proc")]
                public void* RegOpenKeyExA;

                [NativeTypeName("ma_proc")]
                public void* RegCloseKey;

                [NativeTypeName("ma_proc")]
                public void* RegQueryValueExA;

                [NativeTypeName("long")]
                public int CoInitializeResult;
            }
        }
    }
}
