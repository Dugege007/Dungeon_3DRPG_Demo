using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：物品管理
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class ItemManager : MonoBehaviour
    {
        public List<ItemData_SO> weaponItemDataList = new List<ItemData_SO>();

        public List<ItemData_SO> useableItemDataList = new List<ItemData_SO>();

        private void Awake()
        {
            SetItemId();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G)) CreatItem();
        }

        private void SetItemId()
        {
            for (int i = 0; i < useableItemDataList.Count; i++)
            {
                useableItemDataList[i].itemID = i;
            }
        }

        public void CreatItem()
        {
            int randomIndex = Random.Range(0, useableItemDataList.Count);

            InventoryManager.Instance.bagData.AddItem(useableItemDataList[randomIndex], useableItemDataList[randomIndex].amount);
            InventoryManager.Instance.bagUI.RefreshUI();
        }
    }
}
