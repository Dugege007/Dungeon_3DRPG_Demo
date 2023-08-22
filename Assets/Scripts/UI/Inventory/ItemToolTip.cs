using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 创建人：杜
 * 功能说明：物品信息提示
 * 创建时间：2023年4月2日15:54:52
 */

namespace Dungeon_3DRPG_Demo
{
    public class ItemToolTip : MonoBehaviour
    {
        public Text itemNameText;
        public Text itemIDText;
        public Text itemInfoText;

        private RectTransform rectTrans;

        private void Awake()
        {
            rectTrans = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            UpdatePosition();
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        private void Update()
        {
            UpdatePosition();
        }

        public void SetToolTip(ItemData_SO itemData)
        {
            itemNameText.text = itemData.itemName;
            itemIDText.text = "ID: " + itemData.itemID.ToString();
            itemInfoText.text = itemData.description;
        }

        public void UpdatePosition()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3[] corners = new Vector3[4];
            rectTrans.GetWorldCorners(corners);
            float width = corners[3].x - corners[0].x;
            float height = corners[1].y - corners[0].y;

            if (mousePos.y < height && mousePos.x > width)
                rectTrans.position = mousePos + Vector3.up * height * 0.51f + Vector3.left * width * 0.51f;
            else if (mousePos.y > height && mousePos.x < width)
                rectTrans.position = mousePos + Vector3.down * height * 0.51f + Vector3.right * width * 0.51f;
            else if (mousePos.y < height && mousePos.x < width)
                rectTrans.position = mousePos + Vector3.up * height * 0.51f + Vector3.right * width * 0.51f;
            else
                rectTrans.position = mousePos + Vector3.down * height * 0.51f + Vector3.left * width * 0.51f;
        }
    }
}
