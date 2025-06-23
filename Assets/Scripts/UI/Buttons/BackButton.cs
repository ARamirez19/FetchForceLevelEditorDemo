using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
    public void Back()
    {
        char[] worldNumber = SceneManager.GetActiveScene().name.ToCharArray();
        int world = (int)char.GetNumericValue(worldNumber[5]);
        LevelSelectController.worldCount = world;
        LevelManager.GetInstance().ReturnToLevelSelect();
    }
}
