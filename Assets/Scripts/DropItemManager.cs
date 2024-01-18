using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // 可能要搬
public class DropItemData
{
    public ItemObject item;
    public int minAmount;
    public int maxAmount;
    
    
    public static List<DropItem> CalculateDropItems(Resource resource)    // 改成Resource
    {
        List<DropItem> result = new List<DropItem>();

        for (int i = 0; i < resource.dropItemsPossibility.Count; i++)
        {
            int amount = Random.Range(resource.dropItemsPossibility[i].minAmount,
                resource.dropItemsPossibility[i].maxAmount + 1);
            
            if (amount > 0)
            {
                DropItem newItem = new DropItem(resource.dropItemsPossibility[i].item, amount);
                result.Add(newItem);
            }
            
        }
        

        return result;
    }
    
}

public class DropItem
{
    public ItemObject item;
    public int amount;
    
    public DropItem(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
}
