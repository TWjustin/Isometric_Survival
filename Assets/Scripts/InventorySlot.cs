using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    
    [HideInInspector] public Button button;
    
    [HideInInspector] public ItemObject item;
    [HideInInspector] public Player player;
    
    
    private void Awake()
    {
        button = GetComponent<Button>();
        
    }
    
    public void Equip()
    {
        player.heldItem = item;

        player.heldItemImage.sprite = transform.GetChild(0).GetComponent<Image>().sprite;
    }
    
    
    
}
