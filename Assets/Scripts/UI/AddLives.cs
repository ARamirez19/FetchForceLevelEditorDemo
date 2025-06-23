using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLives : MonoBehaviour
{
    public void AddMoreLives()
    {
        PlayerPrefs.SetInt("Lives", 15);
    }
}
