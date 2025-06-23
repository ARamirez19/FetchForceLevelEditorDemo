using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    [SerializeField] private double hoursToNewReward = 8;
    private double minutesToNewReward;
    [SerializeField] private RewardTimer timerReward;

    void Awake()
    {
        timerReward.RewardTimerSetup();
        minutesToNewReward = hoursToNewReward * 60;
        var timeRemaining = (timerReward.offlineTimeMinutes / minutesToNewReward) - (Math.Floor(timerReward.offlineTimeMinutes / minutesToNewReward));
        var duration = Math.Floor(timerReward.offlineTimeMinutes / minutesToNewReward);

        long hourPart = (long)PlayerPrefs.GetInt("RewardQuitHours", 8) - (long)timerReward.offlineTimeHours;
        long intPart = (long)PlayerPrefs.GetInt("RewardQuitMinutes", 5) - (long)timerReward.offlineTimeMinutes;
        long fractionalPart = PlayerPrefs.GetInt("RewardQuitSeconds", 5) - (long)((timerReward.offlineTimeMinutes - (long)timerReward.offlineTimeMinutes) * 60);

        if (timeRemaining < 0 || timeRemaining >= minutesToNewReward)
        {
            timerReward.StartRewardTimer((int)hoursToNewReward, 0, 0);
        }
        else
        {
            if (intPart < 0 || fractionalPart <= 0)
            {
                timerReward.StartRewardTimer((int)hoursToNewReward, 0, 0);
            }
            else
            {
                timerReward.StartRewardTimer((int)hourPart, (int)intPart, (int)fractionalPart);
            }
        }
    }

    private void OnApplicationQuit()
    {
        
    }
}
