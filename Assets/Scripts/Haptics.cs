using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class Haptics : MonoBehaviour
{
    private static bool _enabled = true;
    public static bool Enabled { get { return _enabled; } set { _enabled = value; } }

    private void Awake()
    {
        MMVibrationManager.iOSInitializeHaptics();
        int hapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1);

        if(hapticsEnabled == 0)
        {
            _enabled = false;
        }
    }

    public static void HapticSelection()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.Selection);
    }
    public static void HapticSuccess()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.Success);
    }
    public static void HapticWarning()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.Warning);
    }
    public static void HapticFailure()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.Failure);
    }
    public static void HapticLight()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }
    public static void HapticMedium()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
    }
    public static void HapticHeavy()
    {
        if (_enabled)
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
    }
}