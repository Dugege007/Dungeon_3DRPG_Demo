/*
 * 创建人：杜
 * 功能说明：命令模式接口
 * 
 * 用于处理交互逻辑
 * 分担 Controller 的交互逻辑的职责，让很多比较混乱的交互逻辑代码从 Controller 迁移到 Command 中
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ICommand : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanGetUtility,ICanSendEvent,ICanSendCommand
    {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;

        void ICommand.Execute()
        {
            OnExecute();
        }

        IArchitecture IBelongToArchitecture.GetArchiteccture()
        {
            return mArchitecture;
        }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }

        protected virtual void OnExecute() { }
    }
}
