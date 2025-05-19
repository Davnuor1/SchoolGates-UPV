using UnityEngine;
using System.Runtime.InteropServices;

public static class DeviceDetector
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsTouchDevice();
#endif

    public static bool isTouchDevice
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsTouchDevice() == 1;
#else
            return Input.touchSupported;
#endif
        }
    }
}
