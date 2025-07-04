﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : EnemyController
{
	public GameObject player;
	[SerializeField]
	private float movementSpeed = 5.0f;
	private Vector2 enemyDirection;
    // Start is called before the first frame update
    protected override void ExtraStart()
    {
		player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		if(state == GameState.e_GAMESTATE.PLAYING)
		{
			enemyDirection = player.transform.position - transform.position;
			enemyDirection = enemyDirection.normalized;
			transform.Translate(enemyDirection * movementSpeed * Time.deltaTime);
		}
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" && (state == GameState.e_GAMESTATE.PLAYING || state == GameState.e_GAMESTATE.PAUSED))
        {
            base.deathAnimation = true;
            StartCoroutine(DeathTimer(2f));
            player.GetComponent<PlayerController>().PlayerDead();
            player.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}