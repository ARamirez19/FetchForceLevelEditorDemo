using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockonEnemy : EnemyController
{
	public GameObject player;
	[SerializeField]
	private float movementSpeed;
	[SerializeField]
	private float distance;
	[SerializeField]
	private float lockonTimer;
    private float waitTime = 2.0f;
    private Vector2 enemyDirection;
	private bool lockingOn = false;
    private bool confirmedKill = false;
    [SerializeField]
    private Animator myAnim;

	protected override void ExtraStart()
	{
		player = GameObject.FindGameObjectWithTag("Player");
        myAnim = this.gameObject.GetComponentInChildren<Animator>();
	}
	
	void Update()
	{
		if(state == GameState.e_GAMESTATE.PLAYING && lockingOn == false && confirmedKill == false)
		{
			enemyDirection = player.transform.position - transform.position;
			enemyDirection = enemyDirection.normalized;
			transform.Translate(enemyDirection * movementSpeed * Time.deltaTime);
		}
		
		if(Vector2.Distance(this.gameObject.transform.position, player.transform.position) < distance)
		{
			lockingOn = true;
			StartCoroutine(LockonTimer());
		}
	}
	
	IEnumerator LockonTimer()
	{
		yield return new WaitForSeconds(lockonTimer);
		if(Vector2.Distance(this.gameObject.transform.position, player.transform.position) < distance)
		{
            //levelManager.RestartLevel();
            confirmedKill = true;
            myAnim.SetTrigger("Firing");
            base.deathAnimation = true;
            StartCoroutine(DeathTimer(waitTime));
            player.GetComponent<PlayerController>().PlayerDead();
            player.GetComponent<BoxCollider2D>().enabled = false;
		}
		else
		{
			lockingOn = false;
		}
	}
}