using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：可获取 System 规则
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ICanGetSystem : IBelongToArchitecture
    {

    }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchiteccture().GetSystem<T>();
        }
    }
}
