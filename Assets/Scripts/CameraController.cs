﻿using UnityEngine;
using System.Collections;

//This class is not needed when playing on mobile.

public class CameraController : MonoBehaviour
{

	void Start ()
	{

	}

	void Update ()
	{
		if (!Application.isMobilePlatform)
			Inputs();
	}

	void Inputs()
	{
		float zz = Camera.main.transform.localEulerAngles.z;
        
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKey(KeyCode.A))
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, zz - (90 * Time.deltaTime));
        else if (Input.GetKey(KeyCode.D))
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, zz + (90 * Time.deltaTime));
        else if (Input.GetKey(KeyCode.Q))
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, zz - (180 * Time.deltaTime));
        else if (Input.GetKey(KeyCode.E))
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, zz + (180 * Time.deltaTime));
#endif

    }
}