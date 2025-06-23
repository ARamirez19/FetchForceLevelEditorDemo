using System.Collections;
using System.Collections.Generic;
using GameState;
using UnityEngine;

public class LivesAnimTrigger : MonoBehaviour, IGameState
{
    [SerializeField] private ParticleSystem crumbs;
    private Animator myAnim;
    private e_GAMESTATE state;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        if (myAnim != null)
            Debug.Log("Lives Animator found.");
    }

    public void PlayAnimation()
    {
        myAnim.SetTrigger("lost_life");
        crumbs.Play();
    }

    public void ChangeState(e_GAMESTATE state)
    {
        throw new System.NotImplementedException();
    }
}