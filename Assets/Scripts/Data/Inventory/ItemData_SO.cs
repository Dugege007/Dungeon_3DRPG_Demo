using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：物品数据
 * 创建时间：2023年4月2日14:03:46
 */

namespace Dungeon_3DRPG_Demo
{
    public enum ItemType
    {
        Weapon, Useable
    }

    [CreateAssetMenu(fileName = "New ItemData", menuName = "Inventory/Item Data")]
    public class ItemData_SO : ScriptableObject
    {
        public ItemType itemType;

        public int itemID;
        public string itemName;
        public Sprite icon;
        public int amount;
        public bool isStackable;
        [TextArea]
        public string description;

        public UseableItemData_SO useableItemData;
        public AttackData_SO weaponData;
    }
}
