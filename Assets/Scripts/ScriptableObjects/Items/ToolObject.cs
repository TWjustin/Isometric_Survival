using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ToolType
{
    Axe,
    Pickaxe,
    Hoe,
    Scythe,
    Sword,
    WateringCan,
    Default
}

[CreateAssetMenu(fileName = "New Tool Object", menuName = "Inventory System/Items/Tool")]
public class ToolObject : ItemObject
{
    public ToolType toolType;
    public int strength;  // 強度
    public int durability;  // 耐久度，使用一次減一
    
    public void Awake()
    {
        type = ItemType.Tool;
        toolType = ToolType.Default;
    }
}
