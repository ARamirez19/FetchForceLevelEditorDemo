using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private float screenTime;
    private float endTime = 1.5f;

    void Update()
    {
        screenTime += Time.deltaTime;
        if(screenTime >= endTime)
        {
            ReturnToLevelSelect();
        }
    }

    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("HomeMenu");
    }
}