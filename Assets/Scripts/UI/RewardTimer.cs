using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardTimer : MonoBehaviour
{
    public Action OnTimerComplete;
    public double offlineTimeMinutes
    {
        get
        {
            return (DateTime.Now - PersistantTimer.LoadTimeStamp()).TotalMinutes;
        }
    }
    public double offlineTimeHours
    {
        get
        {
            return (DateTime.Now - PersistantTimer.LoadTimeStamp()).TotalHours;
        }
    }

    [SerializeField] private TimerType _timerType = TimerType.Hours;
    [SerializeField] private int _defaultTimerTimeHours = 0;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timeHeaderText;

    private int _timerUpdateTime = 1;
    private bool _running;
    private TimeSpan _currentTimer = TimeSpan.MinValue;
    private WaitForSeconds _timerDelay = null;
    private string _timeHeaderString = string.Empty;
    private enum TimerType { Hours, Minutes, Seconds}

    public void RewardTimerSetup()
    {
        _timeHeaderString = _timeHeaderText.text;
        _timerDelay = new WaitForSeconds(_timerUpdateTime);
        _currentTimer = new TimeSpan(_defaultTimerTimeHours, 0, 0);

        CalculateNewTime();
    }

    public void StartRewardTimer(int hours, int minutes, int seconds)
    {
        _timeHeaderText.text = _timeHeaderString;
        _currentTimer = new TimeSpan(hours, minutes, seconds);
        StartCoroutine(TimerUpdateRoutine());
    }

    public void DisableRewardTimer()
    {
        _currentTimer = new TimeSpan(0, 0, 0);
        _timerText.text = "";
        _timeHeaderText.text = "";
        _running = false;
        StopAllCoroutines();
    }

    public void ResetRewardTimer(bool continueTimer = true)
    {
        if(continueTimer)
        {
            _timeHeaderText.text = _timeHeaderString;
            _currentTimer = new TimeSpan(_defaultTimerTimeHours, 0, 0);
            StartCoroutine(TimerUpdateRoutine());
        }
        else
        {
            _currentTimer = new TimeSpan(0, 0, 0);
            _timerText.text = "";
            _timeHeaderText.text = "";
            _running = false;
            StopAllCoroutines();
        }
    }

    private void UpdateTimer(TimeSpan subtractedTime = default(TimeSpan))
    {
        if (subtractedTime != default(TimeSpan))
            _currentTimer = _currentTimer.Subtract(subtractedTime);

        switch (_timerType)
        {
            case TimerType.Hours:
                if(_currentTimer.Seconds < 10)
                {
                    _timerText.text = _currentTimer.Hours + ":" + _currentTimer.Minutes + ":0" + _currentTimer.Seconds;
                }
                else
                {
                    _timerText.text = _currentTimer.Hours + ":" + _currentTimer.Minutes + ":" + _currentTimer.Seconds;
                }
                break;
            case TimerType.Minutes:
                if (_currentTimer.Seconds < 10)
                {
                    _timerText.text = _currentTimer.Minutes + ":0" + _currentTimer.Seconds;
                }
                else
                {
                    _timerText.text = _currentTimer.Minutes + ":" + _currentTimer.Seconds;
                }
                break;
            case TimerType.Seconds:
                _timerText.text = _currentTimer.Seconds.ToString();
                break;
            default:
                throw new NotImplementedException("Timer Type not implemented");
        }
    }

    private void CalculateNewTime()
    {
        PersistantTimer.LoadTime();

        DateTime loadedTime = PersistantTimer.LoadedTime;
        if(loadedTime != DateTime.MinValue)
        {
            TimeSpan timeOffset = DateTime.Now - loadedTime;
            _currentTimer = _currentTimer.Subtract(timeOffset);
        }
    }

    private IEnumerator TimerUpdateRoutine()
    {
        if(_running)
        {
            yield break;
        }

        _running = true;
        while(_currentTimer.TotalSeconds > 0)
        {
            yield return _timerDelay;
            UpdateTimer(new TimeSpan(0, 0, _timerUpdateTime));
        }
        _running = false;

        if(OnTimerComplete != null)
        {
            OnTimerComplete.Invoke();
        }
    }

    #region SaveLoadMethods
    private void OnApplicationQuit()
    {
        PersistantTimer.SaveTime(new TimeSpan(_defaultTimerTimeHours, 0, 0) - _currentTimer);
        PlayerPrefs.SetInt("RewardQuitHours", _currentTimer.Hours);
        PlayerPrefs.SetInt("RewardQuitMinutes", _currentTimer.Minutes);
        PlayerPrefs.SetInt("RewardQuitSeconds", _currentTimer.Seconds);
    }

    private void OnDestroy()
    {
        PersistantTimer.SaveTime(new TimeSpan(_defaultTimerTimeHours, 0, 0) - _currentTimer);
        PlayerPrefs.SetInt("RewardQuitHours", _currentTimer.Hours);
        PlayerPrefs.SetInt("RewardQuitMinutes", _currentTimer.Minutes);
        PlayerPrefs.SetInt("RewardQuitSeconds", _currentTimer.Seconds);
    }
    #endregion
}