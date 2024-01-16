using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEventCanvas : MonoBehaviour
{
    private TimedEventResource resource;
    [SerializeField] private InventoryPanel inventoryPanel;

    // 時間
    private DateTime startTime;
    private DateTime endTime;
    private TimeSpan totalTime;
    private float totalSeconds;
    
    private Coroutine timerCoroutine;
    
    // UI
    public Text timeLeftText;
    public Slider timeLeftSlider;
    public Button skipButton;
    public Button harvestButton;

    
    public void Initialize()
    {
        resource = transform.parent.GetComponent<TimedEventResource>();
        
        harvestButton.onClick.AddListener(resource.DoneOrHarvest);
        skipButton.onClick.AddListener(Skip);
        harvestButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);
        

        startTime = DateTime.Now;
        totalTime = new TimeSpan(resource.hours, resource.minutes, resource.seconds);
        endTime = startTime.Add(totalTime);
        totalSeconds = (float) totalTime.TotalSeconds;
        
        
        timerCoroutine = StartCoroutine(Timer());
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
                
                EndAction();
                break;
            }
        }
        
        yield return null;
    }


    private void Skip()
    {
        StopCoroutine(timerCoroutine);
        
        EndAction();
        
        resource.actingPlayer.inventory.AddItemToInventory(resource.dropItemResult, resource.dropAmount);
        if (resource.actingPlayer == InputHandler.Instance.currentPlayer)
        {
            inventoryPanel.UpdateDisplay(resource.actingPlayer);
        }
    }

    private void EndAction()
    {
        timeLeftText.text = "Finished";
        timeLeftSlider.value = 1;
        
        skipButton.gameObject.SetActive(false);
        harvestButton.gameObject.SetActive(true);
    }
}
