using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObjects : ScriptableObject
{
    public GameObject inventoryPrefab;
    public List<InventorySlot> container = new List<InventorySlot>();
    
    // public bool changed;
    public bool isFull;

    public void AddItem(ItemObject _item, int _amount)
    {
        int maxContainerSize = inventoryPrefab.transform.childCount;
        
        
        InventorySlot existingSlot = container.Find(id => id.item == _item);
        
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
            isFull = true;
            Debug.Log("Inventory is full");
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