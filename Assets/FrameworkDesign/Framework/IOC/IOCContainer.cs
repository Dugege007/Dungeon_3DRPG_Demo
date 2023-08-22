using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：IOC容器
 * 
 * Inversion of Control 控制反转
 * 也称依赖注入（Dependency Injection，DI）
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public class IOCContainer
    {
        Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        // 注册 需要应用单例的类 和 该单例对象 的键值对
        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        // 获取可以使用单例的类
        public T Get<T>() where T : class
        {
            var key = typeof(T);

            object retInstance;
            if (mInstances.TryGetValue(key, out retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
}
