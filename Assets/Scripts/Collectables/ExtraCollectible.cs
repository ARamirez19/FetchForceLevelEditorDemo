using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCollectible : MonoBehaviour
{
    public string audioEventID = "";
    private GameObject goal;
    private float moveTime = 0.75f;
    private float opacityChange = 1.0f;
    private SpriteRenderer[] childSprites;
    private Animator myAnim;

    void Start()
    {
        goal = GameObject.FindGameObjectWithTag("Goal");
        childSprites = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        myAnim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(string.Compare(other.tag, "Player") == 0)
        {
            myAnim.SetTrigger("collected");

           if(audioEventID != "")
                //AkSoundEngine.PostEvent(audioEventID, gameObject);
            Haptics.HapticSuccess();
            LevelManager.GetInstance().extraCollectableEarned = true;
            StartCoroutine(MoveToGoal(this.transform, goal.transform.position, moveTime));
        }
    }

    private IEnumerator MoveToGoal(Transform transform, Vector2 position, float timeToMove)
    {
        Vector2 currentPosition = transform.position;
        float time = 0f;
        while(time < 1)
        {
            time += Time.deltaTime / timeToMove;
            opacityChange -= Time.deltaTime;
            foreach(SpriteRenderer spriteRenderer in childSprites)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacityChange);
            }
            transform.position = Vector2.Lerp(currentPosition, position, time);
            yield return null;
        }
        
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }
}