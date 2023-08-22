using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：可获取 Model 规则
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ICanGetModel : IBelongToArchitecture
    {

    }

    public static class CanGetModelExtension
    {
        public static T GetModel<T>(this ICanGetModel self) where T : class, IModel
        {
            return self.GetArchiteccture().GetModel<T>();
        }
    }
}
