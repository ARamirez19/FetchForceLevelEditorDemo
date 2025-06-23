using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : EnemyController
{
	private float speed;
	private float startTime;
	private float journeyLength;
	private float distCovered;
	private float fracJourney;

	private float returnTime;
	private float returnLength;
	private float returnDist;
	private float returnFrac;

	[SerializeField]
	private float incrementSpeed;
	[SerializeField]
	private float maxSpeed;
	[SerializeField]
	private float returnSpeed;

	private bool moveToEndPos = false;
	private bool returnToStartPos = false;
	private bool hitWall = false;
    private bool hitPlayer = false;

	private GameObject field;
	private Transform startPos;
	private Transform endPos;
	private GameObject movementSprite;
    private GameObject player;

    private float waitTime;
    [SerializeField]
    private float playerThrust;
	private Animator myAnim;
	private Vector2 direction;

    // Start is called before the first frame update
    protected override void ExtraStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		field = this.gameObject.transform.GetChild(0).gameObject;
		movementSprite = this.gameObject.transform.GetChild(1).gameObject;
		startPos = this.gameObject.transform.GetChild(2).transform;
		endPos = this.gameObject.transform.GetChild(3).transform;
		field.GetComponent<Renderer>().enabled = false;
		myAnim = movementSprite.GetComponent<Animator>();
		direction = (endPos.transform.position - startPos.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
		if(moveToEndPos == true)
		{
			speed += incrementSpeed;
			distCovered = (Time.time - startTime) * speed;
			fracJourney = distCovered / journeyLength;
			movementSprite.transform.position = Vector2.Lerp(startPos.position, endPos.position, fracJourney);
			myAnim.SetBool("charging", true);
		}
		if(returnToStartPos == true)
		{
			returnDist = (Time.time - returnTime) * returnSpeed;
			returnFrac = returnDist / returnLength;
			movementSprite.transform.position = Vector2.Lerp(endPos.position, startPos.position, returnFrac);
			myAnim.SetBool("charging", false);
			myAnim.SetBool("returning", true);
		}
		if(speed >= maxSpeed)
		{
			speed = maxSpeed;
		}
		if(movementSprite.transform.position == endPos.transform.position)
		{
			if(hitWall == true)
			{
				StartCoroutine(ImpactTimer());
			}
			else
			{
				moveToEndPos = false;
				ReturnCharger();
			}
		}

		if(Input.GetKeyDown (KeyCode.Space))
		{
			MoveCharger();
		}

		if(movementSprite.transform.position.x == endPos.transform.position.x)
		{
			if(hitWall == true)
			{
				StartCoroutine(ImpactTimer());
			}
			else
			{
				moveToEndPos = false;
				ReturnCharger();
			}
		}
		if(movementSprite.transform.position.x == startPos.transform.position.x)
		{
			returnToStartPos = false;
			myAnim.SetBool("returning", false);
			speed = 0;
		}

        if(hitPlayer == true && moveToEndPos == true)
        {
            player.GetComponent<Rigidbody2D>().velocity += direction * playerThrust;
        }
        if(hitPlayer == true && moveToEndPos == false)
        {
            player.GetComponent<PlayerController>().PlayerDead();
        }
    }

	public IEnumerator ImpactTimer()
	{
		yield return new WaitForSeconds(1.5f);
		hitWall = false;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if((other.gameObject.tag == "Player" || other.gameObject.tag == "ShadowCubark") && moveToEndPos == false && returnToStartPos == false)
		{
			MoveCharger();
		}
	}

	new void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Wall")
		{
			myAnim.SetTrigger("hitWall");
            //AkSoundEngine.PostEvent("Play_Charger_Impact", gameObject);
            hitWall = true;
		}
		if(other.gameObject.tag == "Player" || other.gameObject.tag == "ShadowCubark")
		{
            //AkSoundEngine.PostEvent("Play_Charger_Punch", gameObject);
			myAnim.SetTrigger("hitCubark");
            base.deathAnimation = true;
            waitTime = 2.0f;
            hitPlayer = true;
            player.GetComponent<BoxCollider2D>().enabled = false;
            base.StartCoroutine(DeathTimer(waitTime));
		}
        else
        {
            //AkSoundEngine.PostEvent("Play_Charger_Impact", gameObject);
            myAnim.SetTrigger("hitCubark");
        }
	}

	public void MoveCharger()
	{
        moveToEndPos = true;
        startTime = Time.time;
        journeyLength = Vector2.Distance(startPos.position, endPos.position);
	}

	public void ReturnCharger()
	{
		moveToEndPos = false;
		returnTime = Time.time;
		returnLength = Vector2.Distance(endPos.position, startPos.position);
		returnToStartPos = true;
	}
}