using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour   // 不要弄成singleton
{
    [SerializeField] private List<GameObject> slots = new List<GameObject>();

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

    public void UpdateDisplay(Player currentPlayer)
    {
        ClearSlots();
        
        List<InventorySlot> container = currentPlayer.inventory.container;

        for (int i = 0; i < container.Count; i++)
        {
            Transform slot = transform.GetChild(i);
            slot.GetChild(0).GetComponent<Image>().sprite = container[i].item.itemImage;
            slot.GetComponentInChildren<Text>().text = container[i].amount.ToString();
        }
        
        
    }
}
