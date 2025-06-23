using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private bool onlyPlayer = false;
    [SerializeField] private GameObject otherPortal;
    private float timer = 0.5f;

    public float moveTime = 0.75f;
    private GameObject player;
    private SpriteRenderer[] childSprites;
    private Animator myAnim;

    private void Start()
    {
        if(otherPortal.GetComponent<Portal>().GetOnlyPlayerStatus() == true)
        {
            onlyPlayer = true;
        }
        else
        {
            onlyPlayer = false;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        childSprites = player.gameObject.GetComponentsInChildren<SpriteRenderer>();
        myAnim = player.gameObject.GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(timer < 0.5f)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if(onlyPlayer)
        //{
        //    if(other.tag != "Player")
        //    {
        //        return;
        //    }
        //}
        if (other.gameObject.tag == "Player")
        {
            if (timer >= 0.5f && player.GetComponent<PlayerController>().portalEligible == true)
            {
                otherPortal.GetComponent<Portal>().Pause();
                StartCoroutine(MoveToPosition(other.transform, otherPortal.transform.position, moveTime));

                //other.transform.position = otherPortal.transform.position;
            }
        }
    }

    public IEnumerator MoveToPosition(Transform transform, Vector2 positon, float timeToMove)
    {
        //player.transform.position = this.transform.position;
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        myAnim.SetTrigger("PortalEnter");
        //AkSoundEngine.PostEvent("Play_Portal_Enter", gameObject);
        float testing = 0f;
        while (testing < 1)
        {
            testing += Time.deltaTime;
            player.transform.position = Vector2.Lerp(player.transform.position, this.transform.position, testing/timeToMove);
            yield return null;
        }
        /*foreach (SpriteRenderer spriteRenderer in childSprites)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
        }*/
        player.GetComponent<PlayerController>().portalEligible = false;
        Vector2 currentPostion = player.transform.position;
        float movement = 0f;
        while (movement < 1)
        {
            movement += Time.deltaTime / timeToMove;
            player.transform.position = Vector2.Lerp(currentPostion, positon, movement);
            yield return null;   
        }
        myAnim.SetTrigger("PortalExit");
        //AkSoundEngine.PostEvent("Play_Portal_Exit", gameObject);
        player.GetComponent<BoxCollider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        /*foreach(SpriteRenderer spriteRenderer in childSprites)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }*/
        yield return new WaitForSeconds(2.0f);
        player.GetComponent<PlayerController>().portalEligible = true;
    }

    public void Pause()
    {
        timer = 0;
    }

    public bool GetOnlyPlayerStatus()
    {
        return onlyPlayer;
    }
}