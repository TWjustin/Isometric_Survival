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
    
    
    // timed event
    public GameObject windowCanvasPrefab;
    private GameObject windowCanvas;
    
    
    [Header("Do not Edit")] 
    public Player actingPlayer;
    
    
    public void InstantiateCanvas()
    {
        
        Vector3 canvasPosition = transform.position;
        canvasPosition.y += 1.5f;
        windowCanvas = Instantiate(windowCanvasPrefab, transform, false);

        windowCanvas.GetComponent<TimedEventCanvas>().Initialize();

        actingPlayer.inProgress = true;
    }
    

    public void DoneOrHarvest()
    {
        actingPlayer.inProgress = false;
        
        actingPlayer.inventory.AddItemToInventory(dropItemResult, dropAmount);
        actingPlayer.inventoryPanel.UpdateDisplay(actingPlayer);
        Destroy(windowCanvas);
    }
    
    
}
