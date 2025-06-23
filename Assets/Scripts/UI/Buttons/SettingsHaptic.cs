using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsHaptic : MonoBehaviour
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;
    private bool _enabled = true;

    private void Start()
    {
        int hapticsEnabled = PlayerPrefs.GetInt("HapticsEnabled", 1);
        if (hapticsEnabled == 0)
        {
            _enabled = false;
            _mainImage.sprite = _disabledSprite;
        }
    }

    public void OnButtonClick()
    {
        _enabled = !_enabled;
        _mainImage.sprite = _enabled ? _enabledSprite : _disabledSprite;
        Haptics.Enabled = _enabled;

        PlayerPrefs.SetInt("HapticsEnabled", _enabled ? 1 : 0);
    }
}
