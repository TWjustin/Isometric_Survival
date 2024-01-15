using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    private int maxContainerSize;
    private InventorySlot existingSlot;
    
    
    public bool CheckSlotAvailable(DisplayInventory inventoryPanel, ItemObject _item)
    {
        maxContainerSize = inventoryPanel.transform.childCount;
        existingSlot = container.Find(slotInfo => slotInfo.item == _item);

        if (existingSlot != null)
        {
            return true;
        }
        else if (container.Count < maxContainerSize)
        {
            return true;
        }
        else
        {
            Debug.Log("Inventory is full");
            return false;
        }
    }
    
    public void AddItemToInventory(DisplayInventory inventoryPanel, ItemObject _item, int _amount)
    {

        if (existingSlot != null)
        {
            existingSlot.AddAmount(_amount);

        }
        else if (container.Count < maxContainerSize)
        {
            InventorySlot emptySlot = new InventorySlot();

            emptySlot.item = _item;
            emptySlot.AddAmount(_amount);
            container.Add(emptySlot);
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