using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedEventManager : MonoBehaviour // 管理UI
{
    public static TimedEventManager Instance { get; set; }
    
    [HideInInspector] public TimedEventResource currentTimedEvent;
    
    [Header("UI")]
    public GameObject window;
    public Text timeLeftText;
    public Slider timeLeftSlider;
    public Button skipButton;
    
    // [SerializeField] private Button testButton;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        window.SetActive(false);

        // testButton.onClick.AddListener(StartTimer);
        skipButton.onClick.AddListener(Skip);
    }
    
    
    public void OpenWindowAndSetPosition(Player player)
    {
        window.SetActive(true);
        
        Vector3 windowPosition = player.transform.position;
        windowPosition.y += 1.75f;
        window.transform.position = windowPosition;
        
        currentTimedEvent.InitializeWindow();
        
    }
    
    public void CloseWindow()
    {
        window.SetActive(false);
    }
    
    

    private void Skip()
    {
        currentTimedEvent.TimerEnd = DateTime.Now;
        currentTimedEvent.inProgress = false;
        StopCoroutine(currentTimedEvent.lastTimer);
        
        timeLeftText.text = "Finished";
        timeLeftSlider.value = 1;
        
        StopCoroutine(currentTimedEvent.lastDisplay);
        skipButton.gameObject.SetActive(false);
        // testButton.gameObject.SetActive(true);
    }
    
}
