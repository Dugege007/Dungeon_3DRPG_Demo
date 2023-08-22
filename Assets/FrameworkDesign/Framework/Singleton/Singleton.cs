using System;
using System.Reflection;

/*
 * 创建人：杜
 * 功能说明：泛型单例工具
 * 创建时间：
 */

namespace FrameworkDesign
{
    public class Singleton<T> where T : Singleton<T>
    {
        private static T mInstance;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var type = typeof(T);

                    // 利用反射，获取 T 的构造函数数组
                    // BindingFlags 绑定标记，Instance 实例，NonPublic 非public的
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);

                    // 获取第一个无参构造（默认构造）
                    var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                    if (ctor == null)
                    {
                        // 抛出异常
                        throw new Exception("Non Public Constructor Not Found in + " + type.FullName);
                    }

                    mInstance = ctor.Invoke(null) as T;
                }

                return mInstance;
            }
        }
    }
}
