using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistantTimer
{
    private static DateTime loadedTime = new DateTime();
    public static DateTime LoadedTime { get { return loadedTime; } }

    public static bool CompareTime(int minutesToReach)
    {
        TimeSpan timeDifference = DateTime.Now - LoadedTime;
        if (timeDifference.TotalMinutes >= minutesToReach)
        {
            return true;
        }
        return false;
    }

    public static void SaveTime(TimeSpan _timerTime)
    {
        DateTime timeToSave = DateTime.Now - _timerTime;

        SaveManager.Instance.SaveTime(timeToSave);
    }

    public static void LoadTime()
    {
        loadedTime = SaveManager.Instance.LoadTime();
    }

    public static DateTime LoadTimeStamp()
    {
        return SaveManager.Instance.LoadTimeStamp();
    }
}
