using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventResource : MonoBehaviour
{
    // 掉落的物品及數量
    public ItemObject dropItemResult;   //
    public int dropAmount;
    
    
    // 所需要的時間   // private
    public int hours;
    public int minutes;
    public int seconds;
    
    [HideInInspector] public bool inProgress;
    private DateTime TimerStart;
    public DateTime TimerEnd;

    public Coroutine lastTimer;
    public Coroutine lastDisplay;
    
    
    public void InitializeWindow()
    {

        // if (inProgress)
        
        lastDisplay = StartCoroutine(DisplayTime());
        
        // testButton.gameObject.SetActive(false);
        TimedEventManager.Instance.skipButton.gameObject.SetActive(true);
    }
    
    public void StartTimer()
    {
        TimerStart = DateTime.Now;
        TimeSpan time = new TimeSpan(hours, minutes, seconds);
        TimerEnd = TimerStart.Add(time);
        inProgress = true;
        
        lastTimer = StartCoroutine(Timer());

        if (inProgress)
        {
            InitializeWindow();
        }
        
    }

    private IEnumerator Timer()
    {
        DateTime start = DateTime.Now;
        double secondsToFinished = (TimerEnd - start).TotalSeconds;
        yield return new WaitForSeconds(Convert.ToSingle(secondsToFinished));
        
        // 加入inventory
        inProgress = false;
        Debug.Log("Finished");
    }
    
    private IEnumerator DisplayTime()
    {
        DateTime start = DateTime.Now;
        TimeSpan timeLeft = TimerEnd - start;
        double totalSecondsLeft = timeLeft.TotalSeconds;
        double totalSeconds = (TimerEnd - TimerStart).TotalSeconds;
        string text;

        while (TimedEventManager.Instance.window.activeSelf)
        {
            text = "";
            TimedEventManager.Instance.timeLeftSlider.value = 1 - Convert.ToSingle((TimerEnd - DateTime.Now).TotalSeconds / totalSeconds);

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
                
                TimedEventManager.Instance.timeLeftText.text = text;

                totalSecondsLeft -= Time.deltaTime;
                yield return null;
            }
            else
            {
                TimedEventManager.Instance.timeLeftText.text = "Finished";
                TimedEventManager.Instance.skipButton.gameObject.SetActive(false);
                TimedEventManager.Instance.timeLeftSlider.value = 1;
                inProgress = false;
                break;
            }
        }

        yield return null;
    }

    
}
