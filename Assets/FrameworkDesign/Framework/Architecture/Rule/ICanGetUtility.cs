using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：可获取 Utility 规则
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ICanGetUtility : IBelongToArchitecture
    {

    }

    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this ICanGetUtility self) where T : class, IUtility
        {
            return self.GetArchiteccture().GetUtility<T>();
        }
    }
}
