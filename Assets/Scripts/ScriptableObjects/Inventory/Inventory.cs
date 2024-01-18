using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    public List<InventorySlotObject> container = new List<InventorySlotObject>();

    [SerializeField] private int maxContainerSize;
    
    
    public bool CheckSlotAvailable(List<DropItem> itemList)     // 第一次數量不對
    {
        
        int slotsHavingItem = container.Count;
        

        for (int i = 0; i < itemList.Count; i++)
        {
            if (container.Find(slotInfo => slotInfo.item == itemList[i].item) != null)
            {
                continue;
            }
            else if (slotsHavingItem < maxContainerSize)
            {
                slotsHavingItem++;
            }
            else
            {
                Debug.Log("Inventory is full");
                return false;
            }
            
        }
        
        
        return true;    // slotsHavingItem >= maxContainerSize
        
    }
    
    public void AddItemToInventory(List<DropItem> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            InventorySlotObject existSlotObject = container.Find(slotInfo => slotInfo.item == itemList[i].item);
            
            if (existSlotObject != null)
            {
                existSlotObject.AddAmount(itemList[i].amount);
            }
            else if (container.Count < maxContainerSize)
            {
                InventorySlotObject emptySlotObject = new InventorySlotObject();
                emptySlotObject.item = itemList[i].item;
                emptySlotObject.AddAmount(itemList[i].amount);
                container.Add(emptySlotObject);
            }
            
            
        }
        
        
    }
}

[System.Serializable]
public class InventorySlotObject
{
    public ItemObject item;
    public int amount;
    
    public void AddAmount(int value)
    {
        amount += value;
    }
}