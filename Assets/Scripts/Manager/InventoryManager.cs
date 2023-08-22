using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：背包管理
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    public class InventoryManager : Singleton<InventoryManager>
    {
        public InventoryData_SO bagTempData;
        public InventoryData_SO bagData;

        public ContainerUI bagUI;

        public ItemToolTip toolTip;

        public DragData currentDragData;
        public Canvas dragCanvas;

        private bool isBagOpen;

        public bool canStack;

        protected override void Awake()
        {
            base.Awake();
            if (bagTempData != null)
                bagData = Instantiate(bagTempData);
        }

        private void Start()
        {
            bagUI.RefreshUI();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                isBagOpen = !isBagOpen;
                bagUI.gameObject.SetActive(isBagOpen);
            }

            if (Input.GetMouseButtonDown(1) && !CheckInventoryUI(Input.mousePosition))
            {
                isBagOpen = false;
                bagUI.gameObject.SetActive(false);
            }
        }

        public bool CheckInventoryUI(Vector3 mousePos)
        {
            RectTransform rectTrans = (RectTransform)bagUI.transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mousePos))
                return true;
            return false;
        }

        public bool CheckBagUI(Vector3 mousePos)
        {
            for (int i = 0; i < bagUI.slotHolders.Length; i++)
            {
                RectTransform rectTrans = (RectTransform)bagUI.slotHolders[i].transform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mousePos))
                    return true;
            }
            return false;
        }

        public void SwitchStackable()
        {
            canStack = !canStack;
        }
    }
}
