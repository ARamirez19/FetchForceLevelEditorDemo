﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private bool affectedByGravity;
    private Rigidbody2D body;
    [SerializeField]
    private float springForce = 100.0f;
    [SerializeField]
    private float springVelocity = 5.0f;
    [SerializeField]
    private bool useVelocity;
    [SerializeField]
    private bool useDirectionalHit;
    private Vector2 direction;
    private Animator myAnim;

	// Use this for initialization
	void Start ()
    {
        myAnim = this.gameObject.GetComponentInChildren<Animator>();
        body = this.GetComponent<Rigidbody2D>();
	    if(!affectedByGravity)
        {
            body.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            body.constraints = RigidbodyConstraints2D.None;
        }
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" || other.collider.tag == "ShadowCubark")
        {
            if (useDirectionalHit)
            {
                direction = (other.transform.position - this.transform.position).normalized;
            }
            else
            {
                direction = this.transform.up;
            }
            if (useVelocity)
            {
                other.rigidbody.velocity = direction * springVelocity;
            }
            else
            {
                other.rigidbody.AddForce(direction * springForce * 1000, ForceMode2D.Impulse);
            }
            myAnim.SetTrigger("myTrigger");
        }
		if (other.collider.tag == "Enemy" && other.collider.name.Contains("BallEnemy")) 
		{
			if (useDirectionalHit)
			{
				direction = (other.transform.position - this.transform.position).normalized;
			}
			else
			{
				direction = this.transform.up;
			}
			if (useVelocity)
			{
				other.rigidbody.velocity = direction * springVelocity;
			}
			else
			{
				other.rigidbody.AddForce(direction * springForce * 10, ForceMode2D.Impulse);
			}
			myAnim.SetTrigger("myTrigger");
		}
    }
}
