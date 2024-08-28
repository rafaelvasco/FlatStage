using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_context
    {
        public ma_backend_callbacks callbacks;

        public ma_backend backend;

        public ma_log* pLog;

        public ma_log log;

        public ma_thread_priority threadPriority;


        public ulong threadStackSize;

        public void* pUserData;

        public ma_allocation_callbacks allocationCallbacks;


        public void* deviceEnumLock;


        public void* deviceInfoLock;


        public uint deviceInfoCapacity;


        public uint playbackDeviceInfoCount;


        public uint captureDeviceInfoCount;

        public ma_device_info* pDeviceInfos;


        public _Anonymous1_e__Union Anonymous1;


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

            public _wasapi_e__Struct wasapi;

            [FieldOffset(0)]

            public _dsound_e__Struct dsound;

            [FieldOffset(0)]

            public _winmm_e__Struct winmm;

            [FieldOffset(0)]

            public _jack_e__Struct jack;

            [FieldOffset(0)]

            public _null_backend_e__Struct null_backend;

            public unsafe partial struct _wasapi_e__Struct
            {

                public void* commandThread;


                public void* commandLock;


                public void* commandSem;


                public uint commandIndex;


                public uint commandCount;


                public _commands_e__FixedBuffer commands;


                public void* hAvrt;


                public void* AvSetMmThreadCharacteristicsA;


                public void* AvRevertMmThreadcharacteristics;


                public void* hMMDevapi;


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

                public void* hDSoundDLL;


                public void* DirectSoundCreate;


                public void* DirectSoundEnumerateA;


                public void* DirectSoundCaptureCreate;


                public void* DirectSoundCaptureEnumerateA;
            }

            public unsafe partial struct _winmm_e__Struct
            {

                public void* hWinMM;


                public void* waveOutGetNumDevs;


                public void* waveOutGetDevCapsA;


                public void* waveOutOpen;


                public void* waveOutClose;


                public void* waveOutPrepareHeader;


                public void* waveOutUnprepareHeader;


                public void* waveOutWrite;


                public void* waveOutReset;


                public void* waveInGetNumDevs;


                public void* waveInGetDevCapsA;


                public void* waveInOpen;


                public void* waveInClose;


                public void* waveInPrepareHeader;


                public void* waveInUnprepareHeader;


                public void* waveInAddBuffer;


                public void* waveInStart;


                public void* waveInReset;
            }

            public unsafe partial struct _jack_e__Struct
            {

                public void* jackSO;


                public void* jack_client_open;


                public void* jack_client_close;


                public void* jack_client_name_size;


                public void* jack_set_process_callback;


                public void* jack_set_buffer_size_callback;


                public void* jack_on_shutdown;


                public void* jack_get_sample_rate;


                public void* jack_get_buffer_size;


                public void* jack_get_ports;


                public void* jack_activate;


                public void* jack_deactivate;


                public void* jack_connect;


                public void* jack_port_register;


                public void* jack_port_name;


                public void* jack_port_get_buffer;


                public void* jack_free;


                public sbyte* pClientName;


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

            public _win32_e__Struct win32;

            [FieldOffset(0)]
            public int _unused;

            public unsafe partial struct _win32_e__Struct
            {

                public void* hOle32DLL;


                public void* CoInitialize;


                public void* CoInitializeEx;


                public void* CoUninitialize;


                public void* CoCreateInstance;


                public void* CoTaskMemFree;


                public void* PropVariantClear;


                public void* StringFromGUID2;


                public void* hUser32DLL;


                public void* GetForegroundWindow;


                public void* GetDesktopWindow;


                public void* hAdvapi32DLL;


                public void* RegOpenKeyExA;


                public void* RegCloseKey;


                public void* RegQueryValueExA;


                public int CoInitializeResult;
            }
        }
    }
}
