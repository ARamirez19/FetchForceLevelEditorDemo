using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public void Home()
    {
        char[] worldNumber = SceneManager.GetActiveScene().name.ToCharArray();
        int world = (int)char.GetNumericValue(worldNumber[5]);
        LevelSelectController.worldCount = world;
        LevelManager.GetInstance().ReturnToLevelSelect();
    }
}
