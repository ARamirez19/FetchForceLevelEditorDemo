using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Comic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	private Slider slider;
	private Animator comic;
	private AnimatorClipInfo [] comicClipInfo;
	private bool held = false;

	// Start is called before the first frame update
	void Start()
	{
		slider = this.GetComponent<Slider>();
		comic = GameObject.Find("Comic").GetComponent<Animator>();
		comicClipInfo = comic.GetCurrentAnimatorClipInfo(0);
	}

	// Update is called once per frame
	void Update()
	{
		if(held)
		{
			comic.Play(comicClipInfo[0].clip.name, -1, slider.normalizedValue);
		}
		else
		{
			slider.value = comic.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		held = true;
	}

	public void OnPointerUp(PointerEventData data)
	{
		held = false;
	}
}
