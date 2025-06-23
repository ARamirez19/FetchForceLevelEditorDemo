using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private List<GameObject> _animatedMenuItems;

    public void OnSettingsClicked()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _creditsMenu.SetActive(false);

        for(int i=0; i<_animatedMenuItems.Count; i++)
        {
            _animatedMenuItems[i].SetActive(false);
        }
    }

    public void OnHomeClicked()
    {
        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _creditsMenu.SetActive(false);
        for (int i = 0; i < _animatedMenuItems.Count; i++)
        {
            _animatedMenuItems[i].SetActive(true);
        }
    }

    public void OnCreditsClicked()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _creditsMenu.SetActive(true);
        for (int i = 0; i < _animatedMenuItems.Count; i++)
        {
            _animatedMenuItems[i].SetActive(false);
        }
    }
}
