using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：道具栏UI
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class ContainerUI : MonoBehaviour
    {
        public SlotHolder[] slotHolders;

        public void RefreshUI()
        {
            for (int i = 0; i < slotHolders.Length; i++)
            {
                slotHolders[i].itemUI.Index = i;
                slotHolders[i].UpdateItem();
            }
        }
    }
}
