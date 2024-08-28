using System.Runtime.InteropServices;

namespace MINIAUDIO
{
    public unsafe partial struct ma_device_notification
    {
        public ma_device* pDevice;

        public ma_device_notification_type type;


        public _data_e__Union data;

        [StructLayout(LayoutKind.Explicit)]
        public partial struct _data_e__Union
        {
            [FieldOffset(0)]

            public _started_e__Struct started;

            [FieldOffset(0)]

            public _stopped_e__Struct stopped;

            [FieldOffset(0)]

            public _rerouted_e__Struct rerouted;

            [FieldOffset(0)]

            public _interruption_e__Struct interruption;

            public partial struct _started_e__Struct
            {
                public int _unused;
            }

            public partial struct _stopped_e__Struct
            {
                public int _unused;
            }

            public partial struct _rerouted_e__Struct
            {
                public int _unused;
            }

            public partial struct _interruption_e__Struct
            {
                public int _unused;
            }
        }
    }
}
