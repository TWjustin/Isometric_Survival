using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public int durability;  // 耐久度
    
    public void Awake()
    {
        type = ItemType.Tool;
    }
}
