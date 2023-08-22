using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：攻击相关数据
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    [CreateAssetMenu(fileName = "New AttackData", menuName = "Attack Data")]
    public class AttackData_SO : ScriptableObject
    {
        [Header("攻击范围")]
        public float attackRange;

        [Header("技能范围")]
        public float skillRange;

        [Header("攻击冷却")]
        public float attackCD;

        [Header("技能冷却")]
        public float skillCD;

        [Header("伤害值")]
        public int minDamage;
        public int maxDamage;

        [Header("暴击率")]
        public float criticalChance;

        [Header("暴击加成")]
        public float criticalMultiplier;

        public void AddWeaponData(AttackData_SO weapon)
        {
            attackRange += weapon.attackRange;
            skillRange += weapon.skillRange;
            attackCD += weapon.attackCD;
            skillCD += weapon.skillCD;
            minDamage += weapon.minDamage;
            maxDamage += weapon.maxDamage;
            criticalChance += weapon.criticalChance;
            criticalMultiplier += weapon.criticalMultiplier;
        }

        public void ReduceWeaponData(AttackData_SO weapon)
        {
            attackRange -= weapon.attackRange;
            skillRange -= weapon.skillRange;
            attackCD += weapon.attackCD;
            skillCD += weapon.skillCD;
            minDamage -= weapon.minDamage;
            maxDamage -= weapon.maxDamage;
            criticalChance -= weapon.criticalChance;
            criticalMultiplier -= weapon.criticalMultiplier;
        }
    }
}