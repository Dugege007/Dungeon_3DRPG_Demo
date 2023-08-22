using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：可发送命令 规则
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface ICanSendCommand : IBelongToArchitecture
    {

    }

    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetArchiteccture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetArchiteccture().SendCommand<T>(command);
        }
    }
}
