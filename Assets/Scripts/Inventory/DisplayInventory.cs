using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    
    public List<GameObject> slots = new List<GameObject>();

    private void Start()
    {
        ClearSlots();
    }

    private void ClearSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            slots[i].GetComponentInChildren<Text>().text = "";
        }
    }

    public void UpdateDisplay(InventoryObjects inventoryInfo)
    {
        // 在这里执行接收到事件时的逻辑
        ClearSlots();
        
        for (int i = 0; i < inventoryInfo.container.Count; i++)
        {
            Transform slot = transform.GetChild(i);
            slot.GetChild(0).GetComponent<Image>().sprite = inventoryInfo.container[i].item.itemImage;
            slot.GetComponentInChildren<Text>().text = inventoryInfo.container[i].amount.ToString();
        }
        
        
    }
}
