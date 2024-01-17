using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEventCanvas : MonoBehaviour
{
    private Player player;
    [HideInInspector] public TimedEventResource resource;
    
    

    // 時間
    private DateTime startTime;
    private DateTime endTime;
    private TimeSpan totalTime;
    private float totalSeconds;
    
    private Coroutine timerCoroutine;
    
    // UI
    [SerializeField] private Text timeLeftText;
    [SerializeField] private Slider timeLeftSlider;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button doneButton;

    
    
    public void Initialize()
    {
        player = transform.parent.GetComponent<Player>();


        cancelButton.onClick.AddListener(Cancel);
        doneButton.onClick.AddListener(DoneButton);
        skipButton.onClick.AddListener(Skip);
        cancelButton.gameObject.SetActive(true);
        doneButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        

        startTime = DateTime.Now;
        totalTime = new TimeSpan(resource.hours, resource.minutes, resource.seconds);
        endTime = startTime.Add(totalTime);
        totalSeconds = (float) totalTime.TotalSeconds;
        
        
        timerCoroutine = StartCoroutine(Timer());   // coroutine
    }
    
    private IEnumerator Timer()
    {
        TimeSpan timeLeft = endTime - DateTime.Now;
        float totalSecondsLeft = (float) timeLeft.TotalSeconds;
        
        string text;
        
        while (true)    // 因為是coroutine，所以可以這樣寫
        {
            text = "";
            timeLeftSlider.value = 1- (totalSecondsLeft / totalSeconds);
            
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
                
                HandleTimesUpUI();
                break;
            }
        }
        
        yield return null;
    }

    
    
    private void Cancel()
    {
        StopCoroutine(timerCoroutine);
        
        player.inProgress = false;
        InputHandler.Instance.currentPlayer = null;
        
        Destroy(gameObject);
        
    }


    private void Skip()
    {
        StopCoroutine(timerCoroutine);
        
        HandleTimesUpUI();
        
    }
    
    
    private void DoneButton()
    {

        player.GetReward();
        
        StartCoroutine(ObjectUIManager.Instance.SpawnPopup(player));
        
        
        Destroy(gameObject);
    }

    
    

    private void HandleTimesUpUI()
    {
        timeLeftText.text = "Finished";
        timeLeftSlider.value = 1;
        cancelButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        doneButton.gameObject.SetActive(true);
    }
}
