using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：泛型单例
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get { return instance; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = (T)this;
        }

        public static bool IsInitialzed
        {
            get { return instance != null; }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
