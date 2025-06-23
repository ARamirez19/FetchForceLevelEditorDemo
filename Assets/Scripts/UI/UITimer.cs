using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITimer : MonoBehaviour
{
    public Action OnTimerComplete;
    public double OfflineTimeMinutes
    {
        get
        {
            return (DateTime.Now - PersistantTimer.LoadTimeStamp()).TotalMinutes;
        }
    }

    [SerializeField] private TimerType _timerType = TimerType.Hours;
    [SerializeField] private int _defaultTimerTimeMinutes = 0;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timeHeaderText;

    private int _timerUpdateTime = 1;
    private bool _running;
    private TimeSpan _currentTimer = TimeSpan.MinValue;
    private WaitForSeconds _timerDelay = null;
    private string _timeHeaderString = string.Empty;

    private enum TimerType { Hours, Minutes, Seconds }

    public void SetUpTimer()
    {
        _timeHeaderString = _timeHeaderText.text;
        _timerDelay = new WaitForSeconds(_timerUpdateTime);
        _currentTimer = new TimeSpan(0, _defaultTimerTimeMinutes, 0);

        CalculateNewTime();
    }

    public void StartTimer(int minutes, int seconds)
    {
        _timeHeaderText.text = _timeHeaderString;
        //CalculateNewTime();
        _currentTimer = new TimeSpan(0, minutes, seconds);
        StartCoroutine(TimerUpdateRoutine());
    }

    public void DisableTimer()
    {
        _currentTimer = new TimeSpan(0, 0, 0);
        _timerText.text = "";
        _timeHeaderText.text = "";
        _running = false;
        StopAllCoroutines();
    }

    [ContextMenu("ResetTimer")]
    public void ResetTimer(bool continueTimer = true)
    {
        if (continueTimer)
        {
            _timeHeaderText.text = _timeHeaderString;
            _currentTimer = new TimeSpan(0, _defaultTimerTimeMinutes, 0);
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
                _timerText.text = _currentTimer.Hours + ":" + _currentTimer.Minutes + ":" + _currentTimer.Seconds;
                break;
            case TimerType.Minutes:
				if (_currentTimer.Seconds < 10)
					_timerText.text = _currentTimer.Minutes + ":0" + _currentTimer.Seconds;
				else
					_timerText.text = _currentTimer.Minutes + ":" + _currentTimer.Seconds;
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
        if (loadedTime != DateTime.MinValue)
        {
            TimeSpan timeOffset = DateTime.Now - loadedTime;
            _currentTimer = _currentTimer.Subtract(timeOffset);
        }
    }

    private IEnumerator TimerUpdateRoutine()
    {
        if (_running)
            yield break;

        _running = true;
        while (_currentTimer.TotalSeconds > 0)
        {
            yield return _timerDelay;
            UpdateTimer(new TimeSpan(0, 0, _timerUpdateTime));
        }
        _running = false;

        if (OnTimerComplete != null)
            OnTimerComplete.Invoke();
    }

    #region SaveLoadMethods
    private void OnApplicationQuit()
    {
        PersistantTimer.SaveTime(new TimeSpan(0, _defaultTimerTimeMinutes, 0) - _currentTimer);
        SaveManager.Instance.QuitMinutesSave(_currentTimer.Minutes);
        SaveManager.Instance.QuitSecondsSave(_currentTimer.Seconds);
    }

    private void OnDestroy()
    {
        PersistantTimer.SaveTime(new TimeSpan(0, _defaultTimerTimeMinutes, 0) - _currentTimer);
        SaveManager.Instance.QuitMinutesSave(_currentTimer.Minutes);
        SaveManager.Instance.QuitSecondsSave(_currentTimer.Seconds);
    }
    #endregion
}
