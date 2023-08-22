using Dungeon_3DRPG_Demo;
using FrameworkDesign;

/*
 * 创建人：杜
 * 功能说明：游戏管理
 * 创建时间：
 */

public class DungeonRPG : Architecture<DungeonRPG>
{
    protected override void Init()
    {
        // System
        RegisterSystem<IItemSystem>(new ItemSystem());

        // Model
        RegisterModel<IPlayerModel>(new PlayerModel());
        RegisterModel<IDialogueModel>(new DialogueModel());

        // Utility
        RegisterUtility<IStorage>(new PlayerPrefsStorage());
    }
}