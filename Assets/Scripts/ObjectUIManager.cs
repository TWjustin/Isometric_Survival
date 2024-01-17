using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIManager : MonoBehaviour
{
    public static ObjectUIManager Instance { get; set; }
    
    [SerializeField] private GameObject windowCanvasPrefab;
    [SerializeField] private GameObject itemAddedPopupPrefab;


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


    public void SpawnCanvas(Player player)
    {
        GameObject windowCanvas = Instantiate(windowCanvasPrefab, player.transform, false);
        TimedEventCanvas canvasScript = windowCanvas.GetComponent<TimedEventCanvas>();
        canvasScript.resource = player.actingObject;
        canvasScript.Initialize();
    }
    
    public IEnumerator SpawnPopup(Player player)
    {
        List<DropItem> actionReward = player.actionReward;
        
        for (int i = 0; i < actionReward.Count; i++)
        {
            GameObject popup = Instantiate(itemAddedPopupPrefab, player.transform, false);
            popup.GetComponentInChildren<Text>().text = "+" + actionReward[i].amount;
            popup.GetComponentInChildren<Image>().sprite = actionReward[i].item.itemImage;
            
            yield return new WaitForSeconds(0.5f);
        }
    }
}
