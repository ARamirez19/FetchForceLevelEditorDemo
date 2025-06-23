using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSoundVolume : MonoBehaviour
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;
    [SerializeField] private VolumeType _volumeType;

    private enum VolumeType { SoundEffects, Music }

    private bool _enabled = true;
    private string bus = "";

    private void Start()
    {
        int setVolume = 0;
        if (_volumeType == VolumeType.Music)
        {
            bus = "MX";
            setVolume = PlayerPrefs.GetInt("MusicOn", 100);
        }
        else if (_volumeType == VolumeType.SoundEffects)
        {
            bus = "SFX";
            setVolume = PlayerPrefs.GetInt("SoundEffectsOn", 100);
        }

        _mainImage.sprite = setVolume == 100 ? _enabledSprite : _disabledSprite;
        _enabled = setVolume == 100;
    }

    public void OnEnableClick()
    {
        _enabled = !_enabled;
        _mainImage.sprite = _enabled ? _enabledSprite : _disabledSprite;
        //AkSoundEngine.SetRTPCValue(bus, _enabled ? 100f : -100f);

        PlayerPrefs.SetInt(_volumeType == VolumeType.Music ? "MusicOn" : "SoundEffectsOn", _enabled ? 100 : -100);
    }
}