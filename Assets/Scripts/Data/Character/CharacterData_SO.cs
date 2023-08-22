using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    [CreateAssetMenu(fileName = "New CharacterData", menuName = "Character Data")]
    public class CharacterData_SO : ScriptableObject
    {
        [Header("生命")]
        public int MaxHP;
        public int CurrentHP;

        [Header("防御")]
        public int BaseDEF;
        public int CurrentDEF;

        [Header("被击杀可获分数")]
        public int KillPoint;

        [Header("等级")]
        public int MaxLV;
        public int CurrentLV;

        [Header("经验")]
        public int BaseEXP;
        public int CurrentEXP;

        [Header("升级经验加成")]
        public float LevelBuff;
        public float LevelMultiplier
        {
            get { return 1 + (CurrentLV - 1) + LevelBuff; }
        }

        [Header("Position")]
        public Vector3 position;

        public void UpdateExp(int point)
        {
            CurrentEXP += point;
            if (CurrentEXP >= BaseEXP)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            //所有需要提升数据的方法都在这里
            CurrentLV = Mathf.Clamp(CurrentLV + 1, 0, MaxLV);    // Mathf.Clamp()保证currentLevel + 1 在 0~20之间
            BaseEXP += (int)(MaxHP * LevelMultiplier);

            MaxHP = (int)(MaxHP * LevelMultiplier);
            CurrentHP = MaxHP;

            Debug.Log("Level Up! " + CurrentLV + "\nMax Health: " + MaxHP);
        }
    }
}
