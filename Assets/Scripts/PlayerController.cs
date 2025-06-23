using UnityEngine;
using System.Collections;
using GameState;
using UnityEngine.UI;
using TMPro;

public class PlayerController : BaseController
{
    private bool playerInGoal = false;
    private bool canFreezeLevel = true;
    private float completionTimer = 0.1f;
    private float timer = 0.0f;
    private float speedCap = 100.0f;
    private bool playerInputEnabled = false;
    private float jetpackSpeed = 6.0f;
    private float maxJetpackSpeed = 30.0f;
    private float rotationSpeed = 1.0f;
    private int jetpackTimer = 1;
    private float jetpackTime = 2;
    private float jetpackFuel = 10.0f;
    private float maxFuel;
    private bool jetpackEnabled = true;
    private bool jetpackCooldown = true;
    private bool rotatePlayer = false;
    private bool upIsUp = false;
    private bool isGameSlowed = false;
    private float slingshotPower = 5000f;

    private Rigidbody2D playerRigidbody;
    [HideInInspector]
    public bool portalEligible = true;
    [HideInInspector]
    public bool deadPlayer = false;
    private Animator myAnim;

    private Image healthPotion;
    private TextMeshProUGUI livesCount;
    private int display;

    public bool HasTapped { get; private set; }
    //PC control launch indicator
    private LineRenderer lineRenderer;
    public float maxLineLength = 5f; // Maximum stretch length

    private Vector3 startPosition;
    private bool isDragging = false;

    void Update()
    {
        if(playerRigidbody.velocity.magnitude > speedCap)
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized * speedCap;
        }
        
        if ((state == e_GAMESTATE.PLAYING || state == e_GAMESTATE.PAUSED) && canFreezeLevel && playerInputEnabled)
            Inputs();

        if (state == e_GAMESTATE.PLAYING)
        {
            if (playerInputEnabled == false)
                playerInputEnabled = true;

            if (playerInGoal)
            {
                Time.timeScale = 1.0f;
                isGameSlowed = false;

                gsManager.SetGameState(e_GAMESTATE.LEVELCOMPLETE);
            }
        }
        if (!jetpackCooldown)
        {
            if (jetpackFuel < maxFuel)
            {
                jetpackFuel = jetpackFuel + 0.05f;
            }
        }
        else
        {
            jetpackTime += Time.deltaTime;
            if(jetpackTime > jetpackTimer)
            {
                jetpackTime = jetpackTimer;
            }
        }
        if(deadPlayer == true)
        {
            //myAnim.SetBool("Dead", true);

            transform.Rotate(Vector3.forward * 500.0f * Time.deltaTime);
        }
	}

    private void FixedUpdate()
    {
        if(state == e_GAMESTATE.PLAYING && rotatePlayer)
        {
            float angle = Mathf.Atan2(Physics2D.gravity.x, -Physics2D.gravity.y) * Mathf.Rad2Deg; //Converts the gravity x and y values into an angle
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.time * rotationSpeed/1000); //Rotates player to that angle
        }
    }

    private void Inputs()
	{
        #if UNITY_EDITOR || UNITY_STANDALONE

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == e_GAMESTATE.PLAYING || state == e_GAMESTATE.PAUSED)
            {
                if(jetpackEnabled)
                {
                    //UseJetpack();
                }
            }
        }


        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(state == e_GAMESTATE.PLAYING) //Check this - may not need while paused because that's weird...
            {
                
                

                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
                Collider2D col = Physics2D.OverlapCircle(pos, 1);

                if (col != null && col.CompareTag("Player"))
                {
                    startPosition = this.transform.position;
                    isDragging = true;
                    Time.timeScale = .25f;
                    isGameSlowed = true;

                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && isGameSlowed)
        {
            if (state == e_GAMESTATE.PLAYING) //Put in a thing to not allow pausing while game is slowed;
            {
                Time.timeScale = 1f;
                isGameSlowed = false;
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);

                Vector2 diff = new Vector2(this.transform.position.x, this.transform.position.y) - pos;


                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.AddForce(diff * slingshotPower);

                isDragging = false;
                lineRenderer.enabled = false;

            }
            
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            if (state == e_GAMESTATE.PLAYING) //Put in a thing to not allow pausing while game is slowed;
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0f; // Keep it in 2D space
                startPosition = this.transform.position;
                Vector3 dragVector = -(startPosition - mouseWorldPosition);
                float dragDistance = Mathf.Min(dragVector.magnitude, maxLineLength); // Limit max length

                Vector3 launchDirection = -dragVector.normalized; // Reverse direction
                Vector3 endPosition = startPosition + launchDirection * dragDistance;

                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, endPosition);
                if(lineRenderer.enabled == false)
                {
                    lineRenderer.enabled = true;
                }
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(state == e_GAMESTATE.PLAYING)
            {
                lineRenderer.enabled = false;
            }
        }
#else
        if(!jetpackCooldown)
        {
		if (Input.touchCount > 0)
		    {
			    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			    RaycastHit hit;

			    if (state == e_GAMESTATE.PLAYING || state == e_GAMESTATE.PAUSED)
			    {
				    if(Physics.Raycast(ray,out hit) && hit.collider.tag == "PausePlay" && jetpackEnabled)
				    {
					    //levelManager.ToggleLevelFreeze();
                        UseJetpack();
				    }
			    }
		    }
        }
        else
        {
		    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		    {
			    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			    RaycastHit hit;

			    if (state == e_GAMESTATE.PLAYING || state == e_GAMESTATE.PAUSED)
			    {
				    if(Physics.Raycast(ray,out hit) && hit.collider.tag == "PausePlay" && jetpackEnabled)
				    {
					    //levelManager.ToggleLevelFreeze();
                        UseJetpack();
				    }
			    }
		    }
        }
#endif
    }

    public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Goal")
		{
			playerInGoal = true;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            this.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			Debug.Log ("Player in goal!");
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Goal")
		{
			//playerInGoal = false;
			Debug.Log ("Player out of goal!");
		}
	}

	protected override void ExtraStart ()
	{
		canFreezeLevel = levelManager.GetPlayerFreezeStatus();
        maxFuel = jetpackFuel;
        playerRigidbody = this.GetComponent<Rigidbody2D>();
        myAnim = this.gameObject.GetComponentInChildren<Animator>();
        lineRenderer = this.GetComponent<LineRenderer>();
        
    }

    private void UseJetpack()
    {
        myAnim.SetTrigger("Boost");
        Haptics.HapticSelection();
        if (!HasTapped)
            HasTapped = true;

        if (!jetpackCooldown)
        {
            if (playerRigidbody.velocity.magnitude < maxJetpackSpeed && jetpackFuel > 0.0f)
            {
                playerRigidbody.AddForce(this.transform.up * jetpackSpeed * 10.0f, ForceMode2D.Impulse);
                jetpackFuel = jetpackFuel - 0.1f;
            }
        }
        else
        {
            if (jetpackTime >= jetpackTimer)
            {
                if (upIsUp)
                {
                    playerRigidbody.AddForce(new Vector2(0, 1) * jetpackSpeed * 200.0f, ForceMode2D.Impulse);
                }
                else
                {
                    playerRigidbody.AddForce(this.transform.up * jetpackSpeed * 200.0f, ForceMode2D.Impulse);
                }
                jetpackTime = 0;
            }
        }
    }

    public void UseJetpack(bool s)
    {
        jetpackEnabled = s;
    }


    public void UseCooldown(bool s)
    {
        jetpackCooldown = s;
    }


    public void RotatePlayer(bool s)
    {
        rotatePlayer = s;
    }

    public void UpIsUp(bool s)
    {
        upIsUp = s;
    }

    public void PlayerDead()
    {
        deadPlayer = true;
        Time.timeScale = 1f;
        isGameSlowed = false;
        //myAnim.SetInteger("DieNumber", 1);
    }

    public void PlayerShotDead()
    {
        Time.timeScale = 1f;
        Haptics.HapticFailure();
        gsManager.SetGameState(e_GAMESTATE.DEAD);
        StartCoroutine(PlayerShot());
    }

    public IEnumerator PlayerShot()
    {
        deadPlayer = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //healthPotion = GameObject.FindGameObjectWithTag("HealthPotion").GetComponent<Image>();
        //livesCount = GameObject.FindGameObjectWithTag("LivesCount").GetComponent<TextMeshProUGUI>();
        //healthPotion.GetComponent<LivesAnimTrigger>().PlayAnimation();
        //livesCount.enabled = true;
        //display = SaveManager.Instance.LoadLives();
        //livesCount.SetText(display.ToString());
        
        yield return new WaitForSeconds(1.0f);
        //display--;
        //livesCount.SetText(display.ToString());
        yield return new WaitForSeconds(1.0f);
        levelManager.DeathRestart();
    }
}
