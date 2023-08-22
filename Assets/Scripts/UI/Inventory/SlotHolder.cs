using FrameworkDesign;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 创建人：杜
 * 功能说明：物品格控制
 * 创建时间：2023年4月2日15:24:52
 */

namespace Dungeon_3DRPG_Demo
{
    public class SlotHolder : MonoBehaviour,IController, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public ItemUI itemUI;
        private IItemSystem mItemSystem;

        public IArchitecture GetArchiteccture()
        {
            return DungeonRPG.Interface;
        }

        private void Awake()
        {
            mItemSystem = this.GetSystem<IItemSystem>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemUI.GetItem())
            {
                InventoryManager.Instance.toolTip.SetToolTip(itemUI.GetItem());
                InventoryManager.Instance.toolTip.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InventoryManager.Instance.toolTip.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (PointerEventData.InputButton.Right == eventData.button)
            {
                UseItem();
            }
        }

        public void UpdateItem()
        {
            itemUI.Bag = InventoryManager.Instance.bagData;
            var item = itemUI.Bag.items[itemUI.Index];
            itemUI.SetItemUI(item.itemData, item.amount);
        }

        private void UseItem()
        {
            if (!itemUI.GetItem())
                return;

            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.GetInventoryItemAmount() > 0)
            {
                this.SendCommand<UseItemCommand>();

               
                itemUI.Bag.items[itemUI.Index].amount--;
            }
            UpdateItem();
        }
    }
}
