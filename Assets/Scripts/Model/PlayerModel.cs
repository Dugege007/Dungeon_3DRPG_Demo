using FrameworkDesign;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：玩家数据
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public interface IPlayerModel : IModel
    {
        // 最大生命
        BindableProperty<int> MaxHP { get; }
        // 当前生命
        BindableProperty<int> CurrentHP { get; }
        // 基本防御
        BindableProperty<int> BaseDEF { get; }
        // 当前防御
        BindableProperty<int> CurrentDEF { get; }
        // 是否暴击
        BindableProperty<bool> IsCritical { get; }
        // 最高等级
        BindableProperty<int> MaxLV { get; }
        // 当前等级
        BindableProperty<int> CurrentLV { get; }
        // 经验条
        BindableProperty<int> BaseEXP { get; }
        // 当前经验
        BindableProperty<int> CurrentEXP { get; }
    }

    public class PlayerModel : AbstractModel, IPlayerModel
    {
        public static CharacterData_SO tempCharacterData;
        public static AttackData_SO tempAttackData;

        public BindableProperty<int> MaxHP { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.MaxHP };

        public BindableProperty<int> CurrentHP { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.CurrentHP };

        public BindableProperty<int> BaseDEF { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.BaseDEF };

        public BindableProperty<int> CurrentDEF { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.CurrentDEF };

        public BindableProperty<bool> IsCritical { get; } = new BindableProperty<bool>()
        { Value = false };

        public BindableProperty<int> MaxLV { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.MaxLV };

        public BindableProperty<int> CurrentLV { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.CurrentLV };

        public BindableProperty<int> BaseEXP { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.BaseEXP };

        public BindableProperty<int> CurrentEXP { get; } = new BindableProperty<int>()
        { Value = tempCharacterData.CurrentEXP };

        protected override void OnInit()
        {
            tempCharacterData = Resources.Load<CharacterData_SO>("Data/CharacterData/Player Data");
            tempAttackData = Resources.Load<AttackData_SO>("Data/CharacterData/AttackData/Player AttackData");

            var storage = this.GetUtility<IStorage>();

            // 读取存储的数据
            MaxHP.Value = storage.LoadInt(nameof(MaxHP), tempCharacterData.MaxHP);
            // 注册事件
            MaxHP.RegisterOnValueChanged(v => storage.SaveInt(nameof(MaxHP), v));

            CurrentHP.Value = storage.LoadInt(nameof(CurrentHP), tempCharacterData.CurrentHP);
            CurrentHP.RegisterOnValueChanged(v => storage.SaveInt(nameof(CurrentHP), v));

            BaseDEF.Value = storage.LoadInt(nameof(BaseDEF), tempCharacterData.BaseDEF);
            BaseDEF.RegisterOnValueChanged(v => storage.SaveInt(nameof(BaseDEF), v));

            CurrentDEF.Value = storage.LoadInt(nameof(CurrentDEF), tempCharacterData.CurrentDEF);
            CurrentDEF.RegisterOnValueChanged(v => storage.SaveInt(nameof(CurrentDEF), v));
        }

        //public int MaxHealth
        //{
        //    get { if (characterData != null) return characterData.maxHealth; else return 0; }
        //    set { characterData.maxHealth = value; }
        //}

        //public int CurrentHealth
        //{
        //    get { if (characterData != null) return characterData.currentHealth; else return 0; }
        //    set { characterData.currentHealth = value; }
        //}

        //public int BaseDefence
        //{
        //    get { if (characterData != null) return characterData.baseDefence; else return 0; }
        //    set { characterData.baseDefence = value; }
        //}

        //public int CurrentDefence
        //{
        //    get { if (characterData != null) return characterData.currentDefence; else return 0; }
        //    set { characterData.currentDefence = value; }
        //}

        #region 战斗相关
        public void TakeDamage(PlayerModel attacker, PlayerModel defener)
        {
            //TODO 伤害计算
        }

        /// <summary>
        /// 当次攻击力
        /// </summary>
        /// <returns>攻击力</returns>
        private int CurrentATK()
        {
            float coreDamage = UnityEngine.Random.Range(tempAttackData.minDamage, tempAttackData.maxDamage);
            if (IsCritical.Value)
            {
                coreDamage *= tempAttackData.criticalMultiplier;
                Debug.Log("暴击：" + coreDamage);
            }
            return (int)coreDamage;
        }

        #endregion

        #region 装备相关


        #endregion

        #region 消耗品相关
        public void MaxHpUp(int value)
        {
            MaxHP.Value += value;
        }

        public void HpUp(int value)
        {
            if (CurrentHP.Value + value <= MaxHP.Value)
                CurrentHP.Value += value;
            else
                CurrentHP.Value = MaxHP.Value;
        }
        #endregion
    }
}
