using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class UseItemCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.SendEvent<OnUseItemEvent>();
        }
    }
}
