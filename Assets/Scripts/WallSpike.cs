﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpike : EnemyController
{
    private GameObject player;
    float waitTime = 2.0f;

    protected override void ExtraStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player" && (state == GameState.e_GAMESTATE.PLAYING || state == GameState.e_GAMESTATE.PAUSED))
        {
            //AkSoundEngine.PostEvent("Play_Drill_Impact", gameObject);
            base.deathAnimation = true;
            StartCoroutine(DeathTimer(waitTime));
            player.GetComponent<PlayerController>().PlayerDead();
            player.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
