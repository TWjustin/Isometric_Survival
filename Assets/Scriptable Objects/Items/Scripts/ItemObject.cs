using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Food,
    Tool,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    
    [HideInInspector] public ItemType type; // 考慮去掉
    [TextArea(15, 20)]
    public string description;
}


[System.Serializable]   // 可能要搬
public class DropItemData
{
    public ItemObject item;
    public int minAmount;
    public int maxAmount;
    
}

public class DropItem
{
    public ItemObject item;
    public int amount;
    
    public DropItem(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
