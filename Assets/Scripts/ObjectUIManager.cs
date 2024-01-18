using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIManager : MonoBehaviour
{
    public static ObjectUIManager Instance { get; set; }
    
    
    [SerializeField] private float waitTimeBase = 0.5f;
    
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
    
    
    public void SpawnPopupInARow(Player player, List<DropItem> actionReward)
    {

        for (int i = 0; i < actionReward.Count; i++)
        {
            StartCoroutine(SpawnPopup(player.transform, actionReward[i], i * waitTimeBase));
        }
        
    }

    private IEnumerator SpawnPopup(Transform player, DropItem dropItem, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        GameObject popup = Instantiate(itemAddedPopupPrefab, player, false);
        popup.GetComponentInChildren<Text>().text = "+" + dropItem.amount;
        popup.GetComponentInChildren<Image>().sprite = dropItem.item.itemImage;
    }
    
    
}
