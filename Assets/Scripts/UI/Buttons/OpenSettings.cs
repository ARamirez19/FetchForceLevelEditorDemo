using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu = null;
    private bool settingsOpen = false;

    public void OpenSettingsWindow()
    {
        if (settingsOpen == false)
        {
            settingsOpen = true;
            _settingsMenu.SetActive(true);
        }
        else
        {
            settingsOpen = false;
            _settingsMenu.SetActive(false);
        }
    }

    public void CloseSettingsWindow()
    {
        settingsOpen = false;
        _settingsMenu.SetActive(false);
    }
}
