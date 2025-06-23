using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas : EnemyController
{
    [SerializeField] private float time = 5;
    private float timer;
    private GameObject player;

    protected override void ExtraStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        timer = time;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer -= Time.deltaTime;
            
            if(timer < 0)
            {
                base.deathAnimation = true;
                player.GetComponent<BoxCollider2D>().enabled = false;
                base.StartCoroutine(DeathTimer(2.0f));

            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer = time;
        }
    }
}
