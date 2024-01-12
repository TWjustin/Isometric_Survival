using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObjects inventory;
    
    public List<GameObject> slots = new List<GameObject>();

    void Update()
    {
        if (inventory.changed)
        {
            UpdateDisplay();
            inventory.changed = false;
        }
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventory.container[i].item.itemImage;
            slots[i].GetComponentInChildren<Text>().text = inventory.container[i].amount.ToString();
        }
    }
    
    
}
