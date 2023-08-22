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
    public static class ExtensionMethod
    {

        private const float dotThreshold = 0.5f;

        /// <summary>
        /// 检查是否面向目标
        /// </summary>
        /// <param name="transform">被检查对象</param>
        /// <param name="target">目标对象</param>
        /// <returns>是否面向目标</returns>
        public static bool IsFacingTarget(this Transform transform, Transform target)
        {
            var vectorToTarget = target.position - transform.position;
            vectorToTarget.Normalize();

            float dot = Vector3.Dot(transform.forward, vectorToTarget);

            return dot >= dotThreshold;
        }

        /// <summary>
        /// 深度查找子对象transform引用
        /// </summary>
        /// <param name="root">父对象</param>
        /// <param name="childName">具体查找的子对象名称</param>
        /// <returns></returns>
        public static Transform DeepFindChild(this Transform root, string childName)
        {
            Transform result;
            result = root.Find(childName);
            if (result == null)
            {
                foreach (Transform item in root)
                {
                    result = DeepFindChild(item, childName);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return result;
        }
    }
}
