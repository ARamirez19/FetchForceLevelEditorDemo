using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{

    private static SaveManager _instance = null;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }
            return _instance;
        }
    }

    public void SaveStars(string level, int stars)
    {
        PlayerPrefs.SetInt(level + "stars", stars);
        PlayerPrefs.Save();
        //CloudVariables.lives = PlayerPrefs.GetInt("Lives");
        //CloudVariables.stars = PlayerPrefs.GetInt(prefix + level.ToString(), stars);
        //CloudVariables.testingInt = 25;
        //CloudSave.Instance.SaveData();
        //CloudSave.Instance.ReadData();
        //CloudSave.Instance.Synchronise();
    }

    public void SaveLives(int lives)
    {
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }

    public void SaveTime(DateTime timeToSave)
    {
        PlayerPrefs.SetInt("timeStampTimerYear", DateTime.Now.Year);
        PlayerPrefs.SetInt("timeStampTimerMonth", DateTime.Now.Month);
        PlayerPrefs.SetInt("timeStampTimerDay", DateTime.Now.Day);
        PlayerPrefs.SetInt("timeStampTimerHour", DateTime.Now.Hour);
        PlayerPrefs.SetInt("timeStampTimerMinutes", DateTime.Now.Minute);
        PlayerPrefs.SetInt("timeStampTimerSeconds", DateTime.Now.Second);

        PlayerPrefs.SetInt("timerYear", timeToSave.Year);
        PlayerPrefs.SetInt("timerMonth", timeToSave.Month);
        PlayerPrefs.SetInt("timerDay", timeToSave.Day);
        PlayerPrefs.SetInt("timerHour", timeToSave.Hour);
        PlayerPrefs.SetInt("timerMinutes", timeToSave.Minute);
        PlayerPrefs.SetInt("timerSeconds", timeToSave.Second);
        PlayerPrefs.Save();
    }

    public int LoadStars(int world, int level)
    {       
        return PlayerPrefs.GetInt("World" + world + "Level" + level + "stars", 0);
    }

    public int LoadLives()
    {
        return PlayerPrefs.GetInt("Lives", 15);
    }

    public DateTime LoadTime()
    {
        int year = PlayerPrefs.GetInt("timerYear", 1);
        int month = PlayerPrefs.GetInt("timerMonth", 1);
        int day = PlayerPrefs.GetInt("timerDay", 1);
        int hour = PlayerPrefs.GetInt("timerHour", 0);
        int minutes = PlayerPrefs.GetInt("timerMinutes", 0);
        int seconds = PlayerPrefs.GetInt("timerSeconds", 0);

        return new DateTime(year, month, day, hour, minutes, seconds);
    }

    public DateTime LoadTimeStamp()
    {
        int year = PlayerPrefs.GetInt("timeStampTimerYear", 1);
        int month = PlayerPrefs.GetInt("timeStampTimerMonth", 1);
        int day = PlayerPrefs.GetInt("timeStampTimerDay", 1);
        int hour = PlayerPrefs.GetInt("timeStampTimerHour", 0);
        int minutes = PlayerPrefs.GetInt("timeStampTimerMinutes", 0);
        int seconds = PlayerPrefs.GetInt("timeStampTimerSeconds", 0);

        return new DateTime(year, month, day, hour, minutes, seconds);
    }

    public void QuitSecondsSave(int time)
    {
        PlayerPrefs.SetInt("QuitSeconds", time);
    }

    public void QuitMinutesSave(int time)
    {
        PlayerPrefs.SetInt("QuitMinutes", time);
    }

    public void QuitHoursSave(int time)
    {
        PlayerPrefs.SetInt("QuitHours", time);
    }
    
    public int QuitSecondsLoad()
    {
        return PlayerPrefs.GetInt("QuitSeconds", 5);
    }

    public int QuitMinutesLoad()
    {
        return PlayerPrefs.GetInt("QuitMinutes", 5);
    }

    public int QuitHoursLoad()
    {
        return PlayerPrefs.GetInt("QuitHours", 8);
    }

#if UNITY_EDITOR
    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
        //CloudSave.Instance.ReadData();
        //CloudSave.Instance.Synchronise();
    }
#endif
}


