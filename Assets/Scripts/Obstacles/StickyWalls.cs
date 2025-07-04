﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyWalls : MonoBehaviour
{
    class StuckObject
    {
        public BaseController stuckObject;
        public float timeStuck = 0f;
    }

    public string audioEventId = "";
    private List<StuckObject> stuckObjects;
    private float stickDuration = 2.0f;
    private GameObject player;
    private Animator myAnim;


    // Start is called before the first frame update
    void Start()
    {
        stuckObjects = new List<StuckObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        myAnim = player.gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStuckObjects();
    }

	void OnCollisionEnter2D(Collision2D other)
	{
        BaseController collidedObject = other.gameObject.GetComponent<BaseController>();
        if (collidedObject != null)
        {
            if (collidedObject.IsAffectedByStickyWalls)
            {
                if(collidedObject.tag == "Player")
                {
                    //myAnim.SetBool("Shock", true);
                    myAnim.SetInteger("StunNumber", 1);
                }
                StuckObject newStuckObject = new StuckObject();
                newStuckObject.stuckObject = collidedObject;
                newStuckObject.stuckObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                stuckObjects.Add(newStuckObject);
                //if (audioEventId != "")
                    //AkSoundEngine.PostEvent(audioEventId, gameObject);
            }
        }
	}

	void UpdateStuckObjects()
	{
        for(int i=0; i<stuckObjects.Count; i++)
        {
            stuckObjects[i].timeStuck += Time.deltaTime;
            if(stuckObjects[i].timeStuck >= stickDuration)
            {
                stuckObjects[i].stuckObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                if(stuckObjects[i].stuckObject.tag == "Player")
                {
                    Haptics.HapticWarning();
                    //myAnim.SetBool("Shock", false);
                    myAnim.SetInteger("StunNumber", 0);
                }
                stuckObjects.RemoveAt(i);
            }
        }
	}
}