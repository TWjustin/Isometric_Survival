using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObjects : ScriptableObject
{
    public List<InventorySlot> container = new List<InventorySlot>();

    [SerializeField] private int maxContainerSize;
    
    
    public bool CheckSlotAvailable(List<DropItem> itemList)
    {
        //
        int neededSlotLeft = itemList.Count;
        Debug.Log("Total item count: " + neededSlotLeft);
        
        for (int i = 0; i < itemList.Count; i++)
        {
            if (container.Find(slotInfo => slotInfo.item == itemList[i].item) != null)
            {
                neededSlotLeft--;
            }
            else if (container.Count < maxContainerSize)
            {
                neededSlotLeft--;
            }

            if (neededSlotLeft == 0)
            {
                return true;
            }
        }

        Debug.Log("Needed slot left: " + neededSlotLeft);
        return false;
        
    }
    
    public void AddItemToInventory(List<DropItem> itemList)
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