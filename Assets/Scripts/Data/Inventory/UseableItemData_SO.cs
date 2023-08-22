using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：消耗品数据
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    [CreateAssetMenu(fileName = "New EffectData", menuName = "Inventory/Useable Item Effect Data")]
    public class UseableItemData_SO : ScriptableObject
    {
        public int maxHpUp;
        public int HpUp;
    }
}
