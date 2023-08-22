using UnityEngine;
using UnityEngine.UI;

/*
 * 创建人：杜
 * 功能说明：物品UI
 * 创建时间：2023年4月2日15:16:47
 */

namespace Dungeon_3DRPG_Demo
{
    public class ItemUI : MonoBehaviour
    {
        public Image icon = null;
        public Text amount = null;

        public InventoryData_SO Bag { get; set; }
        public int Index { get; set; } = -1;

        public void SetItemUI(ItemData_SO newItemData, int newAmount)
        {
            // 清空物品格
            if (newAmount == 0)
            {
                Bag.items[Index].itemData = null;
                icon.gameObject.SetActive(false);
                return;
            }

            if (newItemData != null)
            {
                icon.sprite = newItemData.icon;
                amount.text = newAmount.ToString();
                icon.gameObject.SetActive(true);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
        }

        public ItemData_SO GetItem()
        {
            return Bag.items[Index].itemData;
        }

        public int GetInventoryItemAmount()
        {
            return Bag.items[Index].amount;
        }
    }
}
