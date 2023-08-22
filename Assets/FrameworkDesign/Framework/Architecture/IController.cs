using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：IController 接口
 * 
 * 由于表现层对象时常进行创建和销毁，所以表现层的对象注册到 Architecture 是没有意义的
 * 这里定义 IController 接口的意义就是标记一下这个表现层的对象是属于表现层的
 * 而在表现层对象中，我们去访问 Architecture 中的 System 或者 Model 就不需要用单例的形式获取了
 * 
 * 创建时间：
 */

namespace FrameworkDesign
{
    public interface IController : IBelongToArchitecture, ICanSendCommand, ICanGetSystem, ICanGetModel,ICanRegisterEvent
    {

    }
}
