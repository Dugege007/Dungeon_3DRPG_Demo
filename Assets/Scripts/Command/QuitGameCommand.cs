using FrameworkDesign;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class QuitGameCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<OnQuitGameEvent>();
        }
    }
}
