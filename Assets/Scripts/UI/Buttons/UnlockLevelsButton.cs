using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockLevelsButton : MonoBehaviour
{
    public void UnlockWorlds()
    {
        for(int i = 0; i<4; i++)
        {
            for(int j=0; j<10;j++)
            {
                PlayerPrefs.SetInt("World" + (i+1) + "Level" + (j+1) + "stars", 1);
            }
        }
    }
}
