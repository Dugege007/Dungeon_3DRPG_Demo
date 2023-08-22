using System;
using System.Collections.Generic;

/*
 * 创建人：杜
 * 功能说明：构建使用 IOC容器 时要重复定义的代码 的泛型基类
 * 
 * 继承 IArchitecture 接口
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()  // 和 Singleton 泛型单例写法一样
    {
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        private bool mInited = false;
        // 缓存需要要初始化的 Model，在 MakeSureArchitecture() 中统一初始化
        private List<IModel> mModels = new List<IModel>();

        private List<ISystem> mSystems = new List<ISystem>();

        private IOCContainer mContainer = new IOCContainer();

        public static Action<T> OnRegisterPatch = architecture => { };

        private static T mArchitecture;

        // 事件系统
        private ITypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public static IArchitecture Interface
        {
            get
            {
                if (mArchitecture == null)
                {
                    MakeSureArchitecture();
                }

                return mArchitecture;
            }
        }

        // 确保对象存在
        private static void MakeSureArchitecture()
        {
            if (mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();

                OnRegisterPatch?.Invoke(mArchitecture);

                // 把缓存的 Model 都初始化
                // Model 层比 System 更加底层，所以 System 可以直接访问 Model 层
                // 所以应确保 Model 层先初始化
                foreach (var model in mArchitecture.mModels)
                {
                    model.Init();
                }

                mArchitecture.mModels.Clear();

                // System 层初始化时可能会访问 Model 层
                foreach (var system in mArchitecture.mSystems)
                {
                    system.Init();
                }

                mArchitecture.mSystems.Clear();

                mArchitecture.mInited = true;
            }
        }

        protected abstract void Init();

        public void RegisterModel<T>(T modelInstance) where T : IModel
        {
            //modelInstance.Architeccture = this;
            modelInstance.SetArchitecture(this);
            mContainer.Register<T>(modelInstance);

            if (!mInited)
                mModels.Add(modelInstance);
            else
                modelInstance.Init();
        }

        public void RegisterUtility<T>(T utilityInstance) where T : IUtility
        {
            mContainer.Register<T>(utilityInstance);
        }

        public void RegisterSystem<T>(T systemInstance) where T : ISystem
        {
            systemInstance.SetArchitecture(this);
            mContainer.Register<T>(systemInstance);

            if (!mInited)
                mSystems.Add(systemInstance);
            else
                systemInstance.Init();
        }

        public T GetModel<T>() where T : class, IModel
        {
            return mContainer.Get<T>();
        }

        // 新增
        public T GetUtility<T>() where T : class, IUtility
        {
            return mContainer.Get<T>();
        }

        public T GetSystem<T>() where T : class, ISystem
        {
            return mContainer.Get<T>();
        }

        public void SendCommand<T>() where T : ICommand, new()
        {
            var command = new T();
            command.SetArchitecture(this);
            command.Execute();
            // 执行完可以去掉这种双向引用
            command.SetArchitecture(null);
        }

        public void SendCommand<T>(T command) where T : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public void SendEvent<T>() where T : new()
        {
            mTypeEventSystem.Send<T>();
        }

        public void SendEvent<T>(T e)
        {
            mTypeEventSystem.Send<T>(e);
        }

        public IUnRegister RegisterEvent<T>(Action<T> onEvent)
        {
            return mTypeEventSystem.Register<T>(onEvent);
        }

        public void UnRegisterEvent<T>(Action<T> onEvent)
        {
            mTypeEventSystem.UnRegister<T>(onEvent);
        }
    }
}
