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

    public ItemType type;
    [TextArea(15, 20)]
    public string description;
}
