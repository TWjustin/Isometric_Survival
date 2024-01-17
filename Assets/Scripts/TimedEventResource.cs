using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventResource : MonoBehaviour
{
    // 掉落的物品及數量
    public ItemObject dropItemResult;   //
    public int dropAmount;
    
    
    // 所需要的時間
    public int hours;
    public int minutes;
    public int seconds;
    
    
    public GameObject windowCanvasPrefab;
    
    
    [Header("Do not Edit")] 
    public Player actingPlayer;
    
    
    public void InstantiateCanvas()
    {
        
        Vector3 canvasPosition = transform.position;
        canvasPosition.y += 1.5f;
        Instantiate(windowCanvasPrefab, transform, false);
        
    }
    
    
    
}
