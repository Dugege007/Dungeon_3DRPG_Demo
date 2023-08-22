using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 创建人：杜
 * 功能说明：拖拽物品
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private RectTransform rectTrans;
        private ItemUI currentItemUI;
        private SlotHolder currentHolder;
        private SlotHolder targetHolder;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
            currentItemUI = GetComponent<ItemUI>();
            currentHolder = GetComponentInParent<SlotHolder>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            InventoryManager.Instance.currentDragData = new DragData();
            InventoryManager.Instance.currentDragData.originalHolder = currentHolder;
            InventoryManager.Instance.currentDragData.originalParent = (RectTransform)currentHolder.transform;

            rectTrans.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (IsPointerOnContainerUI(eventData.position))
                {
                    if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                    {
                        targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                        Debug.Log(targetHolder + " 1");
                    }
                    else
                    {
                        targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                        Debug.Log(targetHolder + " 2");
                    }

                    if (targetHolder != InventoryManager.Instance.currentDragData.originalHolder)
                        ExchangeItem();

                    currentHolder.UpdateItem();
                    targetHolder.UpdateItem();
                }
            }

            transform.SetParent(InventoryManager.Instance.currentDragData.originalParent);
            RectTransform rectTrans = (RectTransform)transform;
            rectTrans.offsetMax = Vector3.zero;
            rectTrans.offsetMin = Vector3.zero;
        }

        public void ExchangeItem()
        {
            Debug.Log(targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index]);
            Debug.Log(currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index]);
            var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
            var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
            //Debug.Log(tempItem.itemData);
            //Debug.Log(targetItem.itemData);

            bool isSameItem = tempItem.itemData == targetItem.itemData;

            if (isSameItem && targetItem.itemData.isStackable)
            {
                targetItem.amount += tempItem.amount;
                tempItem.itemData = null;
                tempItem.amount = 0;
            }
            else
            {
                currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
                targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
            }
        }

        public bool IsPointerOnContainerUI(Vector3 mousePos)
        {
            return InventoryManager.Instance.CheckBagUI(mousePos);
        }
    }
}
