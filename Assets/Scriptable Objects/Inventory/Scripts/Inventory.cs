using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    [SerializeField] private int maxContainerSize;
    
    
    public bool CheckSlotAvailable(List<DropItem> itemList)     // 第一次數量不對
    {
        
        int slotsHavingItem = container.Count;
        

        for (int i = 0; i < itemList.Count; i++)
        {
            if (container.Find(slotInfo => slotInfo.item == itemList[i].item) != null)
            {
                Debug.Log("existingSlots++");
            }
            else if (slotsHavingItem < maxContainerSize)
            {
                slotsHavingItem++;
                Debug.Log("slotsHavingItem++");
            }
            else
            {
                Debug.Log("Inventory is full");
                return false;
            }
            
        }
        
        
        return true;    // slotsHavingItem >= maxContainerSize
        
    }
    
    public void AddItemToInventory(List<DropItem> itemList) //
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            InventorySlot existSlot = container.Find(slotInfo => slotInfo.item == itemList[i].item);
            
            if (existSlot != null)
            {
                existSlot.AddAmount(itemList[i].amount);
            }
            else if (container.Count < maxContainerSize)
            {
                InventorySlot emptySlot = new InventorySlot();
                emptySlot.item = itemList[i].item;
                emptySlot.AddAmount(itemList[i].amount);
                container.Add(emptySlot);
            }
            
            
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