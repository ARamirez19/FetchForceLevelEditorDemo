using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsClearPrefs : MonoBehaviour
{
    public void ClearPrefs()
    {
        PlayerPrefs.DeleteAll();
        //CloudSave.Instance.ReadData();
        //CloudSave.Instance.Synchronise();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
