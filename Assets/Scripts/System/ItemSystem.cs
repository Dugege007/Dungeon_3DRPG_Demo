using FrameworkDesign;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public interface IItemSystem : ISystem
    {
        UseableItemData_SO mTempUseableItemData { get; }

        // 最大生命
        BindableProperty<int> MaxHpUp { get; }
        // 当前生命
        BindableProperty<int> CurrentHpUp { get; }
    }

    public class ItemSystem : AbstractSystem, IItemSystem
    {
        public UseableItemData_SO mTempUseableItemData { get; }

        public BindableProperty<int> MaxHpUp { get; } = new BindableProperty<int>()
        { Value = 0 };

        public BindableProperty<int> CurrentHpUp { get; } = new BindableProperty<int>()
        { Value = 0 };


        protected override void OnInit()
        {
            var playerModel = this.GetModel<IPlayerModel>();

            this.RegisterEvent<OnUseItemEvent>(e =>
            {
                playerModel.MaxHP.Value += MaxHpUp.Value;
            });
        }
    }
}
