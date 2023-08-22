using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：基于类型的事件机制
 * 
 * 凉鞋老师的框架
 * 可以让事件的使用更加轻松
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    // 由于注册事件后，在销毁物体时要注销对象，步骤比较繁琐
    // 为了简化操作，引入一个 IUnRegister 接口
    public interface IUnRegister
    {
        void UnRegister();
    }

    // 使用结构体会好一些...
    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;

        public void UnRegister()
        {
            TypeEventSystem.UnRegister(OnEvent);

            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    // 注销的触发器
    // 把这个脚本挂载到一个游戏物体上，当它销毁时，会把需要注销的事件全部注销
    // 这样无需我们一个个的手动注销了
    public class UnRegisterDestoryTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    // 为了简化使用...
    public static class UnRegisterExtension
    {
        public static void UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject obj)
        {
            var trigger = obj.GetComponent<UnRegisterDestoryTrigger>();

            if (trigger == null)
            {
                trigger = obj.AddComponent<UnRegisterDestoryTrigger>();
            }

            trigger.AddUnRegister(unRegister);
        }
    }

    // 事件机制本身就是一个数据结构，类似字典
    public class TypeEventSystem : ITypeEventSystem
    {
        // 通过 Register 方法完成一次注册，这个接口会有多次注册
        // 一个消息的关键字会有多次注册
        public interface IRegistrations
        {

        }

        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }

        // 首先注册需要一个字典
        Dictionary<Type, IRegistrations> mEventRegistration = new Dictionary<Type, IRegistrations>();

        // 注册过程
        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {

            }
            else
            {
                registrations = new Registrations<T>();
                mEventRegistration.Add(type, registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }

        // 发送
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }

        // 发送
        public void Send<T>(T e)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent(e);
            }
        }

        // 注销过程
        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (mEventRegistration.TryGetValue(type, out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}
