using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEvent : MonoBehaviour
{
    private bool inProgress;
    private DateTime TimerStart;
    private DateTime TimerEnd;

    private Coroutine lastTimer;
    private Coroutine lastDisplay;

    [Header("Production Time")] 
    public int hours;
    public int minutes;
    public int seconds;
    
    [Header("UI")]
    [SerializeField] private GameObject window;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private Slider timeLeftSlider;
    [SerializeField] private Button skipButton;
    
    [SerializeField] private Button testButton;

    
    #region Unity Methods

    private void Start()
    {
        testButton.onClick.AddListener(StartTimer);
        skipButton.onClick.AddListener(Skip);
    }

    #endregion

    
    #region UI Methods

    private void InitializeWindow()
    {
        if (inProgress)
        {
            lastDisplay = StartCoroutine(DisplayTime());
        
            testButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(true);
        }
        
    }

    private IEnumerator DisplayTime()
    {
        DateTime start = DateTime.Now;
        TimeSpan timeLeft = TimerEnd - start;
        double totalSecondsLeft = timeLeft.TotalSeconds;
        double totalSeconds = (TimerEnd - TimerStart).TotalSeconds;
        string text;

        while (window.activeSelf)
        {
            text = "";
            timeLeftSlider.value = 1 - Convert.ToSingle((TimerEnd - DateTime.Now).TotalSeconds / totalSeconds);

            if (totalSecondsLeft > 1)
            {
                if (timeLeft.Hours > 0)
                {
                    text += timeLeft.Hours + "h ";
                    text += timeLeft.Minutes + "m ";
                    yield return new WaitForSeconds(timeLeft.Seconds);
                }
                else if (timeLeft.Minutes > 0)
                {
                    TimeSpan ts = TimeSpan.FromSeconds(totalSecondsLeft);
                    text += ts.Minutes + "m ";
                    text += ts.Seconds + "s ";
                }
                else
                {
                    text += Mathf.FloorToInt((float) totalSecondsLeft) + "s ";
                }
                
                timeLeftText.text = text;

                totalSecondsLeft -= Time.deltaTime;
                yield return null;
            }
            else
            {
                timeLeftText.text = "Finished";
                skipButton.gameObject.SetActive(false);
                timeLeftSlider.value = 1;
                inProgress = false;
                break;
            }
        }

        yield return null;
    }
    
    public void OpenWindow()
    {
        window.SetActive(true);
        InitializeWindow();
    }
    
    public void CloseWindow()
    {
        window.SetActive(false);
    }

    #endregion

    #region Timed Event

    private void StartTimer()
    {
        TimerStart = DateTime.Now;
        TimeSpan time = new TimeSpan(hours, minutes, seconds);
        TimerEnd = TimerStart.Add(time);
        inProgress = true;
        
        lastTimer = StartCoroutine(Timer());
        
        InitializeWindow();
    }

    private IEnumerator Timer()
    {
        DateTime start = DateTime.Now;
        double secondsToFinished = (TimerEnd - start).TotalSeconds;
        yield return new WaitForSeconds(Convert.ToSingle(secondsToFinished));
        
        inProgress = false;
        Debug.Log("Finished");
    }

    private void Skip()
    {
        TimerEnd = DateTime.Now;
        inProgress = false;
        StopCoroutine(lastTimer);
        
        timeLeftText.text = "Finished";
        timeLeftSlider.value = 1;
        
        StopCoroutine(lastDisplay);
        skipButton.gameObject.SetActive(false);
        testButton.gameObject.SetActive(true);
    }

    #endregion
}
