using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventResource : MonoBehaviour
{
    
    // 所需要的時間
    public int hours;
    public int minutes;
    public int seconds;
    
    
    // 新功能
    public List<DropItemData> dropItemsPossibility;     // 可能掉落的物品及數量
    public List<DropItem> CalculateDropItems()
    {
        List<DropItem> result = new List<DropItem>();

        for (int i = 0; i < dropItemsPossibility.Count; i++)
        {
            int amount = Random.Range(dropItemsPossibility[i].minAmount, dropItemsPossibility[i].maxAmount + 1);
            DropItem newItem = new DropItem(dropItemsPossibility[i].item, amount);
            if (amount > 0)
            {
                result.Add(newItem);
            }
            
        }
        Debug.Log("Drop item count: " + result.Count);

        return result;
    }
    
    
    
}
