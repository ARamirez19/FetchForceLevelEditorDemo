using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    [SerializeField] private int maxLives = 15;
    [SerializeField] private double minutesToNewLife = 5;
    [SerializeField] private UITimer livesTimer;

    private int lives;
    
    public TextMeshProUGUI livesText;

    void Awake()
    {
		livesTimer.OnTimerComplete += AddLife;
		lives = SaveManager.Instance.LoadLives(); //Come back to edit for load lives
        SaveManager.Instance.SaveLives(lives); 
		if (lives < maxLives)
		{
			livesTimer.SetUpTimer();

			var timeRemaining = (livesTimer.OfflineTimeMinutes / minutesToNewLife) - (Math.Floor(livesTimer.OfflineTimeMinutes / minutesToNewLife));
			var duration = Math.Floor(livesTimer.OfflineTimeMinutes / minutesToNewLife);

			if (duration < int.MaxValue)
				lives += (int)duration;
			else
				lives = maxLives;

			if (lives > maxLives)
				lives = maxLives;

            SaveManager.Instance.SaveLives(lives);
			UpdateLivesText();

			if (lives < maxLives)
			{
				long intPart = (long)SaveManager.Instance.QuitMinutesLoad() - (long)livesTimer.OfflineTimeMinutes;
                Debug.Log(intPart);
				long fractionalPart = SaveManager.Instance.QuitSecondsLoad() - (long)((livesTimer.OfflineTimeMinutes - (long)livesTimer.OfflineTimeMinutes) * 60);
				if (timeRemaining < 0 || timeRemaining >= 5)
					livesTimer.StartTimer((int)minutesToNewLife, 0);
				else
				{
					if(intPart <= 0 || fractionalPart <= 0)
						livesTimer.StartTimer((int)minutesToNewLife, 0);
					else
						livesTimer.StartTimer((int)intPart, (int)(fractionalPart));
				}
			}
			else
			{
				livesTimer.DisableTimer();
			}
		}
		else
		{
			livesTimer.DisableTimer();
		}
    }

    private void AddLife()
    {
        if (lives < maxLives)
        {
            lives++;
            SaveManager.Instance.SaveLives(lives);
            UpdateLivesText();
            livesTimer.ResetTimer();
			if (lives == maxLives)
				livesTimer.DisableTimer();
        }
        else
        {
			//livesTimer.ResetTimer();
			livesTimer.DisableTimer();
        }

        livesText.text = lives.ToString();
    }

    public void RemoveLife()
    { 
        if (lives > 0)
        {
            lives--;
            SaveManager.Instance.SaveLives(lives);
            UpdateLivesText();
            livesText.text = lives.ToString();
        }
    }

    private void Update()
    {
		//if (Input.GetKeyDown(KeyCode.A))
		//	AddLife();
		//else if (Input.GetKeyDown(KeyCode.S))
		//	RemoveLife();
    }

    //private void Update()
    //{
    //if(lives < maxLives)
    //{
    //currentTime = DateTime.Now;
    //TimeSpan difference = currentTime.Subtract(prevTime);
    //if(difference > TimeSpan.FromMinutes(minutesToNewLife))
    //{
    //    if (lives < maxLives)
    //    {
    //        lives++;
    //        PlayerPrefs.SetInt("Lives", lives);
    //        UpdateLives();
    //    }
    //    prevTime = DateTime.Now;
    //}
    //}

    //if(PlayerPrefs.HasKey("Lives"))
    //{
    //     if (PlayerPrefs.GetInt("Lives") != lives)
    //     {   
    //         lives = PlayerPrefs.GetInt("Lives");
    //         prevTime = DateTime.Now;
    //         UpdateLives();
    //     }
    //}
    //}

    public void UpdateLivesText()
    {
        livesText.text = SaveManager.Instance.LoadLives().ToString();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("QuitTime", DateTime.Now.ToBinary().ToString());
    }

    public void HomeRefillLives()
    {
		//PlayerPrefs.SetInt("Lives", maxLives);
		//lives = maxLives;
		//AddLife();
		//IronSource.Agent.showRewardedVideo();
	}
}
