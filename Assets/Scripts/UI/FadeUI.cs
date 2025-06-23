using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeUI : MonoBehaviour
{
	private CanvasGroup canvas;
	private bool fadeOut = false;
	// Start is called be
	void Start()
	{
		canvas = GetComponent<CanvasGroup>();
		canvas.alpha = 0;
	}
	// Update is called once per frame
	void Update()
    {
		if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
		{
			//StopAllCoroutines();
			FadeIn();
			fadeOut = true;
		}
		else if (fadeOut)
		{
			StartCoroutine(WaitToFade());
			fadeOut = false;
		}

	}

	IEnumerator WaitToFade()
	{
		yield return new WaitForSeconds(4.0f);
		FadeOut();
	}
	
	private void FadeOut()
	{
		canvas.interactable = false;
		StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 0));
	}

	private void FadeIn()
	{
		canvas.interactable = true;
		StartCoroutine(FadeCanvasGroup(canvas, canvas.alpha, 1));
	}

	IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
	{
		float _timeStartedLerping = Time.time;
		float timeSinceStarted = Time.time - _timeStartedLerping;
		float percentageComplete = timeSinceStarted / lerpTime;

		while(true)
		{
			timeSinceStarted = Time.time - _timeStartedLerping;
			percentageComplete = timeSinceStarted / lerpTime;

			float currentValue = Mathf.Lerp(start, end, percentageComplete);

			cg.alpha = currentValue;

			if (percentageComplete >= 1)
			{
				cg.alpha = end;
				break;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public void StartLevel()
	{
        if (PlayerPrefs.HasKey("ComicSeen"))
        {
            SceneManager.LoadScene("HomeMenu");
        }
        else
        {
            PlayerPrefs.SetInt("ComicSeen", 1);
            SceneManager.LoadScene("World1Level1");
        }
	}
}
