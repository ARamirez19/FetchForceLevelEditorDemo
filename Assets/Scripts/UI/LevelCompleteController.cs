using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelCompleteController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTakenText = null;
    [SerializeField] private TextMeshProUGUI recommendedTimeText = null;
    [SerializeField] private TextMeshProUGUI freezedUsedText = null;
    [SerializeField] private TextMeshProUGUI collectedCarrots = null;
    [SerializeField] private TextMeshProUGUI extraCarrot = null;
    [SerializeField] private TextMeshProUGUI levelDisplay = null;
    [SerializeField] private Image[] stars = null;
    [SerializeField] private Sprite _starActive;
    [SerializeField] private Animator _animator;

    private int starIndex = 0;
    private int starsToAdd = 0;

    public void OnEnable()
    {
        //this.GetComponent<FadeEffect>().onFadeInComplete += AnimateStars;
        _animator.SetBool("Show", true);
    }

	public void CompleteLevel(int level, double timeTaken, double recommendedTime, int deaths)
	{
		Scene currScene = SceneManager.GetActiveScene();
		ResetData();
		timeTakenText.text = timeTaken.ToString();
		recommendedTimeText.text = recommendedTime.ToString();
		//freezedUsedText.text = timesFrozen.ToString();

		if (timeTaken < recommendedTime)
		{
			starsToAdd++;
		}

		starsToAdd++;

		if (LevelManager.GetInstance().extraCollectableEarned == true)
		{
			starsToAdd++;
		}

		collectedCarrots.SetText(LevelManager.GetInstance().CollectableAmount + "/" + LevelManager.GetInstance().CollectableAmount);
		if (LevelManager.GetInstance().extraCollectableEarned == true)
		{
			extraCarrot.SetText("1/1");
		}
		else
		{
			extraCarrot.SetText("0/1");
		}
		char[] levelName = SceneManager.GetActiveScene().name.ToCharArray();

		string levelNumber = SceneManager.GetActiveScene().name;

		levelNumber = levelNumber.Remove(0, 11); //Please redo this later lol

        //levelDisplay.SetText("Zone " + levelName[5] + " Level " + levelNumber);

        SaveManager.Instance.SaveStars(currScene.name, starsToAdd);
        ActivateStars();
    }

    public void ActivateStars()
    {
        for (int i = 0; i < starsToAdd; i++)
        {
            if (i < starsToAdd)
            {
                stars[i].sprite = _starActive;
            }
            else
            {
                break;
            }
        }
    }

    private void AnimateStars()
    {
        stars[0].gameObject.SetActive(true);
    }

    private void ResetData()
    {
        starIndex = 0;
        starsToAdd = 0;
        timeTakenText.text = "";
        recommendedTimeText.text = "";
        //freezedUsedText.text = "";
    }
}
