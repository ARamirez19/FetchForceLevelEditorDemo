using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSetup : MonoBehaviour
{
    private float masterVolume;
    private float MXVolume;
    private float SFXVolume;
    private bool MXMute = false;
    private bool SFXMute = false;

    private void Start()
    {
        SetUpDefaultVolumes();
    }

    private void SetUpDefaultVolumes()
    {
        masterVolume = 100f;
        MXVolume = (float)PlayerPrefs.GetInt("MusicOn", 100);
        SFXVolume = (float)PlayerPrefs.GetInt("SoundEffectsOn", 100);
        //AkSoundEngine.SetRTPCValue("Master", 100f);
        //AkSoundEngine.SetRTPCValue("SFX", SFXVolume);
        //AkSoundEngine.SetRTPCValue("MX", MXVolume);
    }
}
