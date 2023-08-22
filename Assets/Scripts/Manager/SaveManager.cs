using FrameworkDesign;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：保存管理
 * 创建时间：
 */

namespace Dungeon_3DRPG_Demo
{
    public interface IStorage : IUtility
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue = 0);

        void SaveFloat(string key, float value);
        float LoadFloat(string key, float defaultValue = 0);

        void SaveString(string key, string value);
        string LoadString(string key, string defaultValue = null);
    }

    public class PlayerPrefsStorage : IStorage
    {
        public float LoadFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public string LoadString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SaveString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }

    public class SaveManager
    {
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data">游戏数据</param>
        /// <param name="name">数据名称</param>
        private void Save(Object data, string name)
        {
            // 将数据序列化，以Json形式保存
            var jsonData = JsonUtility.ToJson(data, true);

            // 可以将游戏数据和Json以键值对的形式匹配
            PlayerPrefs.SetString(name, jsonData);
            // 保存数据
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="data">游戏数据</param>
        /// <param name="name">数据名称</param>
        private void Load(Object data, string name)
        {
            if (PlayerPrefs.HasKey(name))
            {
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(name), data);
            }
        }
    }
}
