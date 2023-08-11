﻿using System;
using System.Runtime.InteropServices;

namespace FlatStage.Foundation.TinyAudio.Wasapi;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct DeviceEnumeratorInst
{
    public DeviceEnumeratorV* Vtbl;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct DeviceEnumeratorV
{
    public delegate* unmanaged[Stdcall]<DeviceEnumeratorInst*, Guid*, void**, uint> QueryInterface;
    public delegate* unmanaged[Stdcall]<DeviceEnumeratorInst*, uint> AddRef;
    public delegate* unmanaged[Stdcall]<DeviceEnumeratorInst*, uint> Release;

    public IntPtr EnumAudioEndpoints;

    public delegate* unmanaged[Stdcall]<DeviceEnumeratorInst*, EDataFlow, ERole, MMDeviceInst**, uint>
        GetDefaultAudioEndpoint;

    public IntPtr GetDevice;
    public IntPtr RegisterEndpointNotificationCallback;
    public IntPtr UnregisterEndpointNotificationCallback;
}