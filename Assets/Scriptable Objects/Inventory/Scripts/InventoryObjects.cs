using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();
    public bool changed;

    public void AddItem(ItemObject _item, int _amount)
    {
        // bool hasItem = false;
        // for (int i = 0; i < container.Count; i++)
        // {
        //     if (container[i].item == _item)
        //     {
        //         container[i].AddAmount(_amount);
        //         hasItem = true;
        //         break;
        //     }
        // }
        // if (!hasItem)
        // {
        //     foreach (var slot in container)
        //     {
        //         if (slot.item == null)
        //         {
        //             slot.item = _item;
        //             slot.AddAmount(_amount);
        //             break;
        //         }
        //     }
        // }
        InventorySlot existingSlot = container.Find(slot => slot.item == _item);
        
        if (existingSlot != null)
        {
            existingSlot.AddAmount(_amount);
        }
        else
        {
            InventorySlot emptySlot = new InventorySlot();

            emptySlot.item = _item;
            emptySlot.AddAmount(_amount);
            container.Add(emptySlot);
        }
        changed = true;
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public void AddAmount(int value)
    {
        amount += value;
    }
}