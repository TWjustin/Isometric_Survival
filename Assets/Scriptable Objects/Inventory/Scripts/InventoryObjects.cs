using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    public bool isFull;
    
    public void AddItem(DisplayInventory inventoryPanel, ItemObject _item, int _amount)
    {
        int maxContainerSize = inventoryPanel.transform.childCount;
        
        InventorySlot existingSlot = container.Find(slotInfo => slotInfo.item == _item);
        
        
        if (existingSlot != null)
        {
            existingSlot.AddAmount(_amount);
            
            isFull = false;
            
        }
        else if (container.Count < maxContainerSize)
        {
            InventorySlot emptySlot = new InventorySlot();

            emptySlot.item = _item;
            emptySlot.AddAmount(_amount);
            container.Add(emptySlot);
            
            isFull = false;
            
        }
        else
        {
            Debug.Log("Inventory is full");
            isFull = true;
        }
        
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