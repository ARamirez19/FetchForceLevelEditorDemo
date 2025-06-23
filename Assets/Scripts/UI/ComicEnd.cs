using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicEnd : MonoBehaviour
{
    public void StartLevel()
    {
        if (PlayerPrefs.HasKey("ComicSeen"))
        {
            SceneManager.LoadScene("HomeMenu");
        }
        else
        { 
            PlayerPrefs.SetInt("ComicSeen", 1);
            SceneManager.LoadScene("World1Level1");
        }
	}

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
