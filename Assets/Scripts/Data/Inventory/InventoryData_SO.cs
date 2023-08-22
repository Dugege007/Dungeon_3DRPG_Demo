using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：库存数据
 * 创建时间：2023年4月2日14:59:28
 */

namespace Dungeon_3DRPG_Demo
{
    [System.Serializable]
    public class InventroyItem
    {
        public ItemData_SO itemData;
        public int amount;
    }

    [CreateAssetMenu(fileName = "New InventoryData", menuName = "Inventory/Inventory Data")]
    public class InventoryData_SO : ScriptableObject
    {
        public List<InventroyItem> items = new List<InventroyItem>();

        public void AddItem(ItemData_SO newItemData, int amount)
        {
            bool isFound = false;

            if (newItemData.isStackable && InventoryManager.Instance.canStack)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].itemData == newItemData)
                    {
                        items[i].amount += amount;
                        isFound = true;
                        break;
                    }
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemData == null && !isFound)
                {
                    items[i].itemData = newItemData;
                    items[i].amount = amount;
                    break;
                }
            }
        }
    }
}
